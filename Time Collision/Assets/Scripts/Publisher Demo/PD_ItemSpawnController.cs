using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PD_ItemSpawnController : MonoBehaviour
{
    public float baseSpawnHeight = 3.5f;
    public float heightIncrement = 0.2f;

    public List<GameObject> spawnedItems;
    public List<float> minTimes;
    public List<float> maxTimes;

    public List<int> maxItems;
    
    private List<float> timers;

    public List<bool> activeWaves;
    private PD_DeathZoneController deathZone;
    private int currentWave;

    // Pool of objects to pull from so the game doesn't have to create and destroy them constantly.
    private List<List<GameObject>> pool;

    private void spawnItem(int itemNum)
    {
        Vector3 spawnPosition = new Vector3(0f, baseSpawnHeight, 0f);
        spawnPosition.x = Random.Range(this.transform.position.x - this.transform.localScale.x / 2, this.transform.position.x + this.transform.localScale.x / 2);
        spawnPosition.z = Random.Range(this.transform.position.z - this.transform.localScale.z / 2, this.transform.position.z + this.transform.localScale.z / 2);

        int increment = 0;
        int maxIncrement = 100;
        int lMask = 1 << 13;
        while(Physics.CheckSphere(spawnPosition, 1f, lMask) && increment < maxIncrement)
        {
            spawnPosition.x = Random.Range(this.transform.position.x - this.transform.localScale.x / 2, this.transform.position.x + this.transform.localScale.x / 2);
            spawnPosition.z = Random.Range(this.transform.position.z - this.transform.localScale.z / 2, this.transform.position.z + this.transform.localScale.z / 2);

            spawnPosition.y += heightIncrement;
            increment++;
        }

        if(!Physics.CheckSphere(spawnPosition, 1f, lMask))
        {
            GameObject newProp = pool[itemNum][pool[itemNum].Count - 1];
            pool[itemNum].RemoveAt(pool[itemNum].Count - 1);
            newProp.transform.position = spawnPosition;
            newProp.transform.Rotate(new Vector3(0f, Random.Range(0f, 360f), 0f));
            newProp.SetActive(true);
        }
    }
    
    private int itemNumFromPropNum(int propNum)
    {
        for(int i = 0; i < spawnedItems.Count; i++)
        {
            if(spawnedItems[i].GetComponent<ObjectAttributes>().propNum == propNum)
            {
                return i;
            }
        }
        return -1;
    }

    public void AcceptToPool(GameObject g)
    {
        int pn = g.GetComponent<ObjectAttributes>().propNum;
        g.SetActive(false);
        pool[itemNumFromPropNum(pn)].Add(g);
    }

    private void checkWave()
    {
        if(deathZone != null)
        {
            currentWave = deathZone.waveNum;
        }
    }

    private List<List<GameObject>> fillPool()
    {
        List<List<GameObject>> newPool = new List<List<GameObject>>();
        List<GameObject> bucket;
        GameObject current;

        foreach (GameObject firedType in spawnedItems)
        {
            bucket = new List<GameObject>();
            for(int i = 0; i < maxItems.Count; i++)
            {
                current = Instantiate(firedType);
                current.SetActive(false);
                current.GetComponent<PD_DespawnObject>().parentSpawner = gameObject;
                bucket.Add(current);
            }
            newPool.Add(bucket);
        }

        return newPool;
    }

    private void Start()
    {
        timers = new List<float>();

        for (int i = 0; i < spawnedItems.Count; i++)
        {
            timers.Add(0f);
            timers[i] = Random.Range(0f, maxTimes[i]);   // Min of zero to start with means objects can start spawning immediately without waiting for the first min time.
        }

        deathZone = GameObject.FindGameObjectWithTag("DeathZone").GetComponent<PD_DeathZoneController>();
        currentWave = 0;

        pool = fillPool();
    }
    
    void Update()
    {
        checkWave();
        if(activeWaves[currentWave])
        {
            for (int i = 0; i < spawnedItems.Count; i++)
            {
                timers[i] -= Time.deltaTime;
                if (timers[i] <= 0)
                {
                    if (pool[i].Count > 0)
                    {
                        spawnItem(i);
                    }

                    timers[i] = Random.Range(minTimes[i], maxTimes[i]);
                }
            }
        }
    }
}
