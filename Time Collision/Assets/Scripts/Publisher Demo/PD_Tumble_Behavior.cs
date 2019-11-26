using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PD_Tumble_Behavior : MonoBehaviour
{
    public float maxVel = 10.0f;
    public float maxRotVel = 10.0f;

    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();

        rb.velocity = Random.insideUnitSphere * maxVel;
        rb.angularVelocity = Random.insideUnitSphere * maxRotVel;
    }
}
