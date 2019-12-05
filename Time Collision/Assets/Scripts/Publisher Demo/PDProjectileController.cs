using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PDProjectileController : MonoBehaviour
{
    public GameObject whoFired;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Player") && !other.gameObject.CompareTag("DeathZone") && !other.gameObject.CompareTag("Mine"))
        {
            Destroy(gameObject);
        }
        if (other.gameObject.CompareTag("Mine"))
        {
            other.gameObject.GetComponent<PD_MineController>().tryExplode();
        }
    }
}
