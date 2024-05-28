using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopyCameraSettings : MonoBehaviour
{


    [SerializeField] private Camera sourceCamera;
    [SerializeField] private Camera targetCamera;

    [Header("Settings to copy")]
    [SerializeField] private bool copyFieldOfView = true;
    [SerializeField] private bool copyPosition = true;
    [SerializeField] private bool copyRotation = true;

    void Update()
    {
        UpdateCameraSettings();
    }

    /// <summary>
    /// Update the target camera settings to match the source camera settings
    /// </summary>
    private void UpdateCameraSettings() {

        if (copyFieldOfView) {targetCamera.fieldOfView = sourceCamera.fieldOfView;}
        if (copyPosition) {targetCamera.transform.position = sourceCamera.transform.position;}
        if (copyRotation) {targetCamera.transform.rotation = sourceCamera.transform.rotation;}

    }
}
