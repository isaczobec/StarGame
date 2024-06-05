using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedArrowEffector : Effector
{

    [SerializeField] private float speed; // the speed of the arrow
    public event EventHandler<EventArgs> OnSpeedArrowVisualTriggered;

    



    public override void OnEffectorTriggered(IHitboxEntity hitboxEntity)
    {
        if (hitboxEntity.isPlayer())
        {

            Player player = (Player)hitboxEntity;
            player.SetSpeed(speed);

            OnSpeedArrowVisualTriggered?.Invoke(this, EventArgs.Empty);


        }
    }

}
