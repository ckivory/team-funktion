using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PDPlayerController : MonoBehaviour
{
    public Camera cam;
    public GameObject arrow;
    
    private float damageTimer;
    private bool insideZone;

    public List<float> levelMasses;
    private int level;

    public List<float> accelerations;
    public float topSpeed;

    public float aimSpeed = 0.01f;
    public float aimDamping = 5f;
    public float aimMin = 10f;
    public float aimMax = 60f;

    public float scrollSensitivity = 10f;
    public float shotForce = 60f;           // How hard should objects be thrown?
    public float maxSpread = 0.2f;          // What is the maximum amount of spread a shot should have?
    public int spreadNum = 10;       // How many items should it take to reach this max spread?
    public int controllerNum = 0;
    public bool usingController = true;

    public GameObject core;
    public GameObject disk;
    public float coreSpeed = 40f;
    public float diskSpeed = 30f;

    public float playerMass = 0f;
    private int shield;
    private float shieldRemaining;

    public float coolDownDuration = 0.1f;
    private float coolDown;

    public List<GameObject> collectedPropPrefabs;
    public List<GameObject> firedPropPrefabs;

    public PD_DeathZoneController deathZone;

    private Vector3 movementInput;
    private float currentAim;
    private float targetAim;
    private float currentAimSpeed;

    [HideInInspector]
    public int MINE_PROPNUM = 6;    // This number is how we differentiate mines from other props. Not the cleanest solution, but it doesn't require us to overhaul the entire inventory system.

    private bool RTpressed;
    private bool LTpressed;
    public List<int> inventory;
    [HideInInspector]
    public int selectedProp;    //made public by Lin
    [HideInInspector]
    public int selectedCount;   //made public by Lin
    private Rigidbody rb;

    private bool alive;

    private void getJoystickMovement()
    {
        movementInput = Vector3.zero;
        if (controllerNum > 0)
        {
            movementInput.x = Input.GetAxis("J" + controllerNum + "Horizontal");
            movementInput.z = Input.GetAxis("J" + controllerNum + "Vertical");
        }
        else
        {
            throw new System.Exception("Make sure to set the controller number in the Unity editor");
        }

        if (movementInput.magnitude > 1)
        {
            movementInput = movementInput.normalized;
        }
    }

    private void getKeyboardMovement()
    {
        movementInput = Vector3.zero;
        if (Input.GetKey(KeyCode.W))
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
        if (usingController)
        {
            getJoystickMovement();
        }
        else
        {
            getKeyboardMovement();
        }

        alignMovement();

        rb.velocity += movementInput * accelerations[level] * Time.deltaTime;         // Additive controls, so it will intentionally feel a little floaty.
        if (rb.velocity.magnitude > topSpeed)
        {
            rb.velocity = rb.velocity.normalized * topSpeed;
        }
        // If movementInput is zero, they will slowly drift to a stop with a drag of 1.
    }

    private void updateRotation()
    {
        Vector3 newForward = (this.transform.position - cam.transform.position);
        if (newForward != Vector3.zero)
        {
            this.transform.forward = new Vector3(newForward.x, 0f, newForward.z);
        }
    }

    private void updateSpin()
    {
        core.transform.Rotate(0f, coreSpeed * Time.deltaTime, 0f);
        disk.transform.Rotate(0f, diskSpeed * Time.deltaTime, 0f);
    }

    private void playerDeath()
    {
        alive = false;
        // Code for changing the player's avatar to represent the fact that they are dead.
        arrow.SetActive(false);     // A suggestion
    }

    private void takeDamage(float damageToDeal)
    {
        Debug.Log("Taking " + damageToDeal + " damage");
        if (shield == -1)
        {
            playerDeath();
            return;
        }

        while (damageToDeal > 0)
        {
            if (shieldRemaining > damageToDeal)
            {
                shieldRemaining -= damageToDeal;
                //damageToDeal = 0;
                return;
            }
            else
            {
                damageToDeal -= shieldRemaining;
                inventory[shield]--;
                GetComponent<PD_DiskController>().RemoveFromDisk(shield);  //added by Lin
                newShield();

                // This gives the player one last chance to get new cover when their last shield breaks. Large objects can break the rest of your shield, but they can't kill you in one shot unless your inventory is empty.
                if (shield == -1)
                {
                    return;
                }
            }
        }
    }

    private void OnTriggerEnter(Collider col)
    {
        if(alive)
        {
            if (col.gameObject.CompareTag("Collectible"))
            {
                int propNum = col.gameObject.GetComponent<ObjectAttributes>().propNum;
                addToInventory(propNum);
                string invString = string.Join(",", inventory.ToArray());
                Debug.Log(invString);
                Destroy(col.gameObject);
            }
            if (col.gameObject.CompareTag("Fired"))
            {
                if (!(gameObject.GetInstanceID() == col.GetComponent<ObjectAttributes>().whoFired.GetInstanceID()))
                {
                    if (shield == -1)
                    {
                        playerDeath();
                    }
                    else
                    {
                        float damageToTake = ObjectAttributes.getDamage(col.gameObject.GetComponent<ObjectAttributes>().propNum);
                        takeDamage(damageToTake);
                    }
                    Destroy(col.gameObject);
                }
            }
            if(col.gameObject.CompareTag("Explosion"))          // For some reason this gets activated twice for each explosion?
            {
                Debug.Log("Player " + controllerNum + " entering explosion!");
                takeDamage(ObjectAttributes.damageList[MINE_PROPNUM]);
            }
        }
    }

    private void OnTriggerStay(Collider col)
    {
        if(col.CompareTag("DeathZone"))
        {
            //Debug.Log("Inside Zone");
            insideZone = true;
        }
        
    }

    private void OnTriggerExit(Collider col)
    {
        if(col.CompareTag("DeathZone"))
        {
            Debug.Log("Leaving Zone");
            insideZone = false;
        }
    }

    // For deathzone to access when it disappears
    public void LeaveZone()
    {
        insideZone = false;
    }

    private void addToInventory(int propNum)
    {
        inventory[propNum]++;
        if (inventory[selectedProp] == 0)
        {
            selectedProp = findNonEmpty();
        }
        updateMass();
        GetComponent<PD_DiskController>().AddToDisk(propNum);  //added by Lin
        if (inventory[selectedProp] == 0)
        {
            selectedProp = findNonEmpty();
        }
        if (shield == -1)
        {
            newShield();
        }
    }

    private void updateAim()
    {
        if (usingController)
        {
            
            targetAim -= Input.GetAxis("J" + controllerNum + "X") * Time.deltaTime * (1 / aimSpeed);
            targetAim += Input.GetAxis("J" + controllerNum + "Y") * Time.deltaTime * (1 / aimSpeed);
            
        }
        else
        {
            targetAim -= Input.GetAxis("Mouse ScrollWheel") * scrollSensitivity * Time.deltaTime * (1 / aimSpeed);
        }

        targetAim = Mathf.Clamp(targetAim, aimMin, aimMax);
        currentAim = Mathf.SmoothDamp(currentAim, targetAim, ref currentAimSpeed, aimSpeed * aimDamping);

        arrow.transform.forward = this.transform.forward;
        arrow.transform.rotation *= Quaternion.Euler(-1 * currentAim, 0f, 0f);
    }

    private void initializeProjectiles(int shotCount)
    {
        for (int i = 0; i < shotCount; i++)
        {
            GameObject shot = Instantiate(firedPropPrefabs[selectedProp], rb.position, arrow.transform.rotation);
            GetComponent<PD_DiskController>().RemoveFromDisk(selectedProp);    //Added by Lin
            shot.GetComponent<Rigidbody>().velocity = rb.velocity + (arrow.transform.forward + spread(shotCount)) * shotForce;
            shot.GetComponent<ObjectAttributes>().whoFired = gameObject;
        }
    }

    private Vector3 spread(int shotCount)
    {
        float spread;
        if (shotCount < spreadNum)
        {
            spread = maxSpread * ((float)(shotCount-1) / (float)spreadNum);
            Debug.Log("Items: " + shotCount + " Interpolated spread: " + spread);
        }
        else
        {
            spread = maxSpread;
        }

        return Random.insideUnitSphere * spread;
    }

    private bool triggerPulled()
    {
        if (usingController)
        {
            if (Input.GetButton("J" + controllerNum + "RB"))
            {
                return true;
            }
        }
        else
        {
            if (Input.GetMouseButton(0))
            {
                return true;
            }
        }
        return false;
    }

    private bool altTriggerPulled()
    {
        if(usingController)
        {
            if(Input.GetButton("J" + controllerNum + "LB"))
            {
                return true;
            }
        }
        else
        {
            if(Input.GetMouseButton(1))
            {
                return true;
            }
        }
        return false;
    }

    private int numProjectiles()
    {
        if (selectedCount > inventory[selectedProp])
        {
            return inventory[selectedProp];
        }
        else
        {
            return selectedCount;
        }
    }

    private void fire()
    {
        if(inventory[selectedProp] < 1)
        {
            selectedProp = findNonEmpty();
        }
        if(inventory[selectedProp] > 0)
        {
            Debug.Log("Firing " + selectedProp + "s. ");            
            initializeProjectiles(numProjectiles());
            for (int i = 0; i < numProjectiles(); i++)
            {
                GetComponent<PD_DiskController>().RemoveFromDisk(selectedProp);
            }
            inventory[selectedProp] -= numProjectiles();
            updateMass();
            coolDown = coolDownDuration;
        }
        else
        {
            Debug.Log("Nothing to fire!");
        }
    }

    private void plantMine()
    {
        if (inventory[MINE_PROPNUM] < 1)
        {
            Debug.Log("No mines to plant.");
        }
        else
        {
            Debug.Log("Planting mine.");
            GameObject newMine = Instantiate(firedPropPrefabs[MINE_PROPNUM], transform.position, Quaternion.identity);
            GetComponent<PD_DiskController>().RemoveFromDisk(MINE_PROPNUM);
            inventory[MINE_PROPNUM]--;
            updateMass();
            coolDown = coolDownDuration;
        }
    }

    private int findNonEmpty()
    {
        for(int i = 0; i < inventory.Count; i++)
        {
            int index = (selectedProp + i) % inventory.Count;
            if (inventory[index] > 0 && index != MINE_PROPNUM)
            {
                return index;
            }
        }
        return selectedProp; 
    }
    
    private bool onTriggerDown(bool rightTrigger)
    {
        if (rightTrigger)
        {
            if (Input.GetAxis("J" + controllerNum + "RT") > 0)
            {
                if (!RTpressed)
                {
                    RTpressed = true;
                    return true;
                }
                return false;
            }
            else
            {
                RTpressed = false;
                return false;
            }
        }
        else
        {
            if (Input.GetAxis("J" + controllerNum + "LT") > 0)
            {
                if (!LTpressed)
                {
                    LTpressed = true;
                    return true;
                }
                return false;
            }
            else
            {
                LTpressed = false;
                return false;
            }
        }
    }

    private void updateSelected()
    {
        if (usingController)
        {
            if (onTriggerDown(true))
            {
                // Duct-tape solution to keep player from shooting mines normally.
                do
                {
                    selectedProp = (selectedProp + 1) % inventory.Count;
                } while (selectedProp == MINE_PROPNUM);
                Debug.Log(selectedProp);
            }
            if (onTriggerDown(false))
            {
                // Duct-tape solution to keep player from shooting mines normally.
                do
                {
                    selectedProp--;
                    if (selectedProp < 0)
                    {
                        selectedProp += inventory.Count;
                    }
                } while (selectedProp == MINE_PROPNUM);
                
                Debug.Log(selectedProp);
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                // Duct-tape solution to keep player from shooting mines normally.
                do
                {
                    selectedProp = (selectedProp + 1) % inventory.Count;
                } while (selectedProp == MINE_PROPNUM);
                Debug.Log(selectedProp);
            }
            if (Input.GetKeyDown(KeyCode.Q))
            {
                // Duct-tape solution to keep player from shooting mines normally.
                do
                {
                    selectedProp--;
                    if (selectedProp < 0)
                    {
                        selectedProp += inventory.Count;
                    }
                } while (selectedProp == MINE_PROPNUM);
                Debug.Log(selectedProp);
            }
        }
    }

    private void updateSelectedCount()
    {
        if(usingController)
        {
            if(Input.GetButtonDown("J" + controllerNum + "A"))
            {
                selectedCount++;
                Debug.Log("Count: " + selectedCount);
            }
            else if(Input.GetButtonDown("J" + controllerNum + "B"))
            {
                selectedCount--;
                if(selectedCount < 1)
                {
                    selectedCount = 1;
                }
                Debug.Log("Count: " + selectedCount);
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                selectedCount++;
                Debug.Log("Count: " + selectedCount);
            }
            else if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                selectedCount--;
                if (selectedCount < 1)
                {
                    selectedCount = 1;
                }
                Debug.Log("Count: " + selectedCount);
            }
        }
    }

    private void updateLevel()
    {
        level = 0;
        
        for(int i = 0; i < levelMasses.Count; i++)
        {
            if(levelMasses[i] <= playerMass)
            {
                level = i;
            }
        }

        //Debug.Log("Level: " + level);
    }

    private void updateMass()
    {
        playerMass = 0;
        for(int i = 0; i < inventory.Count; i++)
        {
            playerMass += ObjectAttributes.getMass(i) * inventory[i];
        }
        updateLevel();
        //Debug.Log("Player mass is: " + playerMass);
    }

    private void newShield()
    {
        shield = -1;
        float bestShieldSoFar = 0f;
        for(int i = 0; i < inventory.Count; i++)
        {
            if(ObjectAttributes.getMass(i) > bestShieldSoFar && inventory[i] > 0)
            {
                bestShieldSoFar = ObjectAttributes.getMass(i);
                shield = i;
            }
        }
        if(shield > -1)
        {
            shieldRemaining = ObjectAttributes.getMass(shield);
        }
        else
        {
            shieldRemaining = 0f;
        }
        Debug.Log("New shield: " + shield);
    }

    private void handleFiring()
    {
        if (triggerPulled())
        {
            //Debug.Log("Cooldown: " + coolDown);
            if (coolDown <= 0)
            {
                if(selectedProp != MINE_PROPNUM)
                {
                    fire();
                }
                else
                {
                    Debug.Log("You shouldn't be able to get here. We can't fire mines normally.");
                }
            }
        }
        else if(altTriggerPulled())
        {
            //Debug.Log("Cooldown: " + coolDown);
            if (coolDown <= 0)
            {
                plantMine();
            }
        }

        if (coolDown > 0)
        {
            coolDown -= Time.deltaTime;
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

        level = 0;
        currentAim = 0f;
        targetAim = 0f;
        RTpressed = false;
        LTpressed = false;
        selectedProp = 0;
        selectedCount = 1;
        shield = -1;
        if(!usingController)
        {
            Cursor.visible = false;
        }
        deathZone = GameObject.FindGameObjectWithTag("DeathZone").GetComponent<PD_DeathZoneController>();
        damageTimer = 0f;
        coolDown = 0f;
        insideZone = true;
        alive = true;
    }

    void Update()
    {
        if(alive)
        {
            updateMovement();
        }
        updateRotation();
        updateSpin();
        if(alive)
        {
            updateAim();
        }
        updateSelected();
        updateSelectedCount();

        if(alive)
        {
            handleFiring();
        }
        
        
        
        if (alive && !insideZone)
        {
            //Debug.Log("Outside Zone!");
            damageTimer += Time.deltaTime;
            if(damageTimer >= 1)
            {
                takeDamage(deathZone.currentDamage);
                damageTimer = 0f;
            }
        }
    }
}
