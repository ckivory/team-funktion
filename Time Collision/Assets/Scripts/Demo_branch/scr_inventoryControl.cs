using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_inventoryControl : MonoBehaviour
{

    public GameObject cube;
    public GameObject oCube;
    Dictionary<string, int> inventory;
    GameObject item;
    List<GameObject> itemList;  //created for future convenience

    // Start is called before the first frame update
    void Start()
    {
        inventory = new Dictionary<string, int>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        item = collision.gameObject;
        if (item.GetComponent<scr_orbitControl>().type == "cube")
        {
            inventory.Add("cube", 1);
        }
    }
}
