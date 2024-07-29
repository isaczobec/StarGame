using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamemodeEffector : Effector
{

    [SerializeField] private PlayerGameModeState playerGameModeState; // The state this effector will set the player to
    public PlayerGameModeState GetPlayerGameModeState() { return playerGameModeState; }


    public event EventHandler<EventArgs> OnEffectorTriggeredByPlayerEvent;

    public override void OnEffectorTriggered(IHitboxEntity hitboxEntity)
    {
        if (hitboxEntity.isPlayer())
        {

            if (Player.Instance.GetIsInvulnerable()) {return;} // do not change the players game mode if they are invulnerable

            Player player = (Player)hitboxEntity;
            player.SetGameModeState(playerGameModeState);

            OnEffectorTriggeredByPlayerEvent?.Invoke(this, EventArgs.Empty);

        }
    }
}
