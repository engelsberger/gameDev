using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    // TO BE ASSIGNED
    [SerializeField]
    private Transform camPosition;
    
    // Set these variables to what fits your game
    [SerializeField]
    private float mouseSensitivity = 500f;
    [SerializeField]
    private float maxVerticalViewAngle = 89f;

    private Camera cam;
    private float xRotation = 0f;
    private float yRotation = 0f;



    private void Awake()
    {
        cam = Camera.main;
        if(camPosition == null) Debug.LogError("CamPosition object not assigned!");
    }

    private void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -maxVerticalViewAngle, maxVerticalViewAngle);

        yRotation += mouseX;
        if (yRotation > 360) { yRotation -= 360; }
        if (yRotation < 0) { yRotation += 360; }

        cam.transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);
        cam.transform.position = camPosition.position;
    }
}
