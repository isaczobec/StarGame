using UnityEngine;

public class EditorCamera : MonoBehaviour {

    [SerializeField] private Camera mainCamera;

    [SerializeField] private float mouseSensitivity = 0.1f;
    [SerializeField] private float zoomSensitivity = 0.1f;
    [SerializeField] private float minOrthoScale = 2f;
    [SerializeField] private float maxOrthoScale = 40f;



    private void MoveCameraWithMouse() {
        Vector2 mouseDelta = LevelEditorInputManager.instance.GetMouseDelta();
        transform.position -= new Vector3(mouseDelta.x,mouseDelta.y,0f) * mouseSensitivity * mainCamera.orthographicSize; // multiply by orthographic size to make the movement speed independent of the zoom level
    }

    private void HandleZooming() {
        float zoomDelta = LevelEditorInputManager.instance.GetZoomDelta();
        mainCamera.orthographicSize -= zoomDelta * zoomSensitivity;
        //clamp
        mainCamera.orthographicSize = Mathf.Clamp(mainCamera.orthographicSize, minOrthoScale, maxOrthoScale);
    }

    private void Update() {
        if (LevelEditorInputManager.instance.GetCameraControlModeEnabled()) {
            MoveCameraWithMouse();
        }
        HandleZooming();
    }

}