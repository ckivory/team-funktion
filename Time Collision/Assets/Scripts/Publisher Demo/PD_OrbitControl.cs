using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PD_OrbitControl : MonoBehaviour
{
    float ySpread = 0;
    [HideInInspector]
    public float rotR;
    [HideInInspector]
    public float rotSpeed = 0;
    Transform centerPoint;
    [HideInInspector]
    public float timer = 0;
    [HideInInspector]
    public GameObject playerToFollow;
    [HideInInspector]
    public float initalLocation;
    [HideInInspector]
    public int type;



    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Rotate();
    }

    void Rotate()
    {
        float currentRadius = (rotR * playerToFollow.GetComponent<PDPlayerController>().scale);
        timer += Time.deltaTime * rotSpeed / currentRadius;
        if(playerToFollow != null)
        {
            centerPoint = playerToFollow.transform;
            float x = Mathf.Cos(timer) * currentRadius;
            float z = Mathf.Sin(timer) * currentRadius;
            float y = Mathf.Sin(timer) * ySpread * playerToFollow.GetComponent<PDPlayerController>().scale;
            Vector3 pos = new Vector3(x, y, z);
            this.transform.LookAt(centerPoint);
            transform.position = pos + centerPoint.position;
        }
    }
}
