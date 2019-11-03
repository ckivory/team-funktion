using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stuck_Behavior : MonoBehaviour
{
    public bool isStuck;
    public float howFar;

    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        if(isStuck)
        {
            rb.isKinematic = true;
            rb.transform.Rotate(180f, 0f, 0f);
            rb.transform.position = new Vector3(rb.transform.position.x, howFar, rb.transform.position.z);
        }
    }

    public void setStuck()
    {
        isStuck = rb.isKinematic = true;
    }
}
