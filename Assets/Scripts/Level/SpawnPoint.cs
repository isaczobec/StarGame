using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
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
        if (changeVelocity) Player.Instance.SetVelocity(startVelocity);
        Player.Instance.SetPlayerMenuState(isMenuSpawnPoint ? PlayerMenuState.mainMenu : PlayerMenuState.active);
        Player.Instance.SetPlayerSpawnPoint(this);
    }

    public PlayerGameModeState GetStartingPlayerGameModeState() {
        return playerGameModeState;
    }

}
