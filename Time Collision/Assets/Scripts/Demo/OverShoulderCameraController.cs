using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverShoulderCameraController : MonoBehaviour
{
    public GameObject player;
    public Transform camTransform;

    public float cameraSpeed;

    public float sensitivityX = 4.0f;
    public float sensitivityY = 1.0f;
    
    private Camera cam;
    private int controllerNum;

    private float distance = 10.0f;

    private float currentX = 0.0f;
    private float currentY = 0.0f;
    private float targetX = 0.0f;
    private float targetY = 0.0f;
    private float omegaX;
    private float omegaY;
    
    private const float MIN_Y = 0.0f;
    private const float MAX_Y = 50.0f;

    private void Start()
    {
        camTransform = this.transform;
        controllerNum = player.GetComponent<PlayerController>().controllerNum;
        cam = GetComponent<Camera>();
    }

    private void updateInput()
    {
        targetX += Input.GetAxis("J" + controllerNum + "XRot") * cameraSpeed;
        targetY += Input.GetAxis("J" + controllerNum + "YRot") * cameraSpeed;
        targetY = Mathf.Clamp(targetY, MIN_Y, MAX_Y);

        currentX = Mathf.SmoothDamp(currentX, targetX, ref omegaX, sensitivityX);
        currentY = Mathf.SmoothDamp(currentY, targetY, ref omegaY, sensitivityY);
    }

    private void updatePosition()
    {
        Vector3 dir = new Vector3(0f, 0f, -distance);
        Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);
        camTransform.position = player.transform.position + rotation * dir;
    }

    private void Update()
    {
        updateInput();
    }

    private void LateUpdate()
    {
        updatePosition();
        this.transform.LookAt(player.transform.position);
    }
}
