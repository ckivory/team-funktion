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
    public float centerOffset;
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
            if (GetComponent<BoxCollider>()!=null)
            {
                GetComponent<BoxCollider>().enabled = false;
            }
            if (GetComponent<CapsuleCollider>() != null)
            {
                GetComponent<CapsuleCollider>().enabled = false;
            }

            
            centerPoint = playerToFollow.transform;
            timer += Time.deltaTime * rotSpeed;
            this.transform.LookAt(centerPoint);
            Rotate();
        }

    }

    void Rotate()
    {
        float x = -Mathf.Cos(timer) * xSpread;
        float z = Mathf.Sin(timer) * zSpread;
        float y = Mathf.Sin(timer) * ySpread;
        Vector3 pos = new Vector3(x, y, z);
        transform.position = pos + centerPoint.position + new Vector3(centerOffset, centerOffset, centerOffset);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            playerToFollow = other.gameObject;
            captured = true;
        }
    }
}
