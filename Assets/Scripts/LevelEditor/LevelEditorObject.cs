using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEditorObject : MonoBehaviour
{
    [SerializeField] private EditorObjectData editorObjectData;
    public EditorObjectData UpdateAndGetEditorObjectData() {
        editorObjectData.SetBaseSettings(this);
        return editorObjectData;
    }

    /// <summary>
    /// the sprite of this object.
    /// </summary>
    [SerializeField] private Sprite sprite;
    public Sprite GetSprite() {
        return sprite;
    }

    [Header("Settings")]
    [SerializeField] public Vector2 offsetWhenPlace;
}
