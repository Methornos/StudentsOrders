using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class Lobby : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private Text c_roomName = default;
    [SerializeField]
    private Text j_roomName = default;

    private void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.GameVersion = "1";
        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.NickName = PlayerPrefs.GetString("Nickname");
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to master");
    }

    public void CreateRoom()
    {
        Debug.Log("Creating room...");
        PhotonNetwork.CreateRoom(c_roomName.text, new Photon.Realtime.RoomOptions { MaxPlayers = 2 });
    }

    public void JoinRoom()
    {
        PhotonNetwork.JoinRoom(j_roomName.text);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Connected to room");
        PhotonNetwork.LoadLevel("Multiplayer");
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log("Failed to join room!" + message);
    }
}
