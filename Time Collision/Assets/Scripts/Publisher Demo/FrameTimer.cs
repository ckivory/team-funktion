using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrameTimer : MonoBehaviour
{
    void Update()
    {
        Debug.Log("FPS: " + (1 / Time.deltaTime));
    }
}
