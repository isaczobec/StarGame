using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamemodeEffector : Effector
{

    [SerializeField] private PlayerGameModeState playerGameModeState; // The state this effector will set the player to
    [SerializeField] private Color color; // The color of the effector and the players new color;
    [SerializeField] private float colorAlpha = 100f; // The alpha of the color
    [SerializeField] private float fadeDuration; // The duration of the fade
    [SerializeField] private SpriteRenderer spriteRenderer; // The sprite renderer of the effector


    private void Start()
    {
        color.a = colorAlpha;
        spriteRenderer.color = color;
    }

    public override void OnEffectorTriggered(IHitboxEntity hitboxEntity)
    {
        if (hitboxEntity.isPlayer())
        {
            Player player = (Player)hitboxEntity;
            player.SetGameModeState(playerGameModeState);

            PlayerVisuals.Instance.FadePlayerColor(color,fadeDuration);
        }
    }
}
