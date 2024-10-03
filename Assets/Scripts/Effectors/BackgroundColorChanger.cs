using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundColorChanger : Effector, ISpawnFromEditorObjectData
{

    private const string color1Ref = "Color 1";
    private const string color2Ref = "Color 2";
    private const string transitionTimeRef = "Transition Time";

    [SerializeField] private Color color1;
    [SerializeField] private Color color2;
    [SerializeField] private float transitionTime;


    public override void OnEffectorTriggered(IHitboxEntity hitboxEntity)
    {
        // Change the background color if the hitbox entity is a player
        if (hitboxEntity.isPlayer())
        {
            WorldMaterialHandler.instance.SetBackgroundColorAtIndex(0, color1, transitionTime);
            WorldMaterialHandler.instance.SetBackgroundColorAtIndex(1, color2, transitionTime);
        }
    }
    public void CopyEditorObjectData(EditorObjectData editorObjectData)
    {
        // Copy the transform color settings from the editor object data to the background color changer
        editorObjectData.CopyTransformSettingsToGameObject(gameObject);
        color1 = ColorTransform.IntToColor(editorObjectData.GetSetting<int>(color1Ref));
        color2 = ColorTransform.IntToColor(editorObjectData.GetSetting<int>(color2Ref));
        transitionTime = editorObjectData.GetSetting<float>(transitionTimeRef);
    }
}
