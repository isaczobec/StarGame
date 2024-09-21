using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// An effector that can trigger an event when it is activated. Usually refered to from different objects in the scene
/// </summary>
public class TriggerEffector : Effector, ISpawnFromEditorObjectData
{
    public event EventHandler<TriggerHitEventArgs> OnTriggered;

    public static event EventHandler<int> OnEffectorTriggeredByPlayerIndexed;

    /// <summary>
    /// The index associated with this trigger effector. Can be used to identify the trigger effector
    /// </summary>
    public int index;

    public override void OnEffectorTriggered(IHitboxEntity hitboxEntity)
    {
        TriggerHitEventArgs args = new TriggerHitEventArgs
        {
            hitboxEntity = hitboxEntity,
            isPlayer = hitboxEntity.isPlayer()
        };

        OnTriggered?.Invoke(this, args);

        if (args.isPlayer) OnEffectorTriggeredByPlayerIndexed?.Invoke(this, index);
    }

    public void CopyEditorObjectData(EditorObjectData editorObjectData)
    {
        index = editorObjectData.GetSetting<int>("Index"); // set the index
        // set transform
        transform.position = editorObjectData.position;
        transform.rotation = Quaternion.Euler(0,0,editorObjectData.rotation);
        transform.localScale = editorObjectData.scale;
    }
}

public class TriggerHitEventArgs {
    public IHitboxEntity hitboxEntity;
    public bool isPlayer;
}
