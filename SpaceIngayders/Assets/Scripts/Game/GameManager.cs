using System.Collections;
using UnityEngine;
using Photon.Pun;

public class GameManager : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private GameObject _playerPrefab = default;
    [SerializeField]
    private GameObject _backButton = default;
    [SerializeField]
    private GameObject _gameOver = default;

    private MeteorsGenerator _generator = default;

    private bool _cursor;
    private bool _isStarted;
    private bool _isReady;

    private int PlayerCount = 2;

    private void Awake()
    {
        _generator = GameObject.FindWithTag("Generator").GetComponent<MeteorsGenerator>();
    }

    private void Start()
    {
        Cursor.visible = _cursor;
		
		StartCoroutine(Spawn());
    }

    private void Update()
    {
        if(Input.GetKeyUp(KeyCode.Space))
        {
            if(PhotonNetwork.IsMasterClient)
            {
                if (!_isStarted && _isReady)
                {
                    _isStarted = true;
                    _generator.StartGenerate();
                }
            }
            else
            {
                gameObject.GetPhotonView().RPC("Ready", RpcTarget.All);
            }
        }

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            _cursor = !_cursor;
            Cursor.visible = _cursor;
            _backButton.SetActive(!_backButton.activeSelf);
        }
    }

    [PunRPC]
    public void Ready() => _isReady = true;

    [PunRPC]
    public void KillPlayer()
    {
        PlayerCount--;
        if(PlayerCount == 0)
        {
            GameOver();
        }
    }

    public void Leave()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        PhotonNetwork.LoadLevel("Menu");
    }
	
	private IEnumerator Spawn()
	{
		yield return new WaitForSeconds(1f);
		PhotonNetwork.Instantiate(_playerPrefab.name, new Vector3(Random.Range(-2, 2), -3), Quaternion.identity);
	}

    private void GameOver()
    {
        _gameOver.SetActive(true);
		Cursor.visible = true;
    }
}
