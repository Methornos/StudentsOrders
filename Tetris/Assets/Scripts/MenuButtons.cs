using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class MenuButtons : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private Animation _animations = default;
    [SerializeField]
    private RectTransform _selectedParent = default;
    [SerializeField]
    private RectTransform _startParent = default;

    //[SerializeField]
    //private RectTransform _singlePlayer = default;
    [SerializeField]
    private RectTransform _multiPlayer = default;
    //[SerializeField]
    //private RectTransform _settings = default;
    [SerializeField]
    private InputField _nickname;

    private bool _isSelected = false;

    private void Start()
    {
        _nickname.text = PlayerPrefs.GetString("Nickname");
    }

    public void ShowMultiplayer()
    {
        if (!_isSelected)
        {
            _isSelected = true;
            _multiPlayer.SetParent(_selectedParent);
            _animations.clip = _animations.GetClip("ShowMultiplayer");
            _animations.Play();
        }
    }

    public void HideMultiplayer()
    {
        _animations.clip = _animations.GetClip("HideMultiplayer");
        _animations.Play();
    }

    public void MultiplayerOut()
    {
        _multiPlayer.SetParent(_startParent);
        _isSelected = false;
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif

        Application.Quit();
    }

    public void SaveNickname()
    {
        PhotonNetwork.NickName = _nickname.text;
        PlayerPrefs.SetString("Nickname", _nickname.text);
    }
}
