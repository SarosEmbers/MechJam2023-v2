using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRigBehaviour : MonoBehaviour
{
    public float mouseSensitivity = 10f;
    float xRotation = 0f;
    float yRotation = 0f;
    public float minXRotation = -40f;
    public float maxXRotation = 80f;

    public PauseMenu pauseMenuManager;

    public void Start()
    {
        //Needs pausemenu in same scene and applied in the inspector
        pauseMenuManager = pauseMenuManager.GetComponent<PauseMenu>();
    }

    void Update()
    {
        if(pauseMenuManager.isPaused == false)
        {
           float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
           float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;
            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, minXRotation, maxXRotation);
            yRotation += mouseX;

            transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);
        }

    }
}
