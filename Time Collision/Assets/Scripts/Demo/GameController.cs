using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public Camera skyCam;

    private List<Camera> cameras;
    public int startCamera;

    public List<GameObject> players;
    
    private int currentCamera;
    private void registerCameras()
    {
        cameras.Add(skyCam);
        foreach (GameObject player in players)
        {
            cameras.Add(player.transform.GetChild(2).GetComponent<Camera>());
        }
    }

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
        cameras = new List<Camera>();
        registerCameras();
        currentCamera = startCamera % cameras.Count;
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
