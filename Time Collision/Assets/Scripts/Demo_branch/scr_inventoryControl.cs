using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_inventoryControl : MonoBehaviour
{
    [HideInInspector]
    public float itemDynamicVar;
    Dictionary<string, int> inventory;
    GameObject item;
    List<GameObject> itemList;

    // Start is called before the first frame update
    void Start()
    {
        itemList = new List<GameObject>();
        inventory = new Dictionary<string, int>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {

        //just an example to manipulate the orbiting items
        //if (itemList.Count > 0)
        //{
        //    foreach (GameObject obj in itemList)
        //    {
        //        obj.GetComponent<scr_orbitControl>().xSpread += itemDynamicVar;
        //        obj.GetComponent<scr_orbitControl>().zSpread += itemDynamicVar;
        //        obj.GetComponent<scr_orbitControl>().ySpread += itemDynamicVar;
        //    }
        //}


        item = other.gameObject;
        if(item.tag == "item")
        {
            itemList.Add(item);
            string itemType = item.GetComponent<scr_orbitControl>().type;
            if (!inventory.ContainsKey(itemType))
            {
                inventory.Add(itemType, 1);
            }
            else
            {
                inventory[itemType]++;
            }
        }

    }
}
