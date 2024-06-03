using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    public static SpawnPoint Instance { get; private set; }

    [SerializeField] private PlayerGameModeState playerGameModeState;
    [SerializeField] private Vector2 startVelocity;


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

    public void InitializePlayer() {
        Player.Instance.transform.position = transform.position;
        Player.Instance.SetGameModeState(playerGameModeState);
        Player.Instance.SetVelocity(startVelocity);
        Player.Instance.SetPlayerMenuState(PlayerMenuState.active);
        Player.Instance.SetPlayerSpawnPoint(this);
    }
}
