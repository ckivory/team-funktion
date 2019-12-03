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
    public float mouseSensitivity = 0.5f;
    
    private Camera cam;
    private int controllerNum;

    public float distance = 20.0f;

    public float zoomScale = 15f;
    public float zoomLerp = 1f;

    private float zoomAmount;

    public float upOffset = 10.0f;

    public float minYRot = 10.0f;
    public float maxYRot = 30.0f;

    public float initialRotation;

    private float currentX = 0.0f;
    private float currentY = 0.0f;
    private float targetX = 0.0f;
    private float targetY = 0.0f;
    private float omegaX;
    private float omegaY;

    private void Start()
    {
        camTransform = this.transform;

        targetX = currentX = initialRotation;

        controllerNum = player.GetComponent<PDPlayerController>().controllerNum;
        cam = GetComponent<Camera>();

        zoomAmount = 1f;
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
        targetY = Mathf.Clamp(targetY, minYRot, maxYRot);

        currentX = Mathf.SmoothDamp(currentX, targetX, ref omegaX, sensitivityX);
        currentY = Mathf.SmoothDamp(currentY, targetY, ref omegaY, sensitivityY);
    }

    private void updateZoom()
    {
        float newZoomAmount = 1 + (player.GetComponent<PDPlayerController>().level / zoomScale);
        float timedLerp = Mathf.Clamp(zoomLerp * Time.deltaTime, 0f, 1f);
        zoomAmount = (zoomAmount * (1 - timedLerp)) + (newZoomAmount * timedLerp);
    }

    private void updatePosition()
    {
        updateZoom();
        Vector3 dir = new Vector3(0f, 0f, -distance * zoomAmount);
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
        camTransform.position += new Vector3(0f, upOffset, 0f);
    }
}
