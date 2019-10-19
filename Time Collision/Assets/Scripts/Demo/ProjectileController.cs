using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    public GameObject whoFired;

    private void OnCollisionEnter(Collision collision)
    {
        if (GetComponent<scr_orbitControl>().isBullet)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject != whoFired && GetComponent<scr_orbitControl>().isBullet)
        {
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }
}
