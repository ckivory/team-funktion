using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public List<Camera> cameras;
    public int startCamera;

    private int currentCamera;

    private void cycleCameras()
    {
        currentCamera = (currentCamera + 1) % cameras.Count;
    }

    private void selectNew()
    {
        foreach(Camera cam in cameras)
        {
            cam.enabled = false;
        }
        cameras[currentCamera].enabled = true;
    }

    void Start()
    {
        currentCamera = startCamera;
        selectNew();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.C))
        {
            cycleCameras();
            selectNew();
        }
    }
}
