using System.Collections;
using UnityEngine;
using Photon.Pun;

public class MeteorsGenerator : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private GameObject _meteorPrefab = default;

    public float SpawnCooldown = 1;

    public void StartGenerate()
    {
        StartCoroutine(Spawn());
    }

    private IEnumerator Spawn()
    {
        float offsetX = Random.Range(-10f, 10f);
        Vector2 position = new Vector2(offsetX, 7);

        PhotonNetwork.Instantiate(_meteorPrefab.name, position, Quaternion.identity);

        yield return new WaitForSeconds(SpawnCooldown);

        StartCoroutine(Spawn());
    }
}
