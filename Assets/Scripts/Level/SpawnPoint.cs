using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour, ISpawnFromEditorObjectData
{
    public static SpawnPoint Instance { get; private set; }

    [SerializeField] private PlayerGameModeState playerGameModeState;
    [SerializeField] private Vector2 startVelocity;
    [SerializeField] private Vector2 initialDirection = Vector2.up; // should be cardinal and normalized

    [SerializeField] private bool isMenuSpawnPoint = false;


    private void Awake() {
        Debug.Log("SpawnPoint Awake");
        Instance = this; // always overwrite the instance because a new level was loaded
    }

    private void Start() {
        InitializePlayer();
    }

    public Vector3 GetPosition() {
        return transform.position;
    }

    public void InitializePlayer(
            bool changePosition = true,
            bool changeGameMode = true,
            bool changeVelocity = true
        ) {
        if (changePosition) Player.Instance.transform.position = transform.position;
        if (changeGameMode) Player.Instance.SetGameModeState(playerGameModeState);
        if (changeVelocity) {
            Player.Instance.SetVelocity(startVelocity);
            Player.Instance.SetSpeed(startVelocity.magnitude);
        }
        Player.Instance.SetPlayerMenuState(isMenuSpawnPoint ? PlayerMenuState.mainMenu : PlayerMenuState.active);
        Player.Instance.SetPlayerSpawnPoint(this);
    }

    public PlayerGameModeState GetStartingPlayerGameModeState() {
        return playerGameModeState;
    }

    public void CopyEditorObjectData(EditorObjectData editorObjectData)
    {
        editorObjectData.CopyTransformSettingsToGameObject(gameObject); // set transform
        startVelocity.x = editorObjectData.GetSetting<float>("Start Velocity X");
        startVelocity.y = editorObjectData.GetSetting<float>("Start Velocity Y");
        // convert startVelocity to initialDirection
        if (Math.Abs(startVelocity.x) > Math.Abs(startVelocity.y)) {
            initialDirection = new Vector2(Math.Sign(startVelocity.x), 0);
        } else {
            initialDirection = new Vector2(0, Math.Sign(startVelocity.y));
        }

        // set playerGameModeState
        int startingGamemode = editorObjectData.GetSetting<int>("Starting Gamemode");
        switch (startingGamemode) {
            case 1:
                playerGameModeState = PlayerGameModeState.Normal;
                break;
            case 2:
                playerGameModeState = PlayerGameModeState.Zap;
                break;
            case 3:
                playerGameModeState = PlayerGameModeState.Glide;
                break;
            default:
                playerGameModeState = PlayerGameModeState.Hook;
                break;
        }
    }
}
