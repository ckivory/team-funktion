using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Camera cam;
    public GameObject arrow;
    public float maxSpeed;
    public float accel;
    public float aimSpeed;
    public GameObject propPrefab;
    public float shotForce;
    public int controllerNum;
    public bool usingController;
    public List<KeyCode> controlKeys;
    
    
    private Vector3 movementInput;
    private float currentAim;
    private float targetAim;
    private float currentAimSpeed;
    private const float AIM_MAX = 60.0f;
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
        Vector3 newForward = (this.transform.position - cam.transform.position);
        if(newForward != Vector3.zero)
        {
            this.transform.forward = new Vector3(newForward.x, 0f, newForward.z);
        }
    }

    private void updateAim()
    {
        targetAim = Input.GetAxis("J" + controllerNum + "LT") * AIM_MAX;
        Mathf.Clamp(targetAim, 0f, AIM_MAX);
        currentAim = Mathf.SmoothDamp(currentAim, targetAim, ref currentAimSpeed, aimSpeed);
        arrow.transform.localEulerAngles = new Vector3(currentAim, 0f, 0f);
    }

    private void fire()
    {
        if(Input.GetButtonDown("J" + controllerNum + "RB") && inventory.Count > 0)
        {
            inventory.Remove(0);
            GameObject prop = Instantiate(propPrefab, transform.position, Quaternion.identity) as GameObject;
            Rigidbody propRB = prop.GetComponent<Rigidbody>();
            propRB.detectCollisions = false;
            propRB.velocity = new Vector3(this.transform.forward.x, 0f, this.transform.forward.z).normalized;
            propRB.velocity = (Quaternion.Euler(currentAim, 0f, 0f) * propRB.velocity) * shotForce;
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
        currentAim = 0f;
        targetAim = 0f;
    }

    void Update()
    {
        updateMovement();
        updateRotation();
        updateAim();
        fire();
    }
}
