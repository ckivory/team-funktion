using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PDProjectileController : MonoBehaviour
{
    public GameObject whoFired;

    private void OnTriggerEnter(Collider other)
    {
        if(!other.gameObject.CompareTag("Player") && !other.gameObject.CompareTag("DeathZone"))
        {
            Debug.Log("Destroyed by: " + other.gameObject.tag);
            Destroy(gameObject);
        }
    }
}
