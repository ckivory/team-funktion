using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_controller : MonoBehaviour
{
    public GameObject players;
    public int itemNumToSpawn;
    public float spawnPointXMin;
    public float spawnPointXMax;
    public float spawnPointZMin;
    public float spawnPointZMax;
    public GameObject[] itemTypeToSpawn;
    List<GameObject> items;
    GameObject itemToRemove;

    // Start is called before the first frame update
    void Start()
    {
        items = new List<GameObject>();
        //spawn a number of item when game starts
        spawnItem(itemNumToSpawn);

    }

    // Update is called once per frame
    void Update()
    {

        foreach (GameObject obj in items)
        {
            if (obj.GetComponent<scr_orbitControl>().captured)
            {
                itemToRemove = obj;
                spawnItem(1);
                break;
            }
        }
        items.Remove(itemToRemove);
    }

    void spawnItem(int num)
    {
        for (int i = 0; i < num; i++)
        {

            Vector3 spawnPoint = new Vector3(Random.Range(spawnPointXMin, spawnPointXMax), 0.5f, Random.Range(spawnPointZMin, spawnPointZMax));
            while ((Vector3.Distance(spawnPoint, players.transform.position) < 3))
            {
                spawnPoint = new Vector3(Random.Range(spawnPointXMin, spawnPointXMax), 0.5f, Random.Range(spawnPointZMin, spawnPointZMax));
            }
            GameObject itemToSave = Instantiate(itemTypeToSpawn[Random.Range(0, itemTypeToSpawn.Length)], spawnPoint, Quaternion.identity);
            itemToSave.tag = "item";
            items.Add(itemToSave);
        }
    }
}
