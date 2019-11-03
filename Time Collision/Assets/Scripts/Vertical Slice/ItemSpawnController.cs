using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawnController : MonoBehaviour
{
    public List<GameObject> spawnedItems;
    public List<float> minTimes;
    public List<float> maxTimes;
    
    private List<float> timers;

    private void spawnItem(int itemNum)
    {
        Vector3 spawnPosition = new Vector3(0f, 2f, 0f);
        
        while(Physics.CheckSphere(spawnPosition, 1f))
        {
            spawnPosition.x = Random.Range(this.transform.position.x - this.transform.localScale.x / 2, this.transform.position.x + this.transform.localScale.x / 2);
            spawnPosition.z = Random.Range(this.transform.position.z - this.transform.localScale.z / 2, this.transform.position.z + this.transform.localScale.z / 2);

            spawnPosition.y += 0.2f;
        }

        Instantiate(spawnedItems[itemNum], spawnPosition, Quaternion.identity);
    }

    private void Start()
    {
        timers = new List<float>();
        
        for (int i = 0; i < spawnedItems.Count; i++)
        {
            timers.Add(0);
            timers[i] = Random.Range(minTimes[i], maxTimes[i]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i < spawnedItems.Count; i++)
        {
            timers[i] -= Time.deltaTime;
            if(timers[i] <= 0)
            {
                timers[i] = Random.Range(minTimes[i], maxTimes[i]);
                spawnItem(i);
            }
        }
    }
}
