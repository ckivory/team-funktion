using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public List<GameObject> players;
    
    private Vector3 playerCenter;
    public Vector3 baseOffset;
    private float scale;
    public float scalingFactor;

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
        scale = 0;
        float displacement;
        foreach (GameObject player in players)
        {
            displacement = (player.transform.position - playerCenter).magnitude;
            if (displacement > scale)
            {
                scale = displacement;
            }
        }
        print(scale);
    }

    private void updatePosition()
    {
        this.transform.position = playerCenter + (baseOffset * ((scale / scalingFactor))) + (baseOffset.normalized * cameraMin);   // The further away the players are, the further out the camera will go.
    }

    private void updateRotation()
    {
        this.transform.LookAt(playerCenter);
    }

    

    // Start is called before the first frame update
    void Start()
    {
        updateFocus();
        updateScale();
        updatePosition();
        updateRotation();
    }

    // Update is called once per frame
    void Update()
    {
        updateFocus();
        updateScale();
        updatePosition();
        updateRotation();
    }
}
