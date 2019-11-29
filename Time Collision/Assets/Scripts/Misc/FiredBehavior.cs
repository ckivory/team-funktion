using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FiredBehavior : MonoBehaviour
{
    public float xRotate;
    public float yRotate;
    public float zRotate;
    public bool goLeft = false;
    public bool goForward = false;
    private Rigidbody rb;
    public bool spin=false;
    private float timer = 0.2f;
    private float speed = 500f;
    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        gameObject.transform.Rotate(xRotate, yRotate, zRotate);
    }
    void Update()
    {
        if (spin)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                speed -= Time.deltaTime * 180;
            }
            if (speed > 0)
            {
                gameObject.transform.Rotate(Vector3.up * Time.deltaTime * speed);
            }
            else spin = false;
        }
        if (goLeft)
        {
            //rb.AddTorque(-transform.right * Random.Range(minForce, maxForce));
            gameObject.transform.Rotate(Vector3.back * Time.deltaTime * speed);
        }
        if (goForward)
        {
            //rb.AddTorque(-transform.forward * Random.Range(minForce, maxForce));
            gameObject.transform.Rotate(Vector3.left * Time.deltaTime * speed);
        }
    }
}
