using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Refereces")]
    [SerializeField] private PlayerInputManager playerInputManager;
    [SerializeField] private string wallTag;
    [SerializeField] private string hitboxTag;
    [SerializeField] private LineRenderer hookRenderer;


    [Header("Player Stats")]
    [Header("Player Movement variables")]
    [SerializeField] private float movementSpeed = 5f;
    [SerializeField] private float playerHookLength = 20f;

    [Header("Hook variables")]
    [SerializeField] private float hookIntoWallOffset = 0.5f;

    [Header("Debug variables")]
    [SerializeField] private Vector2 initialVelocity;

    private enum PlayerState {
        Free,
        Hooked
    }



    // Player variables
    private PlayerState currentPlayerState;
    private Vector2 hookedPosition;
    private float hookRadius;
    private Vector2 velocity;


    private void Start() {
        //SUBSCRIBE TO EVENTS
        playerInputManager.OnHookedInput += OnHook;

        //Set initial variables
        currentPlayerState = PlayerState.Free;
        velocity = initialVelocity;
    }


    private void Update()
    {
        HandleMovement();

        UpdateHookPosition();

    }

    private void HandleMovement()
    {
        switch (currentPlayerState)
        {
            case PlayerState.Free:
                HandleFreeMovement();
                break;
            case PlayerState.Hooked:
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

        currentPlayerState = PlayerState.Hooked;
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
        if (currentPlayerState == PlayerState.Hooked) {
            hookRenderer.SetPosition(0, transform.position);
        }
    }   

    /// <summary>
    /// when the player inputs the hook button
    /// </summary>
    private void OnHook(object sender, Vector2 direction)
    {
        PerformHook(direction);
    }
}
