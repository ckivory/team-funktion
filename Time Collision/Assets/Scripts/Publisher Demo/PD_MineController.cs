using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PD_MineController : MonoBehaviour
{
    public float armTime = 2f;
    private float timer;

    private bool armed;

    public GameObject targetObj;
    private Collider target;

    public GameObject explosionPrefab;

    private void Start()
    {
        armed = false;
        timer = armTime;
        target = targetObj.GetComponent<Collider>();
    }

    private void OnTriggerStay(Collider col)
    {
        if (col.CompareTag("Player") || col.CompareTag("Fired"))
        {
            tryExplode();
        }
    }

    public void tryExplode()
    {
        if (armed)
        {
            explode();
        }
    }

    private void explode()
    {
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    private void Update()
    {
        if(!armed)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                armed = true;
            }
        }
    }
}
