using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectAttribute : MonoBehaviour
{
    // weight is the effective health of the player which also determine their current level of size
    // power is how much damage in terms of weight that will be dealth when the object hit the enemy player
    public float Power { get; set; }
    public float Weight { get; set; }
}
