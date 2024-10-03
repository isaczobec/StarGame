using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileColorChanger : Effector, ISpawnFromEditorObjectData
{

    private const string colorRef = "Color";
    private const string transitionTimeRef = "Transition Time";

    [SerializeField] private Color color;
    [SerializeField] private float transitionTime;


    public override void OnEffectorTriggered(IHitboxEntity hitboxEntity)
    {
        // Change the background color if the hitbox entity is a player
        if (hitboxEntity.isPlayer())
        {
            WorldMaterialHandler.instance.SetTileColorAtIndex(0, color, transitionTime);
        }
    }
    public void CopyEditorObjectData(EditorObjectData editorObjectData)
    {
        // Copy the transform color settings from the editor object data to the background color changer
        editorObjectData.CopyTransformSettingsToGameObject(gameObject);
        color = ColorTransform.IntToColor(editorObjectData.GetSetting<int>(colorRef));
        transitionTime = editorObjectData.GetSetting<float>(transitionTimeRef);
    }
}
