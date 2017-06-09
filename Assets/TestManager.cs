using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestManager : MonoBehaviour {
    Map map;
    [SerializeField]
    GameObject dart;
    [SerializeField]
    Tresure tresure;
    Sprite[] s;
	// Use this for initialization
	void Start () {
        map = new Map();
        map.generate();
        StartCoroutine(digdig());
	}


    IEnumerator digdig()
    {
        for (int i = 0; i < 16; i++)
        {
            map.Dig(i / 4, i % 4);
            yield return new WaitForSeconds(0.5f);
        }
    }
	// Update is called once per frame
	void Update () {
	}
}
