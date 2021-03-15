using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class AnimationsEvents : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private Text _round;
    [SerializeField]
    private Text _goal;
    [SerializeField]
    private Text _speed;

    private Animation _anim = default;

    private Rain _rain = default;
    private StageSystem _stageSystem = default;
    private Scores _scores = default;

    private void Awake()
    {
        _anim = GetComponent<Animation>();

        _rain = GameObject.Find("Rain").GetComponent<Rain>();
        _stageSystem = GameObject.Find("StageSystem").GetComponent<StageSystem>();
        _scores = GameObject.Find("Scores").GetComponent<Scores>();
    }

    public void NextStage()
    {
        _stageSystem.NextStage();

        _round.text = "Round " + (StageSystem.CurrentStageId + 1);
        _goal.text = "Goal : " + StageSystem.CurrentStage.Goal;
        _speed.text = "Speed : " + (1 + ((1 - StageSystem.CurrentStage.Speed) * 10));

        _scores.ResetScores();
    }

    [PunRPC]
    public void StartGame()
    {
        ShowNextRound();
    }

    public void StartRound()
    {
        _rain.StartGame();
    }

    public void ShowNextRound()
    {
        _anim.clip = _anim.GetClip("ShowRound");
        _anim.Play();
    }

    public void ShowVictory()
    {
        _anim.clip = _anim.GetClip("ShowVictory");
        _anim.Play();
    }

    public void ShowLoss()
    {
        _anim.clip = _anim.GetClip("ShowLoss");
        _anim.Play();
    }

    public void ShowLosers()
    {
        _anim.clip = _anim.GetClip("ShowLosers");
        _anim.Play();
    }
}
