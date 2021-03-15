using System.Collections.Generic;
using UnityEngine;

public class ObjectsPool : MonoBehaviour
{
    public RectTransform Pool = default;

    public List<GameObject> Drops = default;

    private void Awake()
    {
        Pool = GameObject.Find("Pool").GetComponent<RectTransform>();
        Drops = new List<GameObject>(Pool.childCount);
    }

    private void Start()
    {
        for (int i = 0; i < Pool.childCount; i++)
        {
            Drops.Add(Pool.GetChild(i).gameObject);
        }
    }

    public void ReturnToPool(GameObject poolItem)
    {
        poolItem.GetComponent<RectTransform>().anchoredPosition = Pool.anchoredPosition;
        Drops.Add(poolItem);
    }

    public GameObject GetItem()
    {
        int id = Random.Range(0, Drops.Count);
        GameObject item = Drops[id];
        Drops.RemoveAt(id);
        return item;
    }
}
