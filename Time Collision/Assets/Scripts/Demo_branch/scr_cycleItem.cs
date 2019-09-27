using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_cycleItem : MonoBehaviour
{
    public List<KeyCode> Keys;
    GameObject bullet;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Q))
        {
            GetComponent<scr_inventoryControl>().cycleSelected("left");
        }
        if (Input.GetKeyUp(KeyCode.E))
        {
            GetComponent<scr_inventoryControl>().cycleSelected("right");
        }
    }
}
