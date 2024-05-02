using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Refereces")]
    [SerializeField] private PlayerInputManager playerInputManager;
    [SerializeField] private string wallTag;
    [SerializeField] private string hurtboxTag;
    [SerializeField] private LineRenderer hookRenderer;

    [SerializeField] private PlayerHitbox playerHitbox;


    [Header("Player Stats")]
    [Header("Player Movement variables")]
    [SerializeField] private float movementSpeed = 5f;
    [SerializeField] private float playerHookLength = 20f;

    [Header("Hook variables")]
    [SerializeField] private float hookIntoWallOffset = 0.5f;

    [Header("Debug variables")]
    [SerializeField] private Vector2 initialVelocity;
    [SerializeField] private Transform spawnPoint;

    [SerializeField] private PlayerGameModeState startingGameModeState;

    // which logic the players movement is calculated by
    private enum PlayerMovementState {
        Free, // normal movement
        Hooked // radial movement around a point

    }

    private enum PlayerGameModeState {
        Hook, // radially attached to walls
        Zap, // press once to instantly change your momentum to that direction
        Normal, // normal bullet hell style movement, holding gives you a speed in a direction
    }


    // Player variables
    private PlayerMovementState currentPlayerMovementState;
    private PlayerGameModeState currentPlayerGameModeState;
    private Vector2 hookedPosition;
    private float hookRadius;
    private Vector2 velocity;

    private bool hookIsEnabled = false;


    private void Start() {
        //SUBSCRIBE TO EVENTS
        playerInputManager.OnMomentaryInput += OnMomentaryInput;
        playerHitbox.OnHitObject += PlayerHitbox_OnHitObject;

        //Set initial variables
        currentPlayerMovementState = PlayerMovementState.Free;
        velocity = initialVelocity;
        currentPlayerGameModeState = startingGameModeState;
    }


    private void Update()
    {
        HandleMovement();

        UpdateHookPosition();

    }

    private void HandleMovement()
    {
        switch (currentPlayerMovementState)
        {
            case PlayerMovementState.Free:
                HandleFreeMovement();
                break;
            case PlayerMovementState.Hooked:
                HandleHookedMovement();
                break;
        }
        
    }

    private void HandleFreeMovement() {
        transform.position += (Vector3)velocity * Time.deltaTime;
    }

    private void HandleHookedMovement() {
        Vector2 radiusDirection = (hookedPosition - (Vector2)transform.position).normalized;
        velocity += MathF.Pow(velocity.magnitude,2) / hookRadius * radiusDirection * Time.deltaTime;
        transform.position += (Vector3)velocity * Time.deltaTime;
    }

    /// <summary>
    /// Attempt to do a hook
    /// </summary>
    /// <param name="direction"></param>
    private void PerformHook(Vector2 direction) {

        // Raycast to check if the player can hook to a wall
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, direction, playerHookLength);
        foreach (RaycastHit2D hit in hits) {
            if (hit.collider.CompareTag(wallTag)) { // if the player hit a wall
                HookToPosition(hit.point);
                break;
            }
        }
    }

    private void HookToPosition(Vector2 position) {

        

        currentPlayerMovementState = PlayerMovementState.Hooked;
        if (!hookIsEnabled) {EnableHook();}
        hookedPosition = position;
        hookRadius = Vector2.Distance(transform.position, hookedPosition);

        // Set the velocity to be perpendicular to the hook
        Vector2 hookVector = hookedPosition - (Vector2)transform.position;
        Vector2 perpVector = new Vector2(hookVector.y, -hookVector.x).normalized;
        if (Vector2.Angle(velocity, perpVector) > 90f) {
            perpVector *= -1;
        }
        velocity = perpVector * velocity.magnitude;

        // Set the line renderer positions
        hookRenderer.SetPosition(0, transform.position);
        hookRenderer.SetPosition(1, hookedPosition);
        
    }

    private void UpdateHookPosition() {
        if (currentPlayerMovementState == PlayerMovementState.Hooked) {
            hookRenderer.SetPosition(0, transform.position);
        }
    }

    private void UnHookPlayer() {
        currentPlayerMovementState = PlayerMovementState.Free;
        DisableHook();
    }

    private void EnableHook() {
        hookIsEnabled = true;
        hookRenderer.enabled = true;
    }

    private void DisableHook() {
        hookIsEnabled = false;
        hookRenderer.enabled = false;
    }

    /// <summary>
    /// when the player inputs the hook button
    /// </summary>
    private void OnMomentaryInput(object sender, Vector2 direction)
    {
        switch (currentPlayerGameModeState) {
            case PlayerGameModeState.Hook:
                PerformHook(direction);
                break;
            case PlayerGameModeState.Zap:
                velocity = direction * velocity.magnitude;
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        
            Debug.Log("Player hit the hitbox");
        
    }
    private void PlayerHitbox_OnHitObject(object sender, PlayerHitbox.OnHitboxTriggeredEventArgs e)
    {
        PlayerDied();
    }

    private void PlayerDied() {
        UnHookPlayer();
        velocity = initialVelocity;
        transform.position = spawnPoint.position;
    }
}
