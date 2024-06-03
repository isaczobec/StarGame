using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraHandler : MonoBehaviour
{
    
    public enum CameraState { 
        activeGame, 
        menu 
        }


    [Header("References")]
    [SerializeField] private Cinemachine.CinemachineVirtualCamera virtualCamera;

    [Header("Settings")]
    [SerializeField] private float menuYOffset = 14f;
    [SerializeField] private const float defaultMoveTime = 0.5f;
    [SerializeField] private float defaultPlayerZOffset = -10f;


    // ---- Variables ----
    private CameraState cameraState = CameraState.menu;
    private Coroutine moveCameraCoroutine;

    public static PlayerCameraHandler Instance { get; private set; }


    // ---- METHODS FOR CAMERA POSITIONING ----

    private void Awake() {
        Instance = this;
    }
    
    private void Start() {
        SetCameraState(cameraState,0f);
        Player.Instance.OnPlayerMenuStateChange += Player_OnPlayerMenuStateChange;
    }


    public void SetCameraState(CameraState cameraState, float moveTime = defaultMoveTime) {
        this.cameraState = cameraState;
        switch (cameraState) {
            case CameraState.activeGame:
                StartMoveCameraCoroutine(Vector2.zero, moveTime);
                break;
            case CameraState.menu:
                StartMoveCameraCoroutine(new Vector2(0,menuYOffset), moveTime);
                break;
        }
    }

    private void StartMoveCameraCoroutine(Vector2 toPosition, float time)
    {
        if (moveCameraCoroutine != null)
        {
            StopCoroutine(moveCameraCoroutine);
        }
        moveCameraCoroutine = StartCoroutine(MoveCameraToPosition(toPosition, time));
    }

    private IEnumerator MoveCameraToPosition(Vector2 targetPosition, float time) {

        Cinemachine.CinemachineTransposer transposer = virtualCamera.GetCinemachineComponent<Cinemachine.CinemachineTransposer>();

        Vector3 offsetTargetPosition = new Vector3(targetPosition.x, targetPosition.y, defaultPlayerZOffset); // Set z to default value
        Debug.Log("Move camera to position: " + offsetTargetPosition);
        Vector3 startPosition = transposer.m_FollowOffset;
        float passedTime = 0f;

        while (passedTime < time) {
            transposer.m_FollowOffset = Vector3.Lerp(startPosition, offsetTargetPosition, passedTime/time);
            passedTime += Time.deltaTime;
            yield return null;
        }

        transposer.m_FollowOffset = offsetTargetPosition;
    }
    private void Player_OnPlayerMenuStateChange(object sender, PlayerMenuState s)
    {
        switch (s)
        {
            case PlayerMenuState.mainMenu:
                SetCameraState(CameraState.menu);
                break;
            case PlayerMenuState.active:
                SetCameraState(CameraState.activeGame);
                break;
        }
    }

}
