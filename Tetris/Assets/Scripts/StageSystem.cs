using UnityEngine;
using Photon.Pun;

public class StageSystem : MonoBehaviour
{
    public static int CurrentStageId = -1;

    public static Stage CurrentStage = default;

    public Stage[] Stages = new Stage[10];

    private void Start()
    {
        CurrentStageId = -1;
    }

    public void NextStage()
    {
        CurrentStageId++;
        CurrentStage = Stages[CurrentStageId];
    }
}
