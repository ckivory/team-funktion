using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PD_ExplosionController : MonoBehaviour
{
    public float endSize = 15f;
    private float size;

    public float duration = 0.2f;
    private float timer;

    public float forceRadius = 30f;
    public float explosionForce = 100f;

    [HideInInspector]
    public GameObject SoundManager;
    private PD_SoundManager sm;
    
    void Start()
    {
        SoundManager = GameObject.FindGameObjectWithTag("SoundManager");
        sm = SoundManager.GetComponent<PD_SoundManager>();

        sm.playSound(3);

        size = 0f;
        timer = 0f;

        foreach (Collider col in Physics.OverlapSphere(transform.position, forceRadius))
        {
            Rigidbody rb = col.gameObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                Vector3 difference = col.gameObject.transform.position - transform.position;
                rb.AddForce((explosionForce / Mathf.Max(difference.magnitude, 1f)) * difference.normalized);
            }
        }
    }

    void Update()
    {
        timer += Time.deltaTime;
        size = endSize * (timer / duration);

        gameObject.transform.localScale = new Vector3(size, size, size);

        if(timer >= duration)
        {
            Destroy(gameObject);
        }
    }
}
