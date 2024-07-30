using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedArrowEffector : Effector
{

    [SerializeField] private float speed; // the speed of the arrow
    public event EventHandler<OnSpeedArrowTriggeredEventArgs> OnSpeedArrowTriggered;


    



    public override void OnEffectorTriggered(IHitboxEntity hitboxEntity)
    {
        if (hitboxEntity.isPlayer())
        {

            Player player = (Player)hitboxEntity;

            OnSpeedArrowTriggered?.Invoke(this, 
                new OnSpeedArrowTriggeredEventArgs
                {
                    spedUp = player.GetSpeed() < speed,
                    speed = speed
                });

            player.SetSpeed(speed);



        }
    }

}

public class OnSpeedArrowTriggeredEventArgs {
    public bool spedUp;
    public float speed;
}