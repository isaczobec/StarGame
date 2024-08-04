using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// An effector that can trigger an event when it is activated. Usually refered to from different objects in the scene
/// </summary>
public class TriggerEffector : Effector
{
    public event EventHandler<TriggerHitEventArgs> OnTriggered;

    public override void OnEffectorTriggered(IHitboxEntity hitboxEntity)
    {
        TriggerHitEventArgs args = new TriggerHitEventArgs
        {
            hitboxEntity = hitboxEntity,
            isPlayer = hitboxEntity.isPlayer()
        };

        OnTriggered?.Invoke(this, args);
    }

}

public class TriggerHitEventArgs {
    public IHitboxEntity hitboxEntity;
    public bool isPlayer;
}
