using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PD_MineController : MonoBehaviour
{
    public bool fired;

    public float armTime = 2f;
    private float timer;

    private bool armed;
    
    public GameObject explosionPrefab;

    private void Start()
    {
        armed = false;
        timer = armTime;
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
        if (armed && fired)
        {
            explode();
        }
    }

    public void forceExplode()
    {
        explode();
    }

    private void explode()
    {
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    private void Update()
    {
        if(!armed && fired)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                armed = true;
            }
        }
    }
}
