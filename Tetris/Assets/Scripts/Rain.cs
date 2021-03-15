using System.Collections;
using UnityEngine;
using Photon.Pun;

public class Rain : MonoBehaviourPunCallbacks
{
    private Transform _dropsCanvas = default;
    private Scores _scores = default;
    private ObjectsPool _pool = default;

    public float SpawnCooldawn = 1;

    private void Awake()
    {
        _dropsCanvas = GameObject.Find("DropsCanvas").GetComponent<Transform>();
        _pool = GameObject.FindWithTag("ObjectsPool").GetComponent<ObjectsPool>();
        _scores = GameObject.Find("Scores").GetComponent<Scores>();
    }

    public void StartGame()
    {
        StartCoroutine(SpawnDrops());
    }

    private IEnumerator SpawnDrops()
    {
        for(int i = 0; i < StageSystem.CurrentStage.TotalDrops; i++)
        {
            GameObject drop = _pool.GetItem();
            RectTransform dropProp = drop.GetComponent<RectTransform>();
            dropProp.SetParent(_dropsCanvas);
            dropProp.localScale = new Vector3(1, 1, 1);
            dropProp.anchoredPosition = new Vector2(Random.Range(-350, 350), 1040);
            drop.GetComponent<Drop>().StartFall(StageSystem.CurrentStage.Speed);

            yield return new WaitForSeconds(SpawnCooldawn);
        }

        _scores.CheckScores();
    }
}
