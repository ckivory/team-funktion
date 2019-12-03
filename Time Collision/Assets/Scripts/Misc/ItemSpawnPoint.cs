using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawnPoint : MonoBehaviour
{
    public GameObject ObjectToRespawn;
    // these are four variable for determine the range of offset when new respawn is created
    public float MinOffsetX;
    public float MaxOffsetX;
    public float MinOffsetZ;
    public float MaxOffsetZ;
    // these are variables that determine the respawn timer based on the min and max value
    public float MinTimer;
    public float MaxTimer;
    private GameObject SpawnedItem;
    private float CurrentTimer;
    private Vector3 OriginalPos;

    private float NewTimer()
    {
        return Random.Range(MinTimer, MaxTimer);
    }
    void OnEnable()
    {
        // getting the respawn timer for the first time
        CurrentTimer = NewTimer();
        OriginalPos = this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {

        if (AllowToSpawn())
        {
            CurrentTimer -= Time.deltaTime;
        }
        if (CurrentTimer <= 0)
        {
            SpawnItem();
        }
    }

    void SpawnItem()
    {
        CurrentTimer = NewTimer();
        Vector3 spawnPoint = OriginalPos + new Vector3(Random.Range(MinOffsetX, MaxOffsetX)*((int)(Random.Range(0,2)*2-1)), 0f, Random.Range(MinOffsetZ, MaxOffsetZ)*((int)(Random.Range(0, 2) * 2 - 1))); ;
        SpawnedItem = Instantiate(ObjectToRespawn, spawnPoint, Quaternion.identity);
    }

    bool AllowToSpawn()
    {
        return SpawnedItem == null || SpawnedItem.GetComponent<scr_orbitControl>().captured;
    }
}
