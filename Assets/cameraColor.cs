using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraColor : MonoBehaviour
{

    public Camera leftEyeCamera;
    public Camera centerEyeCamera;
    public Camera rightEyeCamera;

    void Start()
    {
        // Set background color to black
        SetCameraBackgroundColor(leftEyeCamera, Color.black);
        SetCameraBackgroundColor(centerEyeCamera, Color.black);
        SetCameraBackgroundColor(rightEyeCamera, Color.black);
    }

    void SetCameraBackgroundColor(Camera cam, Color color)
    {
        if (cam != null)
        {
            cam.clearFlags = CameraClearFlags.SolidColor;
            cam.backgroundColor = color;
        }
    }
}

