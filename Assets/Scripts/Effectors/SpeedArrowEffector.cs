using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedArrowEffector : Effector, ISpawnFromEditorObjectData
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
    public void CopyEditorObjectData(EditorObjectData editorObjectData)
    {
        transform.position = editorObjectData.position;
        transform.localScale = editorObjectData.scale;
        transform.rotation = Quaternion.Euler(0, 0, editorObjectData.rotation);
        speed = editorObjectData.GetSetting<float>("Speed");
    }

}

public class OnSpeedArrowTriggeredEventArgs {
    public bool spedUp;
    public float speed;
}