using System;
using UnityEngine;

public class GamemodeEffectorVisual : MonoBehaviour {

    [SerializeField] private GamemodeEffector gamemodeEffector;

    private Color color; // The color of the effector and the players new color;
    [SerializeField] private float alpha = 0.5f; // The alpha of the effectors sprite
    [SerializeField] private SpriteRenderer spriteRenderer; // The sprite renderer of the effector

    [SerializeField] private float shakeDuration = 0.2f;
    [SerializeField] private float shakeStrength = 1f;

    private void Start() {
        // get and set the corresponding color of the effector
        color = DefaultPlayerColors.defaultColorsDict[gamemodeEffector.GetPlayerGameModeState()];
        color.a = alpha;
        spriteRenderer.color = color;
        gamemodeEffector.OnEffectorTriggeredByPlayerEvent += GamemodeEffector_OnEffectorTriggeredByPlayerEvent;
    }

    private void GamemodeEffector_OnEffectorTriggeredByPlayerEvent(object sender, EventArgs e)
    {
        Debug.Log("Effector triggered by player");
        CameraShake.Instance.StartCameraShake(shakeStrength,shakeDuration);
    }
}