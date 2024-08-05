using UnityEngine;

public class LevelEditorInputManager : MonoBehaviour {


    PlayerInput inputActions;


    public static LevelEditorInputManager instance;    

    private void Awake() {

        if (instance == null) {
            instance = this;
        } else {
            Debug.LogError("There is already an instance of LevelEditorInputManager in the scene.");
        }

        inputActions = new PlayerInput();
        inputActions.Editor.Enable();

    }

    /// <summary>
    /// Gets if the player is currently placing objects.
    /// </summary>
    /// <returns></returns>
    public bool GetPlayerIsPlacing() {
        return inputActions.Editor.Place.ReadValue<float>() > 0f;
    }

    

}