using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class Scores : MonoBehaviourPunCallbacks
{
    private Text _scoreMaster = default;
    private Text _scoreClient = default;

    private int _scoresMaster = 0;
    private int _scoresClient = 0;

    private AnimationsEvents _animations = default;

    private void Awake()
    {
        _animations = GameObject.Find("AnimationCanvas").GetComponent<AnimationsEvents>();
    }

    private void Start()
    {
        if(PhotonNetwork.IsMasterClient)
        {
            _scoreMaster = GameObject.Find("scoreP1").GetComponent<Text>();
            _scoreClient = GameObject.Find("scoreP2").GetComponent<Text>();
        }
        else
        {
            _scoreMaster = GameObject.Find("scoreP2").GetComponent<Text>();
            _scoreClient = GameObject.Find("scoreP1").GetComponent<Text>();
        }
    }

    [PunRPC]
    public void RaiseMasterScore(int value)
    {
        _scoresMaster += value;
        _scoreMaster.text = _scoresMaster.ToString();
    }

    [PunRPC]
    public void RaiseClientScore(int value)
    {
        _scoresClient += value;
        _scoreClient.text = _scoresClient.ToString();
    }

    [PunRPC]
    public void ReduceMasterScore(int value)
    {
        _scoresMaster -= value;
        _scoreMaster.text = _scoresMaster.ToString();
    }

    [PunRPC]
    public void ReduceClientScore(int value)
    {
        _scoresClient -= value;
        _scoreClient.text = _scoresClient.ToString();
    }

    public void ResetScores()
    {
        _scoresMaster = 0;
        _scoresClient = 0;
        _scoreMaster.text = _scoresMaster.ToString();
        _scoreClient.text = _scoresClient.ToString();
    }

    public void CheckScores()
    {
        int goal = StageSystem.CurrentStage.Goal;

        if(_scoresMaster >= goal)
        {
            if(_scoresClient >= goal)
            {
                _animations.ShowNextRound();
            }
            else
            {
                if(PhotonNetwork.IsMasterClient)
                {
                    _animations.ShowVictory();
                }
                else
                {
                    _animations.ShowLoss();
                }
            }
        }
        else
        {
            if(_scoresClient >= goal)
            {
                if (PhotonNetwork.IsMasterClient)
                {
                    _animations.ShowLoss();
                }
                else
                {
                    _animations.ShowVictory();
                }
            }
            else
            {
                _animations.ShowLosers();
            }
        }
    }
}
