using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamemodeEffector : Effector
{

    [SerializeField] private PlayerGameModeState playerGameModeState; // The state this effector will set the player to
    private Color color; // The color of the effector and the players new color;
    [SerializeField] private float alpha = 0.5f; // The alpha of the effectors sprite
    [SerializeField] private SpriteRenderer spriteRenderer; // The sprite renderer of the effector


    private void Start()
    {
        // get and set the corresponding color of the effector
        color = DefaultPlayerColors.defaultColorsDict[playerGameModeState];
        color.a = alpha;
        spriteRenderer.color = color;
    }

    public override void OnEffectorTriggered(IHitboxEntity hitboxEntity)
    {
        if (hitboxEntity.isPlayer())
        {
            Player player = (Player)hitboxEntity;
            player.SetGameModeState(playerGameModeState);

        }
    }
}
