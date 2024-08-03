using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelFinishEffector : Effector
{
    public override void OnEffectorTriggered(IHitboxEntity hitboxEntity)
    {
        Debug.Log("LevelFinishEffector triggered");
        if (hitboxEntity.isPlayer()) {
            OnLevelCompleted();
        }
    }

    private void OnLevelCompleted() {
        LevelHandler.Insance.BeginLevelCompletedSequence(transform);
    }
}
