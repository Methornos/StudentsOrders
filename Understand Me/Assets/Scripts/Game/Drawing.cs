using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Photon.Pun;

public class Drawing : MonoBehaviourPunCallbacks, IPointerDownHandler, IDragHandler, IPointerUpHandler //три интерфейса для обработки нажатия
{
    [SerializeField]
    private GameObject _brush = default;
    [SerializeField]
    private Slider _slider = default;
    [SerializeField]
    public GameObject _drawBlocker = default;

    private PhotonView _view = default;
    private PhotonView _roomView = default;
    private LineRenderer _currentLineRenderer = default;
    private Camera _camera = default;
    private RoomManager _roomManager = default;

    private GameObject brushInstance = default;

    public Color BrushColor = default;
    public float BrushSize = 0.2f;
    public bool IsDrew = true;

    private List<Color> Colors = new List<Color>()
    {
        Color.white,
        Color.black,
        Color.red,
        Color.blue,
        new Color(0, 0.8f, 1),
        Color.green,
        new Color(0, 0.65f, 0),
        new Color(1, 1, 0),
        new Color(1, 0.4f, 0.25f),
        new Color(0.6f, 0.35f, 0.03f),
        new Color(0.64f, 0, 1),
        new Color(1, 0.4f, 1)
    };

    //Awake() запускается при инициализации сцены и всех объектов на ней. Сам по себе запускается и нуен для инициализации полей
    private void Awake()
    {
        _view = gameObject.GetPhotonView();
        
        _camera = Camera.main;
        
        BrushColor = Color.white;
    }

    //Start() тоже сам запускается, но позже чем Awake(). Конкретно здесь использовал для инициализации, потому что были проблемы с null объектами
    private void Start()
    {
        _roomManager = GameObject.Find("RoomManager").GetComponent<RoomManager>();
        _roomView = GameObject.Find("RoomManager").GetPhotonView();
    }

    //метод интерфейса, который вызывается которая кнопка мыши кликнута над объектом, на которо мвисит скрипт(в этой игре объект Canvas -> 1920x1080... -> DrawZone)
    public void OnPointerDown(PointerEventData eventData)
    {
        if (!IsDrew)
        {
            _view.RPC("CreateBrush", RpcTarget.All, _camera.ScreenToWorldPoint(eventData.position));
        }
    }

    //как прошлый метод, только вызывается кода мы кликнули уже и тянем мышь
    public void OnDrag(PointerEventData eventData)
    {
        if (!IsDrew)
        {
            PointToMousePosition(_camera.ScreenToWorldPoint(eventData.position));
        }
    }

    //вызывается когда поднимаем мышь
    public void OnPointerUp(PointerEventData eventData)
    {
        if (!IsDrew)
        {
            _roomView.RPC("ReduceDraw", RpcTarget.All);
        }
    }

    //следующие методы нужны для отрисовки наших линий(не знаю спросит ли, но в общем у нас создается объект на сцене, которому мы устанавливаем начальные параметры)
    //а потом в методе OnDrag() мы вызываем метод, который добавляет точки линии, чтобы она была типо кривой и все такое
    //также эти методы имеют аттрибут [PunRPC], котоырй служит для вызова этих методов у всех клиентов в комнате. Вызывается через PhotonView, который должен висеть на оъекте, который
    //вызывает метод. Без него не будет синхронизации между игроками
    [PunRPC]
    public void CreateBrush(Vector3 position)
    {
        brushInstance = Instantiate(_brush);
        _view.RPC("SetCurrentBrush", RpcTarget.All);

        _currentLineRenderer.startColor = BrushColor;
        _currentLineRenderer.endColor = BrushColor;

        _currentLineRenderer.startWidth = BrushSize;
        _currentLineRenderer.endWidth = BrushSize;

        _currentLineRenderer.SetPosition(0, position);
        _currentLineRenderer.SetPosition(1, position);
    }

    [PunRPC]
    public void SetCurrentBrush()
    {
        _currentLineRenderer = brushInstance.GetComponent<LineRenderer>();
    }

    [PunRPC]
    private void AddPoint(Vector2 pointPos)
    {
        _currentLineRenderer.positionCount++;
        int positionIndex = _currentLineRenderer.positionCount - 1;
        _currentLineRenderer.SetPosition(positionIndex, pointPos);
    }

    [PunRPC]
    public void ChangeColor(int color)
    {
        BrushColor = Colors[color];
    }

    private void PointToMousePosition(Vector3 position)
    {
        Vector2 mousePosition = position;

        _view.RPC("AddPoint", RpcTarget.All, mousePosition);
    }

    public void ChangeBrushSize()
    {
         _view.RPC("BrushSizing", RpcTarget.All, _slider.value);
    }

    [PunRPC]
    public void BrushSizing(float size)
    {
        BrushSize = size;
    }

    //метод, который проверяет каждого игрока, рисует ли он следующий или нет
    public void ArtistCheck(bool isMasterArtist)
    {
        if(isMasterArtist)
        {
            if(PhotonNetwork.IsMasterClient)
            {
                IsDrew = false;
                _drawBlocker.SetActive(false);
            }
            else
            {
                IsDrew = true;
                _drawBlocker.SetActive(true);
            }
        }
        else
        {
            if (!PhotonNetwork.IsMasterClient)
            {
                IsDrew = false;
                _drawBlocker.SetActive(false);
            }
            else
            {
                IsDrew = true;
                _drawBlocker.SetActive(true);
            }
        }
    }

    [PunRPC]
    public void SetNooneDraw()
    {
        IsDrew = true;
        _drawBlocker.SetActive(true);
    }
}
