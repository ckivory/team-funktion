﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public List<KeyCode> controls;

    private Vector3 inputVector;

    private void getInput()
    {
        inputVector = Vector3.zero;

        if (Input.GetKey(controls[0]))      // UP
        {
            inputVector.z += 1;
        }
        if (Input.GetKey(controls[1]))      // LEFT
        {
            inputVector.x -= 1;
        }
        if (Input.GetKey(controls[2]))      // DOWN
        {
            inputVector.z -= 1;
        }
        if (Input.GetKey(controls[3]))      // RIGHT
        {
            inputVector.x += 1;
        }

        inputVector = inputVector.normalized;
    }

    private Rigidbody rb;

    private void updateMovement()
    {
        getInput();

        if(inputVector != Vector3.zero)
        {
            rb.velocity += inputVector;     // Additive controls, so it will intentionally feel a little floaty.
            if(rb.velocity.magnitude > speed)
            {
                rb.velocity = rb.velocity.normalized * speed;
            }
        }
        // Otherwise, they will slowly drift to a stop with a drag of 1.
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        updateMovement();
    }
}
