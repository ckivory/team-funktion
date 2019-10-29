using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiskController : MonoBehaviour
{
    public GameObject player;
    private List<int> inventory;
    private float diskSize;
    public List<GameObject> collectedObjects;
    private List<GameObject> diskObjects;

    private void Start()
    {
       
    }
    private void Update()
    {
        
    }

    void addToDisk(int collectedType)
    {

    }

    public void UpdateDisk()
    {
        Vector3 _diskSize = new Vector3(1,0, 1) * diskSize;
        this.transform.localScale = _diskSize;


    }

}
