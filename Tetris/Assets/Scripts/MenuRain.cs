using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuRain : MonoBehaviour
{
    private int _index = 0;

    private Image _rain;

    public Sprite[] Rain;

    private void Awake()
    {
        _rain = GetComponent<Image>();
    }

    private IEnumerator Start()
    {
        while(true)
        {
            _rain.sprite = Rain[_index];
            _index++;
            if (_index >= Rain.Length -1) _index = 0;
            yield return new WaitForSeconds(0.05f);
        }
    }
}
