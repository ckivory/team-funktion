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
    /// <summary>
    /// Applied on idle obj
    /// </summary>
    public float torque;
    [HideInInspector]
    GameObject playerToFollow;
    [HideInInspector]
    public bool captured;
    //[HideInInspector]
    public float xSpread = 0;
    //[HideInInspector]
    public float zSpread = 0;
    //[HideInInspector]
    public float ySpread = 0;
    [HideInInspector]
    public float rotSpeed = 0;
    [HideInInspector]
    public bool isBullet;
    public float mass;
    float timer = 0;
    Transform centerPoint;


    void Idle()
    {
        int rn = UnityEngine.Random.Range(0, 3);
        if (rn < 1)
        {
            GetComponent<Rigidbody>().AddTorque(transform.up * torque);
        }
        else if (rn < 2)
        {
            GetComponent<Rigidbody>().AddTorque(transform.forward * torque);
        }
        else if (rn < 3)
        {
            GetComponent<Rigidbody>().AddTorque(transform.right * torque);
        }
        else if (rn < 3)
        {
            GetComponent<Rigidbody>().AddTorque(transform.right * torque);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        Idle();
        //In case mass was not set
        if (mass < 0.001f)
        {
            mass = 1f;
        }
    }
    public void DisableCollision()
    {
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
        if (GetComponent<MeshCollider>() != null)
        {
            GetComponent<MeshCollider>().enabled = false;
        }
        if (!(GetComponent<Rigidbody>() == null))
        {
            Component.Destroy(GetComponent<Rigidbody>());
        }
    }

    public void EnableCollision()
    {
        if (GetComponent<BoxCollider>() != null)
        {
            GetComponent<BoxCollider>().enabled = true;
        }
        if (GetComponent<CapsuleCollider>() != null)
        {
            GetComponent<CapsuleCollider>().enabled = true;
        }
        if (GetComponent<SphereCollider>() != null)
        {
            GetComponent<SphereCollider>().enabled = true;
        }
        if (GetComponent<MeshCollider>() != null)
        {
            GetComponent<MeshCollider>().enabled = true;
        }

        if (GetComponent<Rigidbody>() == null)
        {
            this.gameObject.AddComponent<Rigidbody>();
        }


    }

    // Update is called once per frame
    void Update()
    {
        if (!captured && !isBullet)
        {
            //if (Vector3.Distance())
        }

        if (captured && !isBullet)   //If captured by player
        {
            //DisableCollision();
            DisableCollision();
            //Set center point
            centerPoint = playerToFollow.transform;
            //Set timer
            timer += Time.deltaTime * rotSpeed;
            //Rotate around center point
            Rotate();
        }

        
        //if (isBullet && !captured)
        //{
        //    EnableCollision();
        //}

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

    void CapturedBy(Collider other)
    {
        playerToFollow = other.gameObject;
        float r = 1.1f * playerToFollow.GetComponent<SphereCollider>().radius;
        if (Math.Abs(dynamicVar) / mass <= 1.1 * r)
        {
            dynamicVar = (1.1f * r + dynamicVar) * mass;
        }
        captured = true;
        while (Math.Abs(xSpread) < r)
        {
            xSpread = UnityEngine.Random.Range(-dynamicVar / mass, dynamicVar / mass);
        }
        while (Math.Abs(zSpread) < r)
        {
            zSpread = UnityEngine.Random.Range(-dynamicVar / mass, dynamicVar / mass);
        }
        ySpread = 0;// UnityEngine.Random.Range(-dynamicVar / mass, dynamicVar / mass);
        rotSpeed = UnityEngine.Random.Range(speedMin, speedMax) / mass;

    }

    void hit(Collider other)
    {
        //hit not implemented yet
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && !isBullet)
        {
            CapturedBy(other);
        }

        if (other.tag == "Player" && isBullet)
        {
            hit(other);
        }
    }
}
