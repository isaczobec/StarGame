using System.Collections;
using UnityEngine;

public class LevelCompletedSequence : MonoBehaviour {

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

        yield return null;
    }
    
}