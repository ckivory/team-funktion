﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPCGrowth : MonoBehaviour
{
    public GameObject Sphere;
    public GameObject Disk;
    public float max;
    public float speed;
    private float timer;
    private Vector3 scale = new Vector3(1, 1, 1);
    void OnEnable()
    {
        timer = 0;
        scale *= speed;
    }

    // Update is called once per frame
    void Update()
    {
        if (timer<max)
        {
            Sphere.transform.localScale += scale/20;
            Disk.transform.localScale += scale/2;
            timer += 1;
        }
    }
}