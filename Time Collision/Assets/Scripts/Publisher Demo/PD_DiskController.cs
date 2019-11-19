﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PD_DiskController : MonoBehaviour
{
    public GameObject disk;
    GameObject player;
    List<int> inventory;
    float diskSize;
    List<GameObject> collectedPropPrefabs;
    List<GameObject> diskObjects;
    public List<int> orbitalCapacityList;
    public List<int> orbitalRadiusList;
    public List<float> orbitalSpeedList;

    private void Start()
    {
        player = GetComponent<GameObject>();
        inventory = GetComponent<PDPlayerController>().inventory;
        collectedPropPrefabs = GetComponent<PDPlayerController>().collectedPropPrefabs;
        diskObjects = new List<GameObject>();
    }
    private void Update()
    {

    }

    public void AddToDisk(int collectedType)
    {
        int tier = GetTier();
        GameObject objectToAdd = Instantiate(collectedPropPrefabs[collectedType]);
        objectToAdd.AddComponent<OrbitControl>();
        objectToAdd.GetComponent<OrbitControl>().playerToFollow = this.gameObject;
        objectToAdd.GetComponent<OrbitControl>().rotR = orbitalRadiusList[tier];
        objectToAdd.GetComponent<OrbitControl>().rotSpeed = -orbitalSpeedList[tier];
        objectToAdd.GetComponent<OrbitControl>().type = collectedType;
        if (diskObjects.Count > 0)
        {
            objectToAdd.GetComponent<OrbitControl>().timer = diskObjects[diskObjects.Count - 1].GetComponent<OrbitControl>().timer + 2*Mathf.PI / orbitalCapacityList[tier]*orbitalRadiusList[tier];
        }
        diskObjects.Add(objectToAdd);
        ResizeDisk(tier);
    }

    public void RemoveFromDisk(int collectedType)
    {
        GameObject objectToRemove;
        for (int i = diskObjects.Count-1; i >= 0; i--)
        {
            if (diskObjects[i].GetComponent<OrbitControl>().type ==  collectedType)
            {
                objectToRemove = diskObjects[i];
                Object.Destroy(objectToRemove);
                diskObjects.RemoveAt(i);
                break;
            }
        }
        ResizeDisk(GetTier());

    }

    public void UpdateDisk()
    {
       
    }

    int GetTier()
    {
        int n = diskObjects.Count + 1;
        int t = 1;
        for (int i = 0; i < orbitalCapacityList.Count; i++)
        {
            if (n < orbitalCapacityList[i])
            {
                return t;
            }
            else
            {
                n -= orbitalCapacityList[i];
                t++;
            }
        }
        return t;
    }

    void ResizeDisk(float tier)
    {
        disk.transform.localScale = new Vector3(5,1,5)*(1f+tier/10f);
    }
}