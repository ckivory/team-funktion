using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_controller : MonoBehaviour
{
    public int itemNumToSpawn;
    public float spawnPointXMin;
    public float spawnPointXMax;
    public float spawnPointZMin;
    public float spawnPointZMax;
    public GameObject pf_itemType1;
    public GameObject pf_itemType2;
    public GameObject pf_itemType3;
    public GameObject pf_itemType4;
    List<GameObject> items;

    // Start is called before the first frame update
    void Start()
    {
        //spawn a number of item when game starts
        for (int i = 0; i < itemNumToSpawn; i++)
        {
            Vector3 spwanPoint = new Vector3(Random.Range(spawnPointXMin, spawnPointXMax), 0.5f, Random.Range(spawnPointZMin, spawnPointZMax));
            GameObject itemToSave = Instantiate(pf_itemType1, spwanPoint, Quaternion.identity);
            //items.Add(itemToSave);
        }

    }

    // Update is called once per frame
    void Update()
    {

    }
}
