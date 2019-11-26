using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PDOverShoulderCameraController : MonoBehaviour
{
    public GameObject player;
    private Transform camTransform;

    public float cameraSpeed = 120;

    public float sensitivityX = 0.125f;
    public float sensitivityY = 0.25f;
    public float mouseSensitivity = 3f;
    
    private Camera cam;
    private int controllerNum;

    public float distance = 20.0f;

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
        controllerNum = player.GetComponent<PDPlayerController>().controllerNum;
        cam = GetComponent<Camera>();
    }

    private void updateInput()
    {
        if(player.GetComponent<PDPlayerController>().usingController)
        {
            targetX += Input.GetAxis("J" + controllerNum + "XRot") * cameraSpeed * Time.deltaTime;
            targetY += Input.GetAxis("J" + controllerNum + "YRot") * cameraSpeed * Time.deltaTime;
        }
        else
        {
            targetX += Input.GetAxis("Mouse X") * cameraSpeed * mouseSensitivity * Time.deltaTime;
            targetY += -1 * Input.GetAxis("Mouse Y") * cameraSpeed * mouseSensitivity * Time.deltaTime;
        }
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
