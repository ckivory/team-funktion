using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_shoot : MonoBehaviour
{
    GameObject bullet;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            bullet = GetComponent<scr_inventoryControl>().removeItem();
            if ( bullet!= null)
            {
                bullet.GetComponent<Rigidbody>().AddForce(this.transform.forward*999f);
            }
        }
    }
}
