using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_orbitControl : MonoBehaviour
{
    public GameObject Controller;
    public GameObject playerToFollow;
    public bool captured;
    public float xSpread;
    public float zSpread;
    public float ySpread;
    public float rotSpeed;
    public string type;
    float timer = 0;
    Transform centerPoint;


    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (captured)
        {
            GetComponent<BoxCollider>().enabled = false;
            
            centerPoint = playerToFollow.transform;
            timer += Time.deltaTime * rotSpeed;
            Rotate();
        }

    }

    void Rotate()
    {
        float x = -Mathf.Cos(timer) * xSpread;
        float z = Mathf.Sin(timer) * zSpread;
        float y = Mathf.Sin(timer) * ySpread;
        Vector3 pos = new Vector3(x, y, z);
        transform.position = pos + centerPoint.position;
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.gameObject.tag == "Player")
    //    {
    //        playerToFollow = collision.gameObject;
    //        captured = true;
    //        xSpread = 1;
    //        ySpread = 1;
    //        zSpread = ;
    //        rotSpeed = 1;
    //    }


    //}

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            playerToFollow = other.gameObject;
            captured = true;
            xSpread = 1;
            ySpread = 1;
            zSpread = 1;
            rotSpeed = 1;
        }
    }
}
