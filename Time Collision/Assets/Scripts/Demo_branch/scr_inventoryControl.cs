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

    private void OnTriggerEnter(Collider other)
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


        item = other.gameObject;    //Get other
        //Update inventory
        if (item.tag == "item")
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

            //Select the first item got picked up
            if (selected == null)
            {
                selected = itemList[0];
                selectedType = selected.GetComponent<scr_orbitControl>().type;
            }
        }



    }

    /// <summary>
    /// Call this method to change selected item. Takes string "left" or "right".
    /// </summary>
    /// <param name="direction">left or right</param>
    [HideInInspector]
    public void cycleSelected(string direction)
    {
        if (inventory.Count < 2)
        {
            return;
        }
        string preType = selectedType;
        if (direction == "right")
        {
            if (selectedNum == itemList.Count - 1)
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
            if (selectedNum == 0)
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


    [HideInInspector]
    public void removeItem()
    {
        if (inventory.ContainsKey(selectedType) && inventory[selectedType] > 0)
        {
            inventory[selectedType]--;
            selected.GetComponent<scr_orbitControl>().captured = false;
            selected.GetComponent<scr_orbitControl>().isBullet = true;

            itemList.Remove(selected);
        }
    }


}
