using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class Lobby : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private Button _createButton = default;
    [SerializeField]
    private Button _joinButton = default;
    [SerializeField]
    private Button _joinRandom = default;
    [SerializeField]
    private Text c_roomName = default;
    [SerializeField]
    private Text j_roomName = default;

    private void Awake()
    {
		PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.GameVersion = "1";
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        _createButton.interactable = true;
        _joinButton.interactable = true;
        _joinRandom.interactable = true;
    }

    public void CreateRoom()
    {
        PhotonNetwork.CreateRoom(c_roomName.text, new Photon.Realtime.RoomOptions { MaxPlayers = 2});
    }

    public void JoinRoom()
    {
        PhotonNetwork.JoinRoom(j_roomName.text);
    }

    public void JoinRandom()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("Game");
    }
}
