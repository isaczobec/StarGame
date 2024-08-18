using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEditorObject : MonoBehaviour
{
    [SerializeField] private EditorObjectData editorObjectData;
    [Header("Visuals")]
    [SerializeField] private SpriteRenderer[] spriteRenderers;

    [SerializeField] private Color hoveredColor = Color.cyan;
    [SerializeField] private Color selectedColor = Color.magenta;

    public bool isHovered { get; private set; } = false;
    public bool isSelected { get; private set; } = false;

    [Header("EditorSettings")]
    public Vector2 minScale = new Vector2(0.1f, 0.1f);
    public Vector2 maxScale = new Vector2(10f, 10f);


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
        // update colors
        if (!isSelected) { // Do not change color if selected, selection is prioritized
            for (int i = 0; i < spriteRenderers.Length; i++) {
                spriteRenderers[i].color = isHovered ? hoveredColor : originalColors[i];
            }
        }
    }

    public void SetSelected(bool selected) {
        isSelected = selected;
        for (int i = 0; i < spriteRenderers.Length; i++) {
            Color color = isSelected ? selectedColor : isHovered ? hoveredColor : originalColors[i];
            spriteRenderers[i].color = color;
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
