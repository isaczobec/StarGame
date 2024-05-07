using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// which logic the players movement is calculated by
public enum PlayerMovementState {
    Free, // normal movement
    Hooked // radial movement around a point

}

public enum PlayerGameModeState {
    Hook, // radially attached to walls
    Zap, // press once to instantly change your momentum to that direction
    Normal, // normal bullet hell style movement, holding gives you a speed in a direction
    Glide, // normal movement but not with instant acceleration
}
public class Player : MonoBehaviour, IHitboxEntity
{
    [Header("Refereces")]
    [SerializeField] private PlayerInputManager playerInputManager;
    [SerializeField] private string wallTag;
    [SerializeField] private string hurtboxTag;
    [SerializeField] private LineRenderer hookRenderer;

    [SerializeField] private Hitbox playerHitbox;


    [Header("Player Stats")]
    [Header("Player Normal Movement variables")]
    [SerializeField] private float normalModeMovementSpeed = 5f;
    [SerializeField] private float sameDirectionAsBaseVelocityMultiplier = 0.7f; //the multiplier which the velocity is multiplied with if the player is moving in the same direction as the base velocity
    [Header("Player hook Movement variables")]
    [SerializeField] private float playerHookLength = 20f;
    [Header("Player glide Movement variables")]
    [SerializeField] private float glideModeDamping = 30f;
    [SerializeField] private float glideModeMaxMovementSpeed = 30f;
    [SerializeField] private float glideModeAcceleration = 5f;

    [Header("Hook variables")]
    [SerializeField] private float hookIntoWallOffset = 0.5f;

    [Header("Debug variables")]
    [SerializeField] private Vector2 initialVelocity;
    [SerializeField] private Transform spawnPoint;

    [SerializeField] private PlayerGameModeState startingGameModeState;



    public static Player Instance { get; private set; } // static singleton


    // -----------------------------

    // PLAYER VARIABLES OR STATS
    private PlayerMovementState currentPlayerMovementState;
    private PlayerGameModeState currentPlayerGameModeState;
    private Vector2 hookedPosition;
    private float hookRadius;
    private Vector2 baseVelocity; // a base velocity to calculate some movement from, etc bullet hell instantaneuos acceleration
    private Vector2 velocity; // the current actual velocity of the player that they move every frame
    private bool hookIsEnabled = false;


    // -----------------------------

    // EVENTS
    public event EventHandler<PlayerGameModeState> OnGameModeStateChange;


    private void Awake() {

        // Set the singleton
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
        }

    }


    private void Start() {
        //SUBSCRIBE TO EVENTS
        playerInputManager.OnMomentaryInput += OnMomentaryInput;

        //Set initial variables
        currentPlayerMovementState = PlayerMovementState.Free;
        velocity = initialVelocity;
        baseVelocity = initialVelocity;
        SetGameModeState(startingGameModeState); // set the starting game mode state, invoke function for extra functionality lol

        //Set the hitbox entity
        playerHitbox.SetOwnerHitboxEntity(this);
    }


    private void Update()
    {
        HandleMovement();

        UpdateHookPosition();

        HandleNormalModeInput();

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
            if (hit.collider.CompareTag(DefaultTagNames.wallTag)) { // if the player hit a wall
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

    private void HandleNormalModeInput() {


        Vector2 inputDirection = playerInputManager.GetPlayerMovementInput().normalized;
        switch (currentPlayerGameModeState) {
            case PlayerGameModeState.Normal:
                /// Normal mode movement
                NormalMovement(inputDirection);
                break;
            case PlayerGameModeState.Glide:
                /// Glide mode movement
                GlideMovement(inputDirection);
                break;
        }   
    }

    private void NormalMovement(Vector2 inputDirection)
    {

        // If the player is moving in the same direction as the base velocity, multiply the velocity by the sameDirectionAsBaseVelocityMultiplier (move slower in that direction)
        if (baseVelocity.x > 0) {
            inputDirection.x = inputDirection.x * sameDirectionAsBaseVelocityMultiplier;

            velocity.x = Mathf.Abs(1+inputDirection.x) * baseVelocity.x;
            velocity.y = inputDirection.y*normalModeMovementSpeed + baseVelocity.y;

        } else {
            inputDirection.y = inputDirection.y * sameDirectionAsBaseVelocityMultiplier;

            velocity.y = Mathf.Abs(1+inputDirection.y) * baseVelocity.y;
            velocity.x = inputDirection.x*normalModeMovementSpeed + baseVelocity.x;
        }

    }

    private void GlideMovement(Vector2 inputDirection)
    {
        velocity += inputDirection * glideModeAcceleration * Time.deltaTime;
        if (velocity.magnitude > glideModeMaxMovementSpeed)
        {
            velocity -= velocity.normalized * glideModeDamping * Time.deltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        
            Debug.Log("Player hit the hitbox");
        
    }

    private void PlayerDied() {
        UnHookPlayer();
        velocity = initialVelocity;
        transform.position = spawnPoint.position;
        SetGameModeState(startingGameModeState);
    }

    public void OnHurtBoxHit(HitboxTriggeredInfo hitboxTriggeredInfo)
    {
        PlayerDied();
    }

    public bool isPlayer()
    {
        return true;
    }


    public void SetGameModeState(PlayerGameModeState playerGameModeState)
    {
        if (currentPlayerGameModeState == PlayerGameModeState.Hook) {
            UnHookPlayer();
        }
        // If the player is currently in glide mode, set the velocity to be the same magnitude as the base velocity but retain the direction
        if (currentPlayerGameModeState == PlayerGameModeState.Glide) {
            velocity = velocity.normalized * baseVelocity.magnitude;
        }
        if (playerGameModeState == PlayerGameModeState.Normal) {
            
            // project the player velocity onto the axis with the speed magnitude, only allow movement in "straight" directions
            if (Mathf.Abs(velocity.x) > Mathf.Abs(velocity.y)) {
                velocity = new Vector2(velocity.x/Mathf.Abs(velocity.x) * velocity.magnitude, 0);
            } else {
                velocity = new Vector2(0, velocity.y/Mathf.Abs(velocity.y) * velocity.magnitude);
            }
            
            if (currentPlayerGameModeState != PlayerGameModeState.Normal) {
                baseVelocity = velocity;
            }

        } else { // if the player is not to be set into normal mode
            if (currentPlayerGameModeState == PlayerGameModeState.Normal) {
                velocity = baseVelocity;
            }

        if (playerGameModeState == PlayerGameModeState.Glide && currentPlayerGameModeState != PlayerGameModeState.Glide) {
            glideModeMaxMovementSpeed = velocity.magnitude;
            baseVelocity = velocity;
        }


        }
        // Change the game mode state
        currentPlayerMovementState = PlayerMovementState.Free;
        currentPlayerGameModeState = playerGameModeState;
        OnGameModeStateChange?.Invoke(this, playerGameModeState);
    }
}
