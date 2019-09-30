using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_inventoryControl : MonoBehaviour
{
    public float expandFactor;
    Dictionary<string, int> inventory;
    [HideInInspector]
    public GameObject item;
    GameObject selected;
    public string selectedType;
    int selectedNum = 0;
    List<GameObject> itemList;
    public bool expand;

    // Start is called before the first frame update
    void Start()
    {
        itemList = new List<GameObject>();
        inventory = new Dictionary<string, int>();
        selected = null;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void Expand()
    {
        //just an example to manipulate the orbiting items
        if (itemList.Count > 0 && expand)
        {
            foreach (GameObject obj in itemList)
            {

                obj.GetComponent<scr_orbitControl>().xSpread += expandFactor * Math.Sign(obj.GetComponent<scr_orbitControl>().xSpread);
                obj.GetComponent<scr_orbitControl>().zSpread += expandFactor * Math.Sign(obj.GetComponent<scr_orbitControl>().zSpread); ;
                //obj.GetComponent<scr_orbitControl>().ySpread += expandFactor/2;
                obj.GetComponent<scr_orbitControl>().rotSpeed += expandFactor / 10;
            }
        }
    }
    void UpdateInventory(Collider other)
    {
        item = other.gameObject;    //Get other
        //Update inventory
        if (item.tag == "item" && !item.GetComponent<scr_orbitControl>().isBullet)
        {
            itemList.Add(item);
            item.GetComponent<scr_orbitControl>().DisableCollision();
            string itemType = item.GetComponent<scr_orbitControl>().type;
            if (!inventory.ContainsKey(itemType))
            {
                inventory.Add(itemType, 1);
            }
            else
            {
                inventory[itemType]++;
            }

            //Select the first item got picked up
            if (selected == null)
            {
                selected = itemList[0];
                selectedType = selected.GetComponent<scr_orbitControl>().type;
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        Expand();
        UpdateInventory(other);
    }

    /// <summary>
    /// Call this method to change selected item. Takes string "left" or "right".
    /// </summary>
    /// <param name="direction">left or right</param>
    [HideInInspector]
    public void cycleSelected(string direction)
    {
        if (itemList.Count == 0)
        {
            selectedNum = 0;
            selected = null;
            selectedType = null;
            return;
        }
        else if (inventory.Count < 2)
        {
            return;
        }
        string preType = selectedType;
        if (direction == "right")
        {
            if (selectedNum >= itemList.Count - 1)
            {
                selectedNum = 0;
            }
            else
            {
                selectedNum++;
            }
        }

        if (direction == "left")
        {
            if (selectedNum <= 0)
            {
                selectedNum = itemList.Count - 1;
            }
            else
            {
                selectedNum--;
            }
        }

        if (itemList.Count > 0)
        {
            selected = itemList[selectedNum];
            selectedType = selected.GetComponent<scr_orbitControl>().type;
        }

        if (selectedType == preType)
        {
            cycleSelected(direction);
        }
    }

    /// <summary>
    /// Removes an selected object from inventory. 
    /// </summary>
    [HideInInspector]
    public GameObject removeItem()
    {
        GameObject bullet = selected;
        if (inventory.Count>0 && inventory.ContainsKey(selectedType) && inventory[selectedType] > 0)
        {
            inventory[selectedType]--;
            selected.GetComponent<scr_orbitControl>().captured = false;
            selected.GetComponent<scr_orbitControl>().isBullet = true;

            itemList.Remove(selected);

            if(inventory[selectedType] <= 0)
            {
                inventory.Remove(selectedType);
            }

            selectedNum = 0;
            if (itemList.Count>0)
            {
                selected = itemList[0];
                selectedType = selected.GetComponent<scr_orbitControl>().type;
            }
            else
            {
                selected = null;
                selectedType = null;
            }
            bullet.GetComponent<scr_orbitControl>().EnableCollision();
            return bullet;
        }

        return null;
    }


}
