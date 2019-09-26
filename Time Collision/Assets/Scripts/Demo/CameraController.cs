using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Camera cam;
    public List<GameObject> players;
    
    private Vector3 playerCenter;
    public Vector3 baseOffset;
    private float scale;
    private float currentSpeed;
    public float cameraSpeed;
    public float cameraPadding;

    public float cameraMin;

    private void updateFocus()
    {
        playerCenter = Vector3.zero;
        if(players.Count >= 0)
        {
            foreach (GameObject player in players)
            {
                playerCenter += player.transform.position;
            }
            playerCenter /= players.Count;
        }
        
    }

    private void updateScale()
    {
        float maxDisplacement = 0f;
        float displacement;
        foreach (GameObject player in players)
        {
            displacement = (player.transform.position - playerCenter).magnitude;
            if (displacement > maxDisplacement)
            {
                maxDisplacement = displacement;
            }
        }
        scale = Mathf.SmoothDamp(scale, Mathf.Max(maxDisplacement, cameraMin), ref currentSpeed, cameraSpeed) + cameraPadding;
        cam.orthographicSize = scale;
        this.transform.position = playerCenter + (baseOffset.normalized * scale);
    }

    private void updateRotation()
    {
        this.transform.LookAt(playerCenter);
    }

    

    // Start is called before the first frame update
    void Start()
    {
        cam = this.GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        updateFocus();
        updateScale();
        updateRotation();
    }
}
