using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_orbitControl : MonoBehaviour
{
    public GameObject Controller;
    public GameObject Player;
    public bool captured;
    public float xSpread;
    public float zSpread;
    public float ySpread;
    public float rotSpeed;
    float timer = 0;
    Transform centerPoint;

    // Start is called before the first frame update
    void Start()
    {
        xSpread = 3;
        zSpread = 3;
        ySpread = 3;
        rotSpeed = 0.5f;
        captured = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (captured)
        {
            centerPoint = Player.transform;
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
}
