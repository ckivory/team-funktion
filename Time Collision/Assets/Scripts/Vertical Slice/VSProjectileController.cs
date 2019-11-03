using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VSProjectileController : MonoBehaviour
{
    public GameObject whoFired;

    private void OnTriggerEnter(Collider other)
    {
        if(!other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Destroyed by: " + other.gameObject.tag);
            Destroy(gameObject);
        }
    }
}
