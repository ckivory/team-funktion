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
    private List<int> numItems;
    
    private List<float> timers;

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
            GameObject newProp = Instantiate(spawnedItems[itemNum], spawnPosition, Quaternion.identity);
            newProp.transform.Rotate(new Vector3(0f, Random.Range(0f, 360f), 0f));
            newProp.GetComponent<PD_DespawnObject>().parentSpawner = gameObject;
            //Debug.Log("Spawned item at: " + spawnPosition);
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

    public void decrementItem(int propNum)
    {
        int item = itemNumFromPropNum(propNum);
        if(item != -1)
        {
            numItems[item]--;
        }
    }

    private void Start()
    {
        timers = new List<float>();
        numItems = new List<int>();

        for (int i = 0; i < spawnedItems.Count; i++)
        {
            timers.Add(0f);
            timers[i] = Random.Range(minTimes[i], maxTimes[i]);
            
            numItems.Add(0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i < spawnedItems.Count; i++)
        {
            //Debug.Log("Number of timers: " + timers.Count);
            timers[i] -= Time.deltaTime;
            if(timers[i] <= 0)
            {
                if (numItems[i] < maxItems[i])
                {
                    spawnItem(i);
                    numItems[i]++;
                }
                
                timers[i] = Random.Range(minTimes[i], maxTimes[i]);
            }
        }
    }
}
