using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class RoomManager : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private Text _guessWord = default;
    [SerializeField]
    private Text _drawCount = default;
    [SerializeField]
    private InputField _wordSetter = default;
    [SerializeField]
    private InputField _countSetter = default;
    [SerializeField]
    private GameObject _wordSetterPanel = default;
    [SerializeField]
    private GameObject _startButton = default;
    [SerializeField]
    private Transform _timer = default;
    [SerializeField]
    private GameObject _ratingPanel = default;
    //все эти поля с аттрибутом [SerializeField] нужно установитьв инспекторе. Там перетягивается объект со сцены(окошко Hierarchy) и закидывается в нужную ячейку
    //значения default нужны для того, чтобы не было предупреждений в консоли, что переменные не проинициализированы(ни на что не влияет, но выглядит как хуйня)

    private PhotonView _view = default;
    private PhotonView _drawingView = default;
    private Drawing _drawing = default;

    public string GuessWord = "";
    public int DrawCount = 0;
    public bool IsMasterArtist = false;

    private void Awake()
    {
        _view = gameObject.GetPhotonView();
        _drawingView = GameObject.FindWithTag("Draw").GetPhotonView();
        _drawing = GameObject.FindWithTag("Draw").GetComponent<Drawing>();
    }

    public void Leave()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        PhotonNetwork.LoadLevel("Menu");
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        _startButton.SetActive(true);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        _startButton.SetActive(false);
    }

    //этот метод вызывается при нажатии кнопки [S E T] панельки, там где вводишь слово и количество линий. Он устанавливает каждому игроку слово которое написал и запускает раунд
    //порядок наших методо: устанавливается слово, запускается раунд, игрок рисует, если он успевает о показывается у второго игрока окошко с оценокй,...
    //после нажатия кнопки оценки запускается новый раунд и второй игрок начинает рисовать и так по кругу
    public void SetGuessWord()
    {
        _view.RPC("SetWord", RpcTarget.All, _wordSetter.text, int.Parse(_countSetter.text));
        _view.RPC("StartRound", RpcTarget.All);
    }

    //запуск раунда. у нас чекается метод, который проверяет, кто следующий рисовать может и запускается корутина таймер(внизу)
    [PunRPC]
    public void StartRound()
    {
        _drawing.ArtistCheck(IsMasterArtist);
        StartCoroutine(Timer());
    }

    public void NextRound()
    {
        if(IsMasterArtist)
        {
            if(!PhotonNetwork.IsMasterClient)
            {
                _wordSetterPanel.SetActive(true);
            }
        }
        else
        {
            if(PhotonNetwork.IsMasterClient)
            {
                _wordSetterPanel.SetActive(true);
            }
        }
    }

    public void PhotonEndRound()
    {
        _view.RPC("EndRound", RpcTarget.All);
    }

    [PunRPC]
    public void EndRound()
    {
        StopAllCoroutines();

        //удаляются все линии со сцены(вызываю два раза, потому что они не удалялись по магической причине). так хотябы надежно чистый холст
        for (int i = 0; i < 2; i++)
        {
            GameObject[] brushes = GameObject.FindGameObjectsWithTag("Brush");

            if (brushes.Length > 0)
            {
                for (int j = 0; j < brushes.Length; j++)
                {
                    Destroy(brushes[j]);
                }
            }
        }

        _guessWord.text = string.Empty;
        GuessWord = string.Empty;
        _drawCount.text = string.Empty;
        DrawCount = 0;

        IsMasterArtist = !IsMasterArtist;

        NextRound();
    }

    //метод вызывается, когда мы отпускаем кнопку мыши(скрипт Drawing, метод OnPointerUp()
    [PunRPC]
    public void ReduceDraw()
    {
        DrawCount--;
        _drawCount.text = DrawCount.ToString();

        if (DrawCount == 0)
        {
            ShowRatingPanel();
            _drawingView.RPC("SetNooneDraw", RpcTarget.All);
        }
    }

    //показ левой панели для оценки
    private void ShowRatingPanel()
    {
        StopAllCoroutines();
        if (IsMasterArtist)
        {
            if (!PhotonNetwork.IsMasterClient)
            {
                _ratingPanel.SetActive(true);
            }
        }
        else
        {
            if (PhotonNetwork.IsMasterClient)
            {
                _ratingPanel.SetActive(true);
            }
        }
    }

    //установка слова и количества линий
    [PunRPC]
    public void SetWord(string word, int count)
    {
        GuessWord = word;
        _guessWord.text = word;
        DrawCount = count;
        _drawCount.text = count.ToString();
    }

    //корутина, которая просто уменьшает наш таймер в игре. Останавливается, если игрок успел нариосвать все заданные линии
    private IEnumerator Timer()
    {
        for (float i = 30; i > 0; i -= 1)
        {
            _timer.localScale = new Vector2(i / 30, 1);
            yield return new WaitForSeconds(1f);
        }

        _view.RPC("EndRound", RpcTarget.All);
    }
}