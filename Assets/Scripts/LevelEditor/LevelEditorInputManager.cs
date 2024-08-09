using System;
using UnityEngine;

public class LevelEditorInputManager : MonoBehaviour {


    PlayerInput inputActions;


    public static LevelEditorInputManager instance;    

    public bool placePressedLastFrame = false;

    public event EventHandler<EventArgs> OnPlacePressed;

    private void Awake() {

        if (instance == null) {
            instance = this;
        } else {
            Debug.LogError("There is already an instance of LevelEditorInputManager in the scene.");
        }

        inputActions = new PlayerInput();
        inputActions.Editor.Enable();

    }


    private void Update() {
        if (GetPlayerIsPlacing()) {
            if (!placePressedLastFrame) {
                OnPlacePressed?.Invoke(this, EventArgs.Empty);
            }
            placePressedLastFrame = true;
        } else {
            placePressedLastFrame = false;
        }
    }

    /// <summary>
    /// Gets if the player is currently placing objects.
    /// </summary>
    /// <returns></returns>
    public bool GetPlayerIsPlacing() {
        return inputActions.Editor.Place.ReadValue<float>() > 0f;
    }

    public bool GetCameraControlModeEnabled() {
        return inputActions.Editor.CameraMoveMode.ReadValue<float>() > 0f;
    }

    public Vector2 GetMouseDelta() {
        return inputActions.Editor.MouseMovement.ReadValue<Vector2>();
    }

    public float GetZoomDelta() {
        return inputActions.Editor.Scroll.ReadValue<float>();
    }

    public bool GetShiftButtonPressed() {
        return inputActions.Editor.ShiftButton.ReadValue<float>() > 0f;
    }

    public bool GetControlButtonPressed() {
        return inputActions.Editor.ControlButton.ReadValue<float>() > 0f;
    }

    

}