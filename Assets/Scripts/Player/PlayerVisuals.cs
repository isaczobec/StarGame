using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class PlayerVisuals : MonoBehaviour
{


    // REFERENCES
    [Header("References")]
    [SerializeField] private Player player;
    [SerializeField] private Material playerMaterial;
    [SerializeField] private string playerColorName = "_PlayerColor";
    [SerializeField] private string playerFlashAmountName = "_FlashAmount";
    [SerializeField] private Material trailMaterial;
    [SerializeField] private VisualEffect trailVFX;
    [SerializeField] private string trailVFXColorName = "Color";
    [SerializeField] private string trailMaterialColorName = "_Color";

    [SerializeField] private PlayerRingVFX playerRingVFX;

    [Header("playerAnimator References")]
    [SerializeField] private Animator playerAnimator;

    // playerAnimator ref string
    private const string playerBleepRef = "PlayerBleep";
    private const string isHorisontalRef = "IsHorisontal";
    

    // COURUTINES
    private Coroutine fadeToPlayerColorCoroutine;
    private Coroutine setFlashAmountCoroutine;


    // EXPOSED VARIABLES
    [Header("rotation settings")]
    [SerializeField] private float RotationSpeed = 5f;
    [Header("player color settings")]
    [SerializeField] private float playerColorFadeDuration= 0.8f;
    [Header("player flash settings")]
    [SerializeField] private float playerFlashDurationClick = 0.12f;



    // VARIABLES
    private bool isFadingToPlayerColor = false;
    
    
    // SINGLETON PATTERN
    public static PlayerVisuals Instance { get; private set; }

    

    // ------------------------------------------------------------

    private void Awake() {

        // Singleton pattern
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(this);
        }
    }

    private void Start() {
        player.OnGameModeStateChange += Player_OnGameModeStateChange;
        player.OnPlayerMomentaryDirectionChanged += Player_OnPlayerMomentaryDirectionChanged;
    }


    void Update()
    {
        Rotate();
    }

    private void Rotate() {
        transform.Rotate(Vector3.forward * RotationSpeed * Time.deltaTime);
    }


    // ------------------------------------------------------------

    // -------FADING PLAYER COLORS-------



    private IEnumerator FadePlayerColorCoroutine(Color newColor, float duration) {
        isFadingToPlayerColor = true;

        float passedTime = 0;

        while (passedTime < duration) {
            Color calculatedColor = Color.Lerp(playerMaterial.GetColor(playerColorName), newColor, passedTime / duration);
            playerMaterial.SetColor(playerColorName, calculatedColor);
            trailMaterial.SetColor(trailMaterialColorName, calculatedColor);
            trailVFX.SetVector4(trailVFXColorName, calculatedColor);
            passedTime += Time.deltaTime;
            yield return null;
        }
        playerMaterial.SetColor(playerColorName, newColor);
        isFadingToPlayerColor = false;
    
        yield break;

    }

    public void FadePlayerColor(Color newColor, float duration) {
        newColor.a = 1;
        if (isFadingToPlayerColor) {
            StopCoroutine(fadeToPlayerColorCoroutine);
        }
        fadeToPlayerColorCoroutine = StartCoroutine(FadePlayerColorCoroutine(newColor, duration));
    }

    // this function changes the player color to match the color of the current game mode and does other visual stuff on game mode change
    private void Player_OnGameModeStateChange(object sender, PlayerGameModeState e)
    {
        Color newColor = DefaultPlayerColors.defaultColorsDict[e];
        FadePlayerColor(newColor,playerColorFadeDuration);
        playerRingVFX.SpawnRingVFX(newColor);
        ShockWaveHandler.instance.SpawnShockWave(player.transform.position, null);
        
    }


    // --------- making player bleep when the player changes direction

    
    private void Player_OnPlayerMomentaryDirectionChanged(object sender, Vector2 e)
    {
        // make the player bleep
        playerAnimator.SetBool(isHorisontalRef, e.x != 0);
        playerAnimator.SetTrigger(playerBleepRef);

        FlashPlayer(1f,playerFlashDurationClick);
    }


    // --------- setting flash amount

    private void FlashPlayer(float flashAmount, float duration) {
        if (setFlashAmountCoroutine != null) {
            StopCoroutine(setFlashAmountCoroutine);
        }
        setFlashAmountCoroutine = StartCoroutine(FlashAmountCoroutine(flashAmount, duration));
    }

    private IEnumerator FlashAmountCoroutine(float flashAmount, float duration) {

        playerMaterial.SetFloat(playerFlashAmountName, flashAmount);
        trailMaterial.SetFloat(playerFlashAmountName, flashAmount);
        yield return new WaitForSeconds(duration);
        playerMaterial.SetFloat(playerFlashAmountName, 0);
        trailMaterial.SetFloat(playerFlashAmountName, 0);
        yield break;
    }

}
