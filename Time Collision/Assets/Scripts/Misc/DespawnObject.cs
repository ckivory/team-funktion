using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DespawnObject : MonoBehaviour
{
    public float DespawnTimer;
    private float Timer;
    // Start is called before the first frame update
    void Start()
    {
        Timer = DespawnTimer;
    }

    // Update is called once per frame
    void Update()
    {
        Timer -= Time.deltaTime;
        if (Timer<=0)
        {
            Destroy(gameObject);
        }
    }
}
