using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stuck_Behavior : MonoBehaviour
{
    public bool isStuck;
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        if(isStuck)
        {
            rb.isKinematic = true;
        }
    }

    public void setStuck()
    {
        isStuck = rb.isKinematic = true;
    }
}
