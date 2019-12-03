using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectAttributes : MonoBehaviour
{
    public GameObject whoFired;
    public int propNum;

    private static List<float> massList = new List<float> {2.5f, 2f, 5f, 1.5f, 1f, 2f, 3f, 1f, 2f, 4f, 6.5f};
    private static List<float> damageList = new List<float> {6f, 5f, 10f, 4f, 12f, 8f, 16f, 14f, 7f, 6.5f, 13f};

    public static float getMass(int p)
    {
        return massList[p];
    }

    public static float getDamage(int p)
    {
        return damageList[p];
    }
}
