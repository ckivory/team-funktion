using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrthographicCameraController : MonoBehaviour
{
    public List<GameObject> players;
    public Vector3 baseOffset;
    public float cameraSpeed;
    public float cameraMin;

    private Camera cam;
    private Vector3 playerCenter;
    private float scale;
    private float currentSpeed;

    private void updateFocus()
    {
        playerCenter = Vector3.zero;
        if(players.Count >= 0)
        {
            foreach (GameObject player in players)
            {
                if(player != null)
                {
                    playerCenter += player.transform.position;
                }
            }
            playerCenter /= players.Count;
        }
        
    }

    private void updateScale()
    {
        float displacement;
        float maxDisplacement = 0f;
        foreach (GameObject player in players)
        {
            if(player != null)
            {
                displacement = (player.transform.position - playerCenter).magnitude;
                if (displacement > maxDisplacement)
                {
                    maxDisplacement = displacement;
                }
            }
        }
        scale = Mathf.SmoothDamp(scale, Mathf.Max(maxDisplacement, cameraMin), ref currentSpeed, cameraSpeed);
        cam.orthographicSize = scale;
        this.transform.position = playerCenter + (baseOffset.normalized * scale);
    }

    private void updateRotation()
    {
        this.transform.LookAt(playerCenter);
    }

    void Start()
    {
        cam = this.GetComponent<Camera>();
    }

    void Update()
    {
        updateFocus();
        updateScale();
        updateRotation();
    }
}
