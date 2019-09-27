using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_orbitControl : MonoBehaviour
{
    public string type;
    public float dynamicVar;
    public GameObject Controller;
    public float speedMax;
    public float speedMin;
    public float centerOffset;
    [HideInInspector]
    GameObject playerToFollow;
    [HideInInspector]
    public bool captured;
    [HideInInspector]
    public float xSpread=0;
    [HideInInspector]
    public float zSpread=0;
    [HideInInspector]
    public float ySpread=0;
    [HideInInspector]
    public float rotSpeed=0;
    float timer = 0;
    Transform centerPoint;


    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (captured)   //If captured by player
        {
            //Disable all collider
            if (GetComponent<BoxCollider>() != null)
            {
                GetComponent<BoxCollider>().enabled = false;
            }
            if (GetComponent<CapsuleCollider>() != null)
            {
                GetComponent<CapsuleCollider>().enabled = false;
            }
            if (GetComponent<SphereCollider>() != null)
            {
                GetComponent<SphereCollider>().enabled = false;
            }
            //Destroy rigidbody
            Component.Destroy(GetComponent<Rigidbody>());


            //Set center point
            centerPoint = playerToFollow.transform;
            //Set timer
            timer += Time.deltaTime * rotSpeed;
            //Rotate around center point
            Rotate();
        }

    }

    /// <summary>
    /// Rotate this item aound the center point
    /// </summary>
    void Rotate()
    {
        float x = Mathf.Cos(timer) * xSpread;
        float z = Mathf.Sin(timer) * zSpread;
        float y = Mathf.Sin(timer) * ySpread;
        Vector3 pos = new Vector3(x, y, z);
        this.transform.LookAt(centerPoint);
        transform.position = pos + centerPoint.position + new Vector3(centerOffset, centerOffset, centerOffset);
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.tag == "Player")
        {
            playerToFollow = other.gameObject;
            float r = 2*playerToFollow.GetComponent<SphereCollider>().radius;
            captured = true;
            while(Math.Abs(xSpread)<r)
            {
                xSpread = UnityEngine.Random.Range(-dynamicVar, dynamicVar);
            }
            while (Math.Abs(ySpread) <r)
            {
                ySpread = UnityEngine.Random.Range(-dynamicVar, dynamicVar);
            }
            while (Math.Abs(zSpread) <r)
            {
                zSpread = UnityEngine.Random.Range(-dynamicVar, dynamicVar);
            }
            rotSpeed = UnityEngine.Random.Range(speedMin, speedMax);
        }
    }
}
