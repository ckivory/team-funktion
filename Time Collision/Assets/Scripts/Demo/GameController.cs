using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public List<Camera> cameras;
    public int startCamera;

    private int currentCamera;
    
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
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            try
            {
                currentCamera = 0;
                selectNew();
            }
            catch
            {
                print("Camera must exist!");
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            try
            {
                currentCamera = 1;
                selectNew();
            }
            catch
            {
                print("Camera must exist!");
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            try
            {
                currentCamera = 2;
                selectNew();
            }
            catch
            {
                print("Camera must exist!");
            }
        }
    }
}
