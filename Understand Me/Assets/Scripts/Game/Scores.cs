using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class Scores : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private Text _hostPlayerNickname = default;
    [SerializeField]
    private Text _hostPlayerScore = default;
    [SerializeField]
    private Text _guestPlayerNickname = default;
    [SerializeField]
    private Text _guestPlayerScore = default;

    private PhotonView _view = default;

    public int HostScores = 0;
    public int GuestScores = 0;

    private void Awake()
    {
        _view = gameObject.GetPhotonView();

        UpdateNickname();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        _guestPlayerNickname.text = newPlayer.NickName;
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        _guestPlayerNickname.text = "Player 2";
    }

    private void UpdateNickname()
    {
        _hostPlayerNickname.text = PhotonNetwork.PlayerList[0].NickName;

        if(!PhotonNetwork.IsMasterClient)
        {
            _guestPlayerNickname.text = PhotonNetwork.NickName;
        }
    }

    public void RaiseScore()
    {
        if(PhotonNetwork.IsMasterClient)
        {
            _view.RPC("RaiseGuestScore", RpcTarget.All);
        }
        else
        {
            _view.RPC("RaiseHostScore", RpcTarget.All);
        }
    }

    [PunRPC]
    public void RaiseHostScore()
    {
        HostScores++;
        _hostPlayerScore.text = HostScores.ToString();
    }

    [PunRPC]
    public void RaiseGuestScore()
    {
        GuestScores++;
        _guestPlayerScore.text = GuestScores.ToString();
    }
}
