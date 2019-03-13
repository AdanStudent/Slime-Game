using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraController : MonoBehaviour
{
    public float mouseSens = 10;
    public Transform target;
    public float dstFromTarget = 2;
    public Vector2 pitchMinMax = new Vector2(-40, 85);

    public float rotationSmoothTime = 0.12f;
    Vector3 rotationSmoothVel;
    Vector3 currentRotation;

    float yaw;
    float pitch;

    // Update is called once per frame
    void LateUpdate()
    {
        yaw += Input.GetAxis("Mouse X") * mouseSens;
        pitch -= Input.GetAxis("Mouse Y") * mouseSens;
        pitch = Mathf.Clamp(pitch, pitchMinMax.x, pitchMinMax.y);

        currentRotation = Vector3.SmoothDamp(currentRotation, new Vector3(pitch, yaw),
            ref rotationSmoothVel, rotationSmoothTime);

        transform.eulerAngles = currentRotation;
        transform.position = target.position - transform.forward * dstFromTarget;
    }
}
