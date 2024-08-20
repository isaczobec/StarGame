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

    /// <summary>
    /// What increments this object can be rotated by.
    /// </summary>
    public float minRotationIncrement = 0f;
    private float currentSubRotation = 0f;
    public float minPositionIncrement = 0f;
    private Vector2 currentSubPosition = Vector2.zero;
    public float minScaleIncrement = 0f;
    private Vector2 currentSubScale = new Vector2(1, 1);
    
    [Header("Settings")]
    [SerializeField] public Vector2 offsetWhenPlace;




    private Color[] originalColors;

    private bool initialized = false;

    public void Initialize() {
        // save the original colors of the sprite renderers
        originalColors = new Color[spriteRenderers.Length];
        for (int i = 0; i < spriteRenderers.Length; i++) {
            originalColors[i] = spriteRenderers[i].color;
        }
        initialized = true;

        // set the current rotation scale and position

        currentSubRotation = transform.rotation.eulerAngles.z;
        currentSubPosition = transform.position;
        currentSubScale = transform.localScale;
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


    public void AddRotation(float angle) {
        currentSubRotation += angle;

        float rot = 0;

        if (minRotationIncrement != 0) {
            // clamp to closest increment of minRotationIncrement
            float remainder = currentSubRotation % minRotationIncrement;
            rot = currentSubRotation - remainder;
        } else {
            rot = currentSubRotation;
        }

        transform.rotation = Quaternion.Euler(0, 0, rot);
    }

    public void AddPosition(Vector2 position) {

        currentSubPosition += position;

        Vector2 pos;
        if (minPositionIncrement != 0f) {
            // clamp to closest increment of minPositionIncrement
            float xRemainder = currentSubPosition.x % minPositionIncrement;
            float yRemainder = currentSubPosition.y % minPositionIncrement;
            pos = new Vector2(currentSubPosition.x - xRemainder, currentSubPosition.y - yRemainder);
        } else {
            pos = currentSubPosition;
        }

        transform.position = new Vector3(pos.x, pos.y, transform.position.z);

    }

    public void MultiplyScale(Vector2 scale) {
        currentSubScale = new Vector2(currentSubScale.x * scale.x, currentSubScale.y * scale.y);

        Vector2 newScale;
        if (minScaleIncrement != 0f) {
            // clamp to closest increment of minScaleIncrement
            float xRemainder = currentSubScale.x % minScaleIncrement;
            float yRemainder = currentSubScale.y % minScaleIncrement;
            newScale = new Vector2(currentSubScale.x - xRemainder, currentSubScale.y - yRemainder);
        } else {
            newScale = currentSubScale;
        }

        Vector2 clampedScale = new Vector2(Mathf.Clamp(newScale.x, minScale.x, maxScale.x), Mathf.Clamp(newScale.y, minScale.y, maxScale.y));
        transform.localScale = new Vector3(clampedScale.x, clampedScale.y, transform.localScale.z);
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

}
