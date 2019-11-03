using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPCRotate : MonoBehaviour
{
    public GameObject Sphere;
    public GameObject Disk;

    // Update is called once per frame
    void Update()
    {
        Sphere.transform.Rotate(1, 0, 0, Space.Self);
        Disk.transform.Rotate(0, 1, 0, Space.Self);
    }
}
