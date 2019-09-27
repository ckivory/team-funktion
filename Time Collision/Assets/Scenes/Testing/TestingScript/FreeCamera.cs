using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeCamera : MonoBehaviour
{
    public class MovementState : MonoBehaviour
    {
        public float xAng;
        public float yAng;
        public float zAng;
        public float xPos;
        public float yPos;
        public float zPos;

        public void SetFromTransform(Transform t)
        {
            xAng = t.eulerAngles.x;
            yAng = t.eulerAngles.y;
            zAng = t.eulerAngles.z;
            xPos = t.position.x;
            yPos = t.position.y;
            zPos = t.position.z;
        }

        public void Translate(Vector3 translation)
        {
            Vector3 rotatedTranslation = Quaternion.Euler(xAng, yAng, zAng) * translation;

            xPos += rotatedTranslation.x;
            yPos += rotatedTranslation.y;
            zPos += rotatedTranslation.z;
        }

        public void LerpTowards(MovementState target, float positionLerp, float rotationLerp)
        {
            xAng = Mathf.Lerp(xAng, target.xAng, rotationLerp);
            yAng = Mathf.Lerp(yAng, target.yAng, rotationLerp);
            zAng = Mathf.Lerp(zAng, target.zAng, rotationLerp);

            xPos = Mathf.Lerp(xPos, target.xPos, positionLerp);
            yPos = Mathf.Lerp(yPos, target.yPos, positionLerp);
            zPos = Mathf.Lerp(zPos, target.zPos, positionLerp);
        }

        public void UpdateTransform(Transform t)
        {
            t.eulerAngles = new Vector3(xAng, yAng, zAng);
            t.position = new Vector3(xPos, yPos, zPos);
        }
    }
    Vector3 GetInputTranslationDirection()
    {
        Vector3 direction = new Vector3();
        if (Input.GetKey(KeyCode.W))
        {
            direction += Vector3.forward;
        }
        if (Input.GetKey(KeyCode.S))
        {
            direction += Vector3.back;
        }
        if (Input.GetKey(KeyCode.A))
        {
            direction += Vector3.left;
        }
        if (Input.GetKey(KeyCode.D))
        {
            direction += Vector3.right;
        }
        return direction;
    }

    MovementState TargetState = new MovementState();
    MovementState BetweenState = new MovementState();

    public float positionLerpTime = 0.5f;
    public float rotationLerpTime = 0.05f;
    public float movementSpeed = 10f;
    public Rigidbody rb;

    void OnEnable()
    {
        rb = GetComponent<Rigidbody>();
        TargetState.SetFromTransform(transform);
        BetweenState.SetFromTransform(transform);
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }

        // Rotation
        if (Input.GetMouseButton(1))
        {
            var mouseMovement = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

            TargetState.yAng += mouseMovement.x;
            TargetState.xAng += mouseMovement.y * (-1);
        }

        // Translation
        var translation = GetInputTranslationDirection() * movementSpeed * Time.deltaTime;

        TargetState.Translate(translation);

        float lerpInterval = Mathf.Log(0.01f);
        var positionLerp = 1f - Mathf.Exp((lerpInterval / positionLerpTime) * Time.deltaTime);
        var rotationLerp = 1f - Mathf.Exp((lerpInterval / rotationLerpTime) * Time.deltaTime);
        BetweenState.LerpTowards(TargetState, positionLerp, rotationLerp);
        BetweenState.UpdateTransform(transform);
    }

}
