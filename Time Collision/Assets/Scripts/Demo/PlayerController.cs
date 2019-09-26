using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public int controllerNum;
    public bool usingController;

    public List<KeyCode> controlKeys;
    private Vector3 inputVector;
    private List<int> inventory;

    private Rigidbody rb;

    private void getControllerInput()
    {
        inputVector = Vector3.zero;
        if(controllerNum > 0)
        {
            inputVector.x = Input.GetAxis("J" + controllerNum + "Horizontal");
            inputVector.z = Input.GetAxis("J" + controllerNum + "Vertical");
        }
        else
        {
            throw new System.Exception("Make sure to set the controller number in the Unity editor");
        }

        print("Input magnitude: " + inputVector.magnitude);
        if(inputVector.magnitude > 1)
        {
            inputVector = inputVector.normalized;
        }
    }

    private void getKeyboardInput()
    {
        inputVector = Vector3.zero;
        if(Input.GetKey(controlKeys[0]))       // UP
        {
            inputVector.z += 1;
        }
        if (Input.GetKey(controlKeys[1]))       // LEFT
        {
            inputVector.x -= 1;
        }
        if (Input.GetKey(controlKeys[2]))       // DOWN
        {
            inputVector.z -= 1;
        }
        if (Input.GetKey(controlKeys[3]))       // RIGHT
        {
            inputVector.x += 1;
        }
    }

    private void updateMovement()
    {
        if(usingController)
        {
            getControllerInput();
        }
        else
        {
            getKeyboardInput();
        }

        rb.velocity += inputVector;         // Additive controls, so it will intentionally feel a little floaty.
        if(rb.velocity.magnitude > speed)
        {
            rb.velocity = rb.velocity.normalized * speed;
        }
        // If inputVector is zero, they will slowly drift to a stop with a drag of 1.
    }

    private void updateRotation()
    {
        this.transform.LookAt(new Vector3(0f, this.transform.position.y, 0f));
        // TODO: Implement twin-stick controls
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
