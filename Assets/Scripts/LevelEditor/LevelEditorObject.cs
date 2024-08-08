using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEditorObject : MonoBehaviour
{
    [SerializeField] private EditorObjectData editorObjectData;
    [Header("Visuals")]
    [SerializeField] private SpriteRenderer[] spriteRenderers;

    [SerializeField] private Color hoveredColor = Color.cyan;
    [SerializeField] private Color selectedColor = Color.green;

    public bool isHovered { get; private set; } = false;
    public bool isSelected { get; private set; } = false;


    private Color[] originalColors;

    private bool initialized = false;

    public void Initialize() {
        // save the original colors of the sprite renderers
        originalColors = new Color[spriteRenderers.Length];
        for (int i = 0; i < spriteRenderers.Length; i++) {
            originalColors[i] = spriteRenderers[i].color;
        }
        initialized = true;
    }

    private void Start() {
        if (!initialized) {
            Initialize();
        }
    }

    public void SetHovered(bool hovered) {
        isHovered = hovered;
        for (int i = 0; i < spriteRenderers.Length; i++) {
            spriteRenderers[i].color = isHovered ? hoveredColor : originalColors[i];
        }
    }

    public void SetSelected(bool selected) {
        isSelected = selected;
        for (int i = 0; i < spriteRenderers.Length; i++) {
            spriteRenderers[i].color = isSelected ? selectedColor : originalColors[i];
        }
    }
    
    public EditorObjectData UpdateAndGetEditorObjectData() {
        editorObjectData.SetBaseSettings(this);
        
        return editorObjectData;

    }

    public string GetObjectID() {
        return editorObjectData.SpawnnableObjectID;
    }

    public void SetEditorObjectData(EditorObjectData editorObjectData) {
        this.editorObjectData = editorObjectData;
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
