using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class GameLobby : MonoBehaviourPunCallbacks
{
    private Button _gameStartButton = default;
    private GameObject _readyButton = default;
    private PhotonView _animations = default;

    private Text _enemyNickname = default;
    private Text _roomName = default;

    private void Awake()
    {
        _gameStartButton = GameObject.Find("GameStartButton").GetComponent<Button>();
        _readyButton = GameObject.Find("ReadyButton");
        _animations = GameObject.Find("AnimationCanvas").GetPhotonView();
        _enemyNickname = GameObject.Find("EnemyNickname").GetComponent<Text>();
        _roomName = GameObject.Find("RoomName").GetComponent<Text>();
    }

    private void Start()
    {
        _gameStartButton.enabled = false;
        _roomName.text = PhotonNetwork.CurrentRoom.Name;

        if (PhotonNetwork.IsMasterClient)
        {
            _readyButton.SetActive(false);
        }
        else
        { 
            _gameStartButton.gameObject.SetActive(false);
            gameObject.GetPhotonView().RPC("SetEnemyNick", RpcTarget.All);
        }
    }

    [PunRPC]
    public void SetEnemyNick()
    {
        if(PhotonNetwork.IsMasterClient)
        {
            _enemyNickname.text = PhotonNetwork.PlayerList[1].NickName;
        }
        else
        {
            _enemyNickname.text = PhotonNetwork.PlayerList[0].NickName;
        }
    }

    public void StartGame()
    {
        _gameStartButton.gameObject.SetActive(false);

        _animations.RPC("StartGame", RpcTarget.All);
    }

    public void LeaveRoom()
    {
        Debug.Log("Disconnecting...");

        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        PhotonNetwork.LoadLevel("Menu");
    }

    public void Ready()
    {
        GetComponent<PhotonView>().RPC("SetReady", RpcTarget.All);
    }

    [PunRPC]
    private void SetReady()
    {
        Game.IsReady = true;
        _gameStartButton.enabled = true;
        _readyButton.SetActive(false);
    }
}
