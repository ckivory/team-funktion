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
    public float yOffset;
    public float rotSpeed;
    float timer = 0;
    Transform centerPoint;

    // Start is called before the first frame update
    void Start()
    {
        xSpread = 10;
        zSpread = 10;
        yOffset = 0;
        rotSpeed = 0.5f;
        captured = true;
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
        Vector3 pos = new Vector3(x, yOffset, z);
        transform.position = pos + centerPoint.position;
    }
}
