using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class Scores : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private Text _score = default;

    public int Score = 0;

    public void RaiseScore(int value)
    {
        Score++;
        _score.text = Score.ToString();
    }
}
