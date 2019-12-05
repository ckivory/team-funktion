using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PD_MineTrigger : MonoBehaviour
{
    public GameObject mine;
    private PD_MineController mc;

    private void Start()
    {
        mc = mine.GetComponent<PD_MineController>();
    }

    private void OnTriggerStay(Collider other)
    {
        mc.tryExplode();
    }
}
