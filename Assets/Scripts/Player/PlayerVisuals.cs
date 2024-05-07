using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class PlayerVisuals : MonoBehaviour
{


    // REFERENCES
    [SerializeField] private Player player;
    [SerializeField] private Material playerMaterial;
    [SerializeField] private string playerColorName = "_PlayerColor";
    [SerializeField] private Material trailMaterial;
    [SerializeField] private VisualEffect trailVFX;
    [SerializeField] private string trailVFXColorName = "Color";
    [SerializeField] private string trailMaterialColorName = "_Color";
    

    // COURUTINES
    private Coroutine fadeToPlayerColorCoroutine;


    // EXPOSED VARIABLES
    [SerializeField] private float RotationSpeed = 5f;
    [SerializeField] private float playerColorFadeDuration= 0.8f;


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
    }

}
