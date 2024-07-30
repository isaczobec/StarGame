using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
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
    Practice, // Invulnerable and moves freely with wasd
}

public enum PlayerMenuState {
    active,
    mainMenu
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
    [SerializeField] private float glideModeDamping = 60f;
    [SerializeField] private float glideModeMaxMovementSpeed = 30f;
    [SerializeField] private float glideModeAcceleration = 45f;

    [Header("Hook variables")]
    [SerializeField] private float hookIntoWallOffset = 0.5f;


    [Header("Death settings")]
    [SerializeField] private float respawnTime = 1f;
    [SerializeField] private float moveBackToSpawnPointTime = 0.7f; // when the player invisibly teleports back to spawn and the camera starts to reset after the player dies


    [Header("Practice mode variables")]
    [SerializeField] private bool practiceModeEnabled = false;
    [SerializeField] private float practiceModeSpeed = 10f;
    [SerializeField] private Vector2 practiceModeSpawnDirection = Vector2.up;

    [Header("Debug variables")]
    [SerializeField] private float initialSpeed;
    [SerializeField] private Vector2 initialDirection;

    [SerializeField] private PlayerGameModeState startingGameModeState;



    public static Player Instance { get; private set; } // static singleton


    // -----------------------------

    // PLAYER VARIABLES OR STATS
    private PlayerMovementState currentPlayerMovementState;
    private PlayerGameModeState currentPlayerGameModeState;
    private PlayerMenuState currentPlayerMenuState;
    private Vector2 hookedPosition;
    private float hookRadius;
    private float baseSpeed; // a base speed to calculate some movement from, etc bullet hell instantaneuos acceleration
    private Vector2 baseDirection; // Should be a normalized cardinal direction. a base Velocity to calculate some movement from, etc bullet hell instantaneuos acceleration
    private Vector2 velocity; // the current actual velocity of the player that they move every frame
    private bool hookIsEnabled = false;

    private bool letPlayerMove = false;

    private SpawnPoint playerSpawnPoint;
    
    private bool playerIsInvulnerable = false;

    // death
    private bool playerIsDead = false;


    // practice mode
    private Vector2 practiceModeSpawnPoint = Vector2.zero;
    private PlayerGameModeState practiceModeGameModeState = PlayerGameModeState.Practice;


    // ---- coroutines ----

    private Coroutine respawnPlayerCoroutine;

    // -----------------------------

    // EVENTS
    public event EventHandler<PlayerGameModeState> OnGameModeStateChange;
    public event EventHandler<PlayerDeathEventArgs> OnPlayerDeath;
    /// <summary>
    /// Called when the player spawns or respawns
    /// </summary>
    public event EventHandler<EventArgs> OnPlayerSpawn; 
    public event EventHandler<PlayerMenuState> OnPlayerMenuStateChange;

    public event EventHandler<Vector2> OnPlayerMomentaryDirectionChanged;

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
        currentPlayerMenuState = PlayerMenuState.mainMenu;
        SetVelocity(initialDirection.normalized * initialSpeed);
        SetGameModeState(startingGameModeState); // set the starting game mode state, invoke function for extra functionality lol

