using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Photon.Pun;

public class Drop : MonoBehaviourPunCallbacks, IPointerDownHandler
{
    [SerializeField]
    private GameObject _trail = default;

    private AnimationCurve _curve = new AnimationCurve(
        new Keyframe(0, 0, 1, 1),
        new Keyframe(1, 1, 1, 1));

    private ParticleSystem _particles = default;
    private Scores _scores = default;
    private PhotonView _targetView = default;
    private ObjectsPool _pool = default;

    private RectTransform self = default;
    private Image mine = default;
    private TrailRenderer _trailRenderer = default;

    private bool _inPool = false;

    public int State = 0;

    private void Awake()
    {
        self = GetComponent<RectTransform>();
        mine = GetComponent<Image>();
        _trailRenderer = _trail.GetComponent<TrailRenderer>();
        _particles = GameObject.FindWithTag("DropParticles").GetComponent<ParticleSystem>();
        _scores = GameObject.FindWithTag("Scores").GetComponent<Scores>();
        _pool = GameObject.FindWithTag("ObjectsPool").GetComponent<ObjectsPool>();

        _targetView = PhotonView.Get(_scores);

        StateProp();
    }

    private void Update()
    {
        if(self.anchoredPosition.y <= -960)
        {
            Return();
        }
    }

    public void StartFall(float speed) => StartCoroutine(Fall(self.anchoredPosition, new Vector3(self.anchoredPosition.x, -960, 0), speed));

    private IEnumerator Fall(Vector3 origin, Vector3 target, float duration)
    {
        _inPool = false;
        _trail.SetActive(true);
        float currentTime = 0f;
        while (currentTime <= duration
               && !_inPool)
        {
            currentTime = currentTime + Time.deltaTime;
            float percent = Mathf.Clamp01(currentTime / duration);

            float curvePercent = _curve.Evaluate(percent);
            self.anchoredPosition = Vector3.LerpUnclamped(origin, target, curvePercent);

            yield return null;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        CheckState();
        Return();
    }

    private void Return()
    {
        _inPool = true;
        _trail.SetActive(false);
        _particles.transform.position = gameObject.transform.position;
        _particles.Play();
        _pool.ReturnToPool(gameObject);
    }

    private void MinusScore()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            _targetView.RPC("ReduceMasterScore", RpcTarget.All, 1);
        }
        else
        {
            _targetView.RPC("ReduceClientScore", RpcTarget.All, 1);
        }
    }

    private void StateProp()
    {
        switch (State)
        {
            case 1: //do more scores
                mine.color = Color.blue;
                _trailRenderer.startColor = Color.blue;
                _trailRenderer.endColor = Color.blue;
                break;
            case 2: //do enemy less scores
                mine.color = Color.red;
                _trailRenderer.startColor = Color.red;
                _trailRenderer.endColor = Color.red;
                break;
            default:
                mine.color = Color.white;
                _trailRenderer.startColor = Color.white;
                _trailRenderer.endColor = Color.white;
                break;
        }
    }

    private void CheckState()
    {
        switch (State)
        {
            case 1: //do more scores
                if (PhotonNetwork.IsMasterClient) _targetView.RPC("RaiseMasterScore", RpcTarget.All, 2);
                else _targetView.RPC("RaiseClientScore", RpcTarget.All, 2);
                _particles.startColor = Color.blue;
                break;
            case 2: //do enemy less scores
                if (PhotonNetwork.IsMasterClient) _targetView.RPC("ReduceClientScore", RpcTarget.All, 2);
                else _targetView.RPC("ReduceMasterScore", RpcTarget.All, 2);
                _particles.startColor = Color.red;
                break;
            default:
                if (PhotonNetwork.IsMasterClient) _targetView.RPC("RaiseMasterScore", RpcTarget.All, 1);
                else _targetView.RPC("RaiseClientScore", RpcTarget.All, 1);
                _particles.startColor = Color.white;
                break;
        }
    }
}
