using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiskController : MonoBehaviour
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
    int tier;

    private void Start()
    {
        player = GetComponent<GameObject>();
        inventory = GetComponent<VSPlayerController>().inventory;
        collectedPropPrefabs = GetComponent<VSPlayerController>().collectedPropPrefabs;
        diskObjects = new List<GameObject>();
    }
    private void Update()
    {
        ResizeDisk();
    }

    public void AddToDisk(int collectedType)
    {

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
        changeTier();
        //ResizeDisk(tier);
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
        changeTier();
        //ResizeDisk(GetTier());

    }

    void changeTier()
    {
        int n = diskObjects.Count + 1;
        for (int i = 0; i < orbitalCapacityList.Count; i++)
        {
            if (n < orbitalCapacityList[i])
            {
                tier = i+1;
                return;
            }
            else
            {
                n -= orbitalCapacityList[i];
            }
        }
        return;
        //tier = t;
    }

    void ResizeDisk()
    {
        Vector3 newSize = new Vector3(5, 1, 5) * (1f + (tier-1f) / 10f);
        Vector3 resizeFactor = (newSize - disk.transform.localScale)/10;
        if(disk.transform.localScale == newSize)
        {
            return;
        }
        disk.transform.localScale += resizeFactor;
    }
}
