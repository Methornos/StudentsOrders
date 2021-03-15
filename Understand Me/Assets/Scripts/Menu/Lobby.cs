using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class Lobby : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private Button _createButton = default;
    [SerializeField]
    private Button _joinButton = default;
    [SerializeField]
    private InputField _nickname = default;
    //аттрибут SerializeField нужен для отображения этих полей в редакторе

    private void Awake()
    {
        _nickname.text = PlayerPrefs.GetString("Nickname");
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.GameVersion = "1";
        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.NickName = PlayerPrefs.GetString("Nickname");
        //подключение к фотону + загрузка никнейма из реестра
    }

    public override void OnConnectedToMaster()
    {
        _createButton.interactable = true;
        _joinButton.interactable = true;
        //переопределенный метод фотона. Вызывается после подключения к серверам
    }

    public void CreateRoom()
    {
        int roomName = Random.Range(1, 999);
        PhotonNetwork.CreateRoom(roomName.ToString(), new Photon.Realtime.RoomOptions { MaxPlayers = 2});
    }

    public void JoinRoom()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("Game");
    }

    public void SetNickname()
    {
        PlayerPrefs.SetString("Nickname", _nickname.text);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
