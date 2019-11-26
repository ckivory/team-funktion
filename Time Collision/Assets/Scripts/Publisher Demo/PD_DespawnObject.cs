using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PD_DespawnObject : MonoBehaviour
{
    public float DespawnTimer;
    private float Timer;

    public GameObject parentSpawner;
    private int propNum;

    // Start is called before the first frame update
    void Start()
    {
        Timer = DespawnTimer;
        propNum = gameObject.GetComponent<ObjectAttributes>().propNum;
    }

    private void OnDestroy()
    {
        if(parentSpawner != null)
        {
            parentSpawner.SendMessage("decrementItem", propNum);
        }
    }

    // Update is called once per frame
    void Update()
    {
        Timer -= Time.deltaTime;
        if (Timer <= 0)
        {
            Destroy(gameObject);
        }
    }
}
