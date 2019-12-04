using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PDPlayerController : MonoBehaviour
{
    public Camera cam;
    public GameObject arrow;
    public GameObject target;
    public GameObject vfx;

    private float damageTimer;
    private bool insideZone;

    public List<float> levelMasses;
    [HideInInspector]
    public int level;

    public List<float> accelerations;
    public float topSpeed;

    public float aimSpeed = 0.01f;
    public float aimDamping = 5f;
    public float aimMin = 10f;
    public float aimMax = 60f;

    public float scrollSensitivity = 10f;
    public float shotForce = 60f;               // How hard should objects be thrown?
    public float forceIncreaseFactor = 0.6f;    // How much of the player's growth should apply to how hard they fire objects?
    public float maxSpread = 0.2f;              // What is the maximum amount of spread a shot should have?
    public int spreadNum = 10;                  // How many items should it take to reach this max spread?
    public int controllerNum = 0;
    public bool usingController = true;

    public GameObject core;
    public GameObject disk;
    public float coreSpeed = 40f;
    public float diskSpeed = 30f;

    float lastRot;

    [HideInInspector]
    public float playerMass = 0f;

    private int shield;
    private float shieldRemaining;

    public float massScale = 10f;
    public float scaleLerp = 1f;
    [HideInInspector]
    public float scale;

    private float baseHeight;

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

    public bool alive; //made public by Lin

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
        float angleOffset = lastRot - transform.localRotation.eulerAngles.y;
        core.transform.Rotate(coreSpeed * Time.deltaTime, 0f, 0f);
        disk.transform.Rotate(0f, diskSpeed * Time.deltaTime + angleOffset, 0f);
        lastRot = transform.localRotation.eulerAngles.y;
    }

    private void playerDeath()
    {
        alive = false;
        // Code for changing the player's avatar to represent the fact that they are dead.
        arrow.SetActive(false);     // A suggestion
        vfx.SetActive(false);
    }

    private void takeDamage(float damageToDeal)
    {
        // Play particle effect for taking damage
        vfx.transform.GetChild(1).GetComponent<ParticleSystem>().Play();
        cam.SendMessage("beginRumble");

        // Debug.Log("Taking " + damageToDeal + " damage");
        if (shield == -1)
        {
            playerDeath();
            updateMass();
            return;
        }

        while (damageToDeal > 0)
        {
            if (shieldRemaining > damageToDeal)
            {
                shieldRemaining -= damageToDeal;
                updateMass();
                //damageToDeal = 0;
                return;
            }
            else
            {
                damageToDeal -= shieldRemaining;
                inventory[shield]--;
                GetComponent<PD_DiskController>().RemoveFromDisk(shield);  //added by Lin
                newShield();
                updateMass();
                
                if (shield == -1)
                {
                    if(damageToDeal > 0)
                    {
                        playerDeath();
                        return;
                    }
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
                col.gameObject.SendMessage("ReturnToPool");
            }
            if (col.gameObject.CompareTag("Fired"))
            {
                if (!(gameObject.GetInstanceID() == col.GetComponent<ObjectAttributes>().whoFired.GetInstanceID()))
                {
                    StartCoroutine(gameObject.GetComponent<FlashOnHit>().FlashObject()); //trigger flash on hit
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
                // Debug.Log("Player " + controllerNum + " entering explosion!");
                takeDamage(ObjectAttributes.getDamage(MINE_PROPNUM) / 2);           // Duct tape solution to keep damage from being disproportional.
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
            // Debug.Log("Leaving Zone");
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

    private void updateTarget()
    {
        target.transform.localPosition = new Vector3(0f, target.transform.localPosition.y, 0f);
        target.transform.position = new Vector3(target.transform.position.x, 0f, target.transform.position.z);

        Vector3 initVel = (arrow.transform.forward * shotForce * Mathf.Max(scale * forceIncreaseFactor, 1f));
        float a = Physics.gravity.y;
        float b = initVel.y;
        float c = rb.position.y;

        float t = (-b - Mathf.Sqrt(b*b - (4*a*c)))/(2*a);
        initVel.y = 0f;
        float targetDistance = initVel.magnitude * t;

        target.transform.position += targetDistance * transform.forward.normalized;
    }

    private void updateAim()
    {
        if (usingController)
        {
            
            targetAim -= Input.GetAxis("J" + controllerNum + "LT") * Time.deltaTime * (1 / aimSpeed);
            targetAim += Input.GetAxis("J" + controllerNum + "RT") * Time.deltaTime * (1 / aimSpeed);
            
        }
        else
        {
            targetAim -= Input.GetAxis("Mouse ScrollWheel") * scrollSensitivity * Time.deltaTime * (1 / aimSpeed);
        }

        targetAim = Mathf.Clamp(targetAim, aimMin, aimMax);
        currentAim = Mathf.SmoothDamp(currentAim, targetAim, ref currentAimSpeed, aimSpeed * aimDamping);

        arrow.transform.forward = this.transform.forward;
        arrow.transform.rotation *= Quaternion.Euler(-1 * currentAim, 0f, 0f);

        updateTarget();
    }

    private void instantiateProj(Vector3 spreadAmount)
    {
        GameObject shot = Instantiate(firedPropPrefabs[selectedProp], rb.position, arrow.transform.rotation);
        GetComponent<PD_DiskController>().RemoveFromDisk(selectedProp);    //Added by Lin
        shot.GetComponent<Rigidbody>().velocity = rb.velocity + (arrow.transform.forward + spreadAmount) * shotForce * Mathf.Max(scale * forceIncreaseFactor, 1f);
        shot.GetComponent<ObjectAttributes>().whoFired = gameObject;
    }

    private void initializeProjectiles(int shotCount)
    {
        instantiateProj(Vector3.zero);

        for (int i = 1; i < shotCount; i++)
        {
            instantiateProj(spread(shotCount));
        }
    }

    private Vector3 spread(int shotCount)
    {
        float spread;
        if (shotCount < spreadNum)
        {
            spread = maxSpread * ((float)(shotCount) / (float)spreadNum);
            // Debug.Log("Items: " + shotCount + " Interpolated spread: " + spread);
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
            // Debug.Log("Firing " + selectedProp + "s. ");            
            initializeProjectiles(numProjectiles());
            inventory[selectedProp] -= numProjectiles();
            updateMass();
            coolDown = coolDownDuration;
        }
        else
        {
            // Debug.Log("Nothing to fire!");
        }
        if(inventory[selectedProp] < 1)
        {
            selectedProp = findNonEmpty();
        }
    }

    private void plantMine()
    {
        if (inventory[MINE_PROPNUM] < 1)
        {
            // Debug.Log("No mines to plant.");
        }
        else
        {
            // Debug.Log("Planting mine.");
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
    
    private bool isInventoryEmpty()
    {
        for(int i = 0; i < inventory.Count; i++)
        {
            if(inventory[i] > 0 && i != MINE_PROPNUM)
            {
                return false;
            }
        }
        return true;
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
            if (Input.GetButtonDown("J" + controllerNum + "Y"))
            {
                if (!isInventoryEmpty())
                {
                    // Duct-tape solution to keep player from shooting mines normally.
                    do
                    {
                        selectedProp = (selectedProp + 1) % inventory.Count;
                    } while (selectedProp == MINE_PROPNUM || (inventory[selectedProp] < 1 && !isInventoryEmpty()));

                    // Debug.Log(selectedProp);
                }
            }
            if (Input.GetButtonDown("J" + controllerNum + "X"))
            {
                if (!isInventoryEmpty())
                {
                    // Duct-tape solution to keep player from shooting mines normally.
                    do
                    {
                        selectedProp--;
                        if (selectedProp < 0)
                        {
                            selectedProp += inventory.Count;
                        }
                    } while (selectedProp == MINE_PROPNUM || (inventory[selectedProp] < 1 && !isInventoryEmpty()));

                    // Debug.Log(selectedProp);
                }
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (!isInventoryEmpty())
                {
                    // Duct-tape solution to keep player from shooting mines normally.
                    do
                    {
                        selectedProp = (selectedProp + 1) % inventory.Count;
                    } while (selectedProp == MINE_PROPNUM || (inventory[selectedProp] < 1 && !isInventoryEmpty()));
                    Debug.Log(selectedProp);
                }
            }
            if (Input.GetKeyDown(KeyCode.Q))
            {
                if (!isInventoryEmpty())
                {
                    // Duct-tape solution to keep player from shooting mines normally.
                    do
                    {
                        selectedProp--;
                        if (selectedProp < 0)
                        {
                            selectedProp += inventory.Count;
                        }
                    } while (selectedProp == MINE_PROPNUM || (inventory[selectedProp] < 1 && !isInventoryEmpty()));
                    Debug.Log(selectedProp);
                }
            }
        }
    }

    private void updateSelectedCount()
    {
        if(usingController)
        {
            if(Input.GetButtonDown("J" + controllerNum + "B"))
            {
                selectedCount *= 2;
                if (selectedCount > 16)
                {
                    selectedCount = 16;
                }
            }
            else if(Input.GetButtonDown("J" + controllerNum + "A"))
            {
                selectedCount /= 2;
                if(selectedCount < 1)
                {
                    selectedCount = 1;
                }
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                selectedCount *= 2;
                if (selectedCount > 16)
                {
                    selectedCount = 16;
                }
            }
            else if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                selectedCount /= 2;
                if (selectedCount < 1)
                {
                    selectedCount = 1;
                }
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
        if(shield != -1)
        {
            playerMass -= (ObjectAttributes.getMass(shield) - shieldRemaining);
        }
        updateLevel();
        //Debug.Log("Player mass is: " + playerMass);
    }

    private void updateScale()
    {
        float newScale = (1 + (level / massScale));
        float timedScaleLerp = Mathf.Clamp(scaleLerp * Time.deltaTime, 0f, 1f);
        scale = (scale * (1 - timedScaleLerp)) + (newScale * timedScaleLerp);
        transform.localScale = new Vector3(scale * 0.5f, scale * 0.5f, scale * 0.5f);
        transform.position = new Vector3(transform.position.x, baseHeight + scale - 1, transform.position.z);
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
        // Debug.Log("New shield: " + shield);
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
                    // Debug.Log("You shouldn't be able to get here. We can't fire mines normally.");
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
        scale = 1f;
        baseHeight = transform.position.y;
        lastRot = transform.localRotation.eulerAngles.y;

        if (!usingController)
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
            updateAim();
            handleFiring();

            if(!insideZone)
            {
                //Debug.Log("Outside Zone!");
                damageTimer += Time.deltaTime;
                if (damageTimer >= 1)
                {
                    takeDamage(deathZone.currentDamage);
                    damageTimer = 0f;
                }
            }
        }
        updateRotation();
        updateSpin();
        updateSelected();
        updateSelectedCount();
        updateScale();
    }
}
