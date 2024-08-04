using System;
using System.Collections;
using UnityEngine;

public class LevelCompletedSequence : MonoBehaviour {



    [Header("References")]
    [SerializeField] private UIButtonAnimated exitToMainMenuButton;

    [Header("Settings")]

    [SerializeField] private float levelCompletedSequenceTime = 2f;
    [SerializeField] private float screenCoverTimeExitToMenu = 0.6f;



    // corountines
    private Coroutine levelCompletedSequenceCoroutine;



    public static LevelCompletedSequence Instance { get; private set; }
    private void Awake() {
        if (Instance != null) {
            Debug.LogWarning("LevelCompletedSequence already exists");
            Destroy(this);
            return;
        }
        Instance = this;
    }

    private void Start() {
        exitToMainMenuButton.OnUIButtonClicked += ExitToMainMenuButton_OnUIButtonClicked;
        LevelHandler.Insance.OnReturnToMenu += LevelHandler_OnReturnToMenu;
    }



    public void StartLevelCompletedSequence(Transform levelCompleteObjectTransform) {
        if (levelCompletedSequenceCoroutine != null) {
            Debug.LogWarning("LevelCompletedSequence already running");
           return;
        }
        levelCompletedSequenceCoroutine = StartCoroutine(LevelCompletedSequenceCoroutine(levelCompleteObjectTransform));
    }

    private IEnumerator LevelCompletedSequenceCoroutine(Transform levelCompleteObjectTransform) {

        Player.Instance.EnablePlayerFinishedLevelMode(levelCompleteObjectTransform.position);

        // change the camera
        PlayerCameraHandler.Instance.OnLevelCompleted();
        PlayerCameraHandler.Instance.SetCameraTargetObject(levelCompleteObjectTransform);

        yield return new WaitForSeconds(levelCompletedSequenceTime);

        // show the exit to main menu button
        exitToMainMenuButton.ChangeVisible(true);

        levelCompletedSequenceCoroutine = null;
    }



    
    // called when the exit to main menu button is clicked
    private void ExitToMainMenuButton_OnUIButtonClicked(object sender, EventArgs e)
    {
        LevelHandler.Insance.ExitToMainMenuScreenCovered(screenCoverTimeExitToMenu,screenCoverTimeExitToMenu);
    }
    private void LevelHandler_OnReturnToMenu(object sender, EventArgs e)
    {
        exitToMainMenuButton.ChangeVisible(false);
    }
    
}