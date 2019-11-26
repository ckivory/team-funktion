using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectAttributes : MonoBehaviour
{
    public GameObject whoFired;
    public int propNum;

    private static List<float> massList = new List<float> {1f, 1f, 5f, 3f, 1f, 2f, 1f, 0.5f, 2f, 2f};
    private static List<float> damageList = new List<float> {2f, 2f, 10f, 6f, 10f, 4f, 15f, 9f, 5f, 4f};

    public static float getMass(int p)
    {
        return massList[p];
    }

    public static float getDamage(int p)
    {
        return damageList[p];
    }
}
