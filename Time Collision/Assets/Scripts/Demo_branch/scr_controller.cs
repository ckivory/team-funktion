using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class scr_controller : MonoBehaviour
{
    public GameObject players;
    public int itemNumToSpawn;
    public GameObject[] itemTypeToSpawn;
    public GameObject[] spawnPoints;
    List<GameObject> items;
    GameObject itemToRemove;
    //Legacy code
    //public float spawnPointXMin;
    //public float spawnPointXMax;
    //public float spawnPointZMin;
    //public float spawnPointZMax;
    // these are four variable for determine the range of offset when new respawn is created
    public float MinOffsetX;
    public float MaxOffsetX;
    public float MinOffsetZ;
    public float MaxOffsetZ;

    // these are variables that determine the respawn timer based on the min and max value
    public float MinTimer;
    public float MaxTimer;
    private float CurrentTimer;

    private float NewTimer()
    {
        return Random.Range(MinTimer, MaxTimer);
    }
    void OnEnable()
    {
        // getting the respawn timer for the first time
        CurrentTimer = NewTimer();
    }
    void  applyOffset(ref Vector3 v3)
    {
        v3 += new Vector3(Random.Range(MinOffsetX, MaxOffsetX), 0f, Random.Range(MinOffsetZ, MaxOffsetZ));
    }

    // Start is called before the first frame update
    void Start()
    {
        items = new List<GameObject>();
        //spawn a number of item when game starts
        if (spawnPoints.Length < 2)
        {
            throw new System.ArgumentException("Need at least two spawn points");
        }
        spawnItem(itemNumToSpawn);
    }

    // Update is called once per frame
    void Update()
    {
        CurrentTimer-=Time.deltaTime;
        if (CurrentTimer <= 0f)
        {
            respawnItem();
            CurrentTimer = NewTimer();
        }
    }


    void spawnItem(int num)
    {
        for (int i = 0; i < num; i++)
        {
            Vector3 spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length - 1)].transform.position;
            float distanceToPlayer = Vector3.Distance(spawnPoint, players.transform.position);
            while (distanceToPlayer < 5)
            {
                spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length - 1)].transform.position;
                applyOffset(ref spawnPoint);
            }
            GameObject itemToSave = Instantiate(itemTypeToSpawn[Random.Range(0, itemTypeToSpawn.Length)], spawnPoint, Quaternion.identity);
            itemToSave.tag = "item";
            items.Add(itemToSave);
        }
    }
    /// <summary>
    /// Respawn an item after player captured an item
    /// </summary>
    void respawnItem()
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




    //Legacy code
    //void spawnItem(int num)
    //{
    //    for (int i = 0; i < num; i++)
    //    {

    //        Vector3 spawnPoint = new Vector3(Random.Range(spawnPointXMin, spawnPointXMax), 0.5f, Random.Range(spawnPointZMin, spawnPointZMax));
    //        while ((Vector3.Distance(spawnPoint, players.transform.position) < 3))
    //        {
    //            spawnPoint = new Vector3(Random.Range(spawnPointXMin, spawnPointXMax), 0.5f, Random.Range(spawnPointZMin, spawnPointZMax));
    //        }
    //        GameObject itemToSave = Instantiate(itemTypeToSpawn[Random.Range(0, itemTypeToSpawn.Length)], spawnPoint, Quaternion.identity);
    //        itemToSave.tag = "item";
    //        items.Add(itemToSave);
    //    }
    //}
}
