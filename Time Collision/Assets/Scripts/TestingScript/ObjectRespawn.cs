using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectRespawn : MonoBehaviour
{
    public GameObject ObjectToRespawn;
    // these are four variable for determine the range of offset when new respawn is created
    public float MinOffsetX;
    public float MaxOffsetX;
    public float MinOffsetY;
    public float MaxOffsetY;
    // these are variables that determine the respawn timer based on the min and max value
    public float MinTimer;
    public float MaxTimer;
    private float CurrentTimer;

    private float NewTimer()
    {
        return Random.Range(MinTimer, MaxTimer);
    }
    void OnEnable()
    {
        // getting the respawn timer for the first time
        CurrentTimer = NewTimer();
    }
}
