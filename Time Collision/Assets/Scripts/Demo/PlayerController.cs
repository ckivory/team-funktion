using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Camera cam;
    public float maxSpeed;
    public float accel;
    public int controllerNum;
    public bool usingController;
    public List<KeyCode> controlKeys;
    
    private Vector3 movementInput;
    private List<int> inventory;
    private Rigidbody rb;

    private void getJoystickMovement()
    {
        movementInput = Vector3.zero;
        if(controllerNum > 0)
        {
            movementInput.x = Input.GetAxis("J" + controllerNum + "Horizontal");
            movementInput.z = Input.GetAxis("J" + controllerNum + "Vertical");
        }
        else
        {
            throw new System.Exception("Make sure to set the controller number in the Unity editor");
        }

        if(movementInput.magnitude > 1)
        {
            movementInput = movementInput.normalized;
        }
    }

    private void getKeyboardMovement()
    {
        movementInput = Vector3.zero;
        if(Input.GetKey(controlKeys[0]))       // UP
        {
            movementInput.z += 1;
        }
        if (Input.GetKey(controlKeys[1]))       // LEFT
        {
            movementInput.x -= 1;
        }
        if (Input.GetKey(controlKeys[2]))       // DOWN
        {
            movementInput.z -= 1;
        }
        if (Input.GetKey(controlKeys[3]))       // RIGHT
        {
            movementInput.x += 1;
        }
    }

    private void alignMovement()
    {
        movementInput = movementInput.x * Vector3.Cross(Vector3.up, transform.forward).normalized + movementInput.z * transform.forward.normalized;
    }

    private void updateMovement()
    {
        if(usingController)
        {
            getJoystickMovement();
        }
        else
        {
            getKeyboardMovement();
        }

        alignMovement();

        rb.velocity += movementInput * accel;         // Additive controls, so it will intentionally feel a little floaty.
        if(rb.velocity.magnitude > maxSpeed)
        {
            rb.velocity = rb.velocity.normalized * maxSpeed;
        }
        // If movementInput is zero, they will slowly drift to a stop with a drag of 1.
    }
    
    private void updateRotation()
    {
        Vector3 newForward = -1 * (cam.transform.position - this.transform.position);
        if(newForward != Vector3.zero)
        {
            this.transform.forward = new Vector3(newForward.x, 0f, newForward.z);
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Prop")
        {
            inventory.Add(other.gameObject.GetComponent<PropController>().propNum);
            Destroy(other.gameObject);
        }
    }
    
    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
        inventory = new List<int>();
    }

    void Update()
    {
        updateMovement();
        updateRotation();
    }
}
