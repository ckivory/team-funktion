using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropController : MonoBehaviour
{
    public int propNum;

    private void OnTriggerEnter(Collider other)
    {
        other.SendMessage("addToInventory", propNum);
        Destroy(gameObject);
    }
}