        //Set the hitbox entity
        playerHitbox.SetOwnerHitboxEntity(this);
    }

    public void InitializePlayer()
    {
        if (!practiceModeEnabled) { // if the player is not in practice mode
            SpawnPoint.Instance.InitializePlayer();
        } else {

            // if the player is in practice mode
            baseDirection = practiceModeSpawnDirection;
            MoveToSpawnPoint();
            velocity = practiceModeSpawnDirection * baseSpeed;
            SetGameModeState(practiceModeGameModeState);
            SpawnPoint.Instance.InitializePlayer(changePosition:false,changeVelocity:false,changeGameMode:false);
        }

        OnPlayerSpawn?.Invoke(this, EventArgs.Empty);
    }

    private void Update()
    {


        // if the player is not dead, handle movement
        if (!playerIsDead) {
            HandleMovement();
            UpdateHookPosition();
            HandleNormalModeInput();
        }

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
        if (!letPlayerMove) { return; }

        if (playerIsDead) { return; }

        if (velocity.normalized != direction.normalized) {

            switch (currentPlayerGameModeState) {
                case PlayerGameModeState.Hook:
                    PerformHook(direction);
                OnPlayerMomentaryDirectionChanged?.Invoke(this, direction);
                    break;
                case PlayerGameModeState.Zap:
                    velocity = direction * baseSpeed;
                OnPlayerMomentaryDirectionChanged?.Invoke(this, direction);
                    break;
            }
        }
    }

    private void HandleNormalModeInput() {

        if (!letPlayerMove) { return; }
        if (playerIsDead) { return; }
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
            case PlayerGameModeState.Practice:
                PracticeMovement(inputDirection);
                break;
        }   
    }

    private void NormalMovement(Vector2 inputDirection)
    {

        // If the player is moving in the same direction as the base velocity, multiply the velocity by the sameDirectionAsBaseVelocityMultiplier (move slower in that direction)
        if (Mathf.Abs(baseDirection.x) > Mathf.Abs(baseDirection.y)) { // if the base direction is horizontal

            velocity.x = (inputDirection.x*sameDirectionAsBaseVelocityMultiplier + baseDirection.x) * baseSpeed;
            velocity.y = inputDirection.y*normalModeMovementSpeed;

        } else { // if the base direction is vertical
            
                velocity.x = inputDirection.x*normalModeMovementSpeed;
                velocity.y = (inputDirection.y*sameDirectionAsBaseVelocityMultiplier + baseDirection.y) * baseSpeed;
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

    private void PracticeMovement(Vector2 inputDirection) {
        velocity = inputDirection * practiceModeSpeed;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        
            Debug.Log("Player hit the hitbox");
        
    }

    /// <summary>
    /// Called when the player dies. Sets the player to be dead and starts the respawn coroutine.
    /// </summary>
    private void PlayerDied()
    {
        UnHookPlayer();
        playerIsDead = true;
        StartPlayerRespawn();

        OnPlayerDeath?.Invoke(this, 
            new PlayerDeathEventArgs {
                respawnTime = respawnTime
            });
    }


    // ----- respawning -----

    /// <summary>
    /// Starts the player respawn coroutine. Cancels the previous respawn coroutine if it exists.
    /// </summary>
    private void StartPlayerRespawn() {
        if (respawnPlayerCoroutine != null) {
            StopCoroutine(respawnPlayerCoroutine);
        }
        StartCoroutine(RespawnPlayerCoroutine());
    }

    private IEnumerator RespawnPlayerCoroutine() {
        yield return new WaitForSeconds(moveBackToSpawnPointTime);

        MoveToSpawnPoint();

        yield return new WaitForSeconds(respawnTime - moveBackToSpawnPointTime);

        InitializePlayer();
        playerIsDead = false;
    }

    /// <summary>
    /// Moves the player to the spawnpoint.
    /// </summary>
    private void MoveToSpawnPoint() {
        transform.position = practiceModeSpawnPoint;
    }


    public void OnHurtBoxHit(HitboxTriggeredInfo hitboxTriggeredInfo)
    {
        if (playerIsInvulnerable || playerIsDead) { return; } // do not take damage if the player is invulnerable
        PlayerDied();
    }

    public bool isPlayer()
    {
        return true;
    }


    // not very well laid out 
    public void SetGameModeState(PlayerGameModeState playerGameModeState)
    {

        // do not to anything if we try to enter practice mode from practice mode
        if (currentPlayerGameModeState == PlayerGameModeState.Practice && playerGameModeState == PlayerGameModeState.Practice) {
            return;
        }

        if (currentPlayerGameModeState == PlayerGameModeState.Hook) {
            UnHookPlayer();
        }

        if (currentPlayerGameModeState != PlayerGameModeState.Normal) { // do not set the base direction if the player is in normal mode since that gamemode has one locked movement direction

            // set the base direction to be the (main) direction of the velocity
            if (Mathf.Abs(velocity.x) > Mathf.Abs(velocity.y)) {
                if (velocity.x >= 0) {
                    baseDirection = new Vector2(1, 0);
                }
                else
                {
                    baseDirection = new Vector2(-1, 0);
                }
            } else {
                if (velocity.y >= 0)
                {
                    baseDirection = new Vector2(0, 1);
                }
                else
                {
                    baseDirection = new Vector2(0, -1);
                }
            }

            velocity = velocity.normalized * baseSpeed;
        } else { // if the player is in normal mode, set the velocity to be the base direction
            velocity = baseDirection * baseSpeed;
        }


        if (playerGameModeState == PlayerGameModeState.Glide && currentPlayerGameModeState != PlayerGameModeState.Glide) {
            glideModeMaxMovementSpeed = baseSpeed;
        }

        // make the player invulnerable if they ender  practice mode
        if (playerGameModeState == PlayerGameModeState.Practice) {
            playerIsInvulnerable = true;
        } else {
            playerIsInvulnerable = false;
        }

        // Change the game mode state
        currentPlayerMovementState = PlayerMovementState.Free;
        currentPlayerGameModeState = playerGameModeState;
        OnGameModeStateChange?.Invoke(this, playerGameModeState);
    }

    public PlayerGameModeState GetGameModeState() {
        return currentPlayerGameModeState;
    }

    /// <summary>
    /// Gets the base speed of the player, ie the speed of the last speed arrow they hit
    /// </summary>
    /// <returns></returns>
    public float GetSpeed() {
        return baseSpeed;
    }

    /// <summary>
    /// Set the speed of the player
    /// </summary>
    /// <param name="speed"></param>
    public void SetSpeed(float speed) {
        baseSpeed = speed;

        switch (currentPlayerGameModeState) {
            case PlayerGameModeState.Glide:
                glideModeMaxMovementSpeed = baseSpeed;
                velocity = velocity.normalized * speed / velocity.magnitude;
                break;
            case PlayerGameModeState.Hook:
                velocity = velocity.normalized * speed;
                break;
            case PlayerGameModeState.Normal:
                break; // nothing to do here
            case PlayerGameModeState.Zap:
                velocity = velocity.normalized * speed;
                break;
        }
    }

    public Vector2 GetVelocity() {
        return velocity;
    }

    /// <summary>
    /// Set the velocity of the player. Should be in a cardinal direction (Fix please?)
    /// <summary>
    public void SetVelocity(Vector2 velocity) {
        this.velocity = velocity;
        baseSpeed = velocity.magnitude;
        baseDirection = velocity.normalized;
    }


    public void SetPlayerMenuState(PlayerMenuState playerMenuState) {
        currentPlayerMenuState = playerMenuState;
        switch (playerMenuState) {
            case PlayerMenuState.active:
                letPlayerMove = true;
                break;
            case PlayerMenuState.mainMenu:
                letPlayerMove = false;
                break;
        }
        OnPlayerMenuStateChange?.Invoke(this, playerMenuState);
    }

    public PlayerMenuState GetPlayerMenuState() {
        return currentPlayerMenuState;
    }

    public void SetPlayerSpawnPoint(SpawnPoint spawnPoint) {
        playerSpawnPoint = spawnPoint;
    }

    public void SetBaseDirection(Vector2 direction) {
        baseDirection = direction;
    }
    public Vector2 GetBaseDirection() {
        return baseDirection;
    }





    // ------------------- PRACTICE MODE -------------------

    public void SetPracticeModeEnabled(bool enabled) {
        practiceModeEnabled = enabled;
        if (enabled) {
            SetGameModeState(PlayerGameModeState.Practice);
        } else {
            SetGameModeState(SpawnPoint.Instance.GetStartingPlayerGameModeState());
        }
        
    }
    public bool GetPracticeModeEnabled() {
        return practiceModeEnabled;
    }

    public bool GetIsInvulnerable() {
        return playerIsInvulnerable;
    }

    public void SetPracticeModeSpawnDirection(Vector2 direction) {
        practiceModeSpawnDirection = direction;
    }
    public Vector2 GetPracticeModeSpawnDirection() {
        return practiceModeSpawnDirection;
    }

    public void SetPracticeModeSpawnPoint(Vector2 spawnPoint) {
        practiceModeSpawnPoint = spawnPoint;
    }

    public Vector2 GetPracticeModeStartVelocity() {
        return initialDirection.normalized * initialSpeed;
    }

    public void SetGameModeStatePracticeMode(PlayerGameModeState gameModeState) {
        practiceModeGameModeState = gameModeState;
        SetGameModeState(gameModeState);
        velocity = practiceModeSpawnDirection * baseSpeed;
    }

    public void KillPlayer() {
        PlayerDied();
    }


}

// event args


public class PlayerDeathEventArgs {
    public float respawnTime;
}