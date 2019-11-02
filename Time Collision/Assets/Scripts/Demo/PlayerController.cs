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
    public float aimDamping;
    public float shotForce;
    public int controllerNum;
    public bool usingController;

    public List<GameObject> collectedPropPrefabs;
    public List<GameObject> firedPropPrefabs;

    private Vector3 movementInput;
    private float currentAim;
    private float targetAim;
    private float currentAimSpeed;
    private const float AIM_MAX = 60f;
    private List<int> inventory;
    private int selectedProp;
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
        if(Input.GetKey(KeyCode.W))
        {
            movementInput.z += 1;
        }
        if (Input.GetKey(KeyCode.A))
        {
            movementInput.x -= 1;
        }
        if (Input.GetKey(KeyCode.S))
        {
            movementInput.z -= 1;
        }
        if (Input.GetKey(KeyCode.D))
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

    private void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.CompareTag("Collectible"))
        {
            int propNum = col.gameObject.GetComponent<ObjectAttributes>().propNum;
            addToInventory(propNum);
            string invString = string.Join(",", inventory.ToArray());
            Debug.Log(invString);
            Destroy(col.gameObject);
        }
    }

    private void addToInventory(int propNum)
    {
        inventory[propNum]++;
    }

    private void updateAim()
    {
        if(usingController)
        {
            // TODO: Change to X and Y
            targetAim -= Input.GetAxis("J" + controllerNum + "LT") * Time.deltaTime * (1 / aimSpeed);
            targetAim += Input.GetAxis("J" + controllerNum + "RT") * Time.deltaTime * (1 / aimSpeed);
        }
        else
        {
            // TODO: Implement scroll-based pitch
        }

        targetAim = Mathf.Clamp(targetAim, 0f, AIM_MAX);
        currentAim = Mathf.SmoothDamp(currentAim, targetAim, ref currentAimSpeed, aimSpeed * aimDamping);

        arrow.transform.forward = this.transform.forward;
        arrow.transform.rotation *= Quaternion.Euler(-1 * currentAim, 0f, 0f);
    }

    private void initializeProjectile()
    {
        GameObject shot = Instantiate(firedPropPrefabs[selectedProp], rb.position, arrow.transform.rotation);
        shot.GetComponent<Rigidbody>().velocity = arrow.transform.forward * shotForce;
    }

    private bool triggerPulled()
    {
        if(usingController)
        {
            if (Input.GetButtonDown("J" + controllerNum + "RB") || Input.GetKeyDown(KeyCode.F))//&& inventory.Count > 0)
            {
                return true;
            }
        }
        else
        {
            if(Input.GetMouseButtonDown(0))
            {
                return true;
            }
        }
        return false;
    }

    private void fire()
    {
        if(inventory[selectedProp]> 0)
        {
            Debug.Log("Firing " + selectedProp + ". " + (inventory[selectedProp] - 1) + " items left.");
            initializeProjectile();
            inventory[selectedProp]--;
        }
        else
        {
            Debug.Log("Click");
        }
    }

    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
        inventory = new List<int>();
        for (int i = 0; i < collectedPropPrefabs.Count; i++)
        {
            inventory.Add(0);
        }
        currentAim = 0f;
        targetAim = 0f;
        selectedProp = 1;
    }

    void Update()
    {
        updateMovement();
        updateRotation();
        updateAim();
        if(triggerPulled())
        {
            fire();
        }
    }
}
