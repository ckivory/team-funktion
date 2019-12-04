using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DespawnCollide : MonoBehaviour
{
    public GameObject particle;
    void OnDestroy()
    {
        Instantiate(particle, transform.position, transform.rotation);
    }
}
