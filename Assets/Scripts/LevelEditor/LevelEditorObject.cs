using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;

public class LevelEditorObject : MonoBehaviour
{
    [SerializeField] protected EditorObjectData editorObjectData;
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
    public bool halfStepGridOffset = false; // if the object should be offset by half a grid step when snapping. Usefull for block objects.
    private Vector2 currentSubPosition = Vector2.zero;
    public float minScaleIncrement = 0f;
    private Vector2 currentSubScale = new Vector2(1, 1);

    [SerializeField] private float shiftAddedAngleIncrementing = 45f;
    [SerializeField] private float shiftAddedPositionIncrementing = 1f;
    [SerializeField] private float shiftAddedScaleIncrementing = 1f;

    /// <summary>
    /// if this object can have nodes.
    /// </summary>
    [SerializeField] private bool canHaveNodes = false;
    [SerializeField] private int defaultNodeAmount = 4;
    [SerializeField] private int maxNodeAmount = 8;
    protected List<EditorObjectNode> nodes = new List<EditorObjectNode>();
    private bool nodesVisible = false;

    [SerializeField] private LineRenderer nodeLineRenderer;

    [Header("Settings")]
    [SerializeField] public Vector2 offsetWhenPlace;



    // -------- events ------

    /// <summary>
    /// Called when the object is selected or deselected. The bool is true if the object is selected, false if it is deselected.
    /// </summary>
    public event EventHandler<bool> OnSelectedChanged;


    // -------- overridable methods ------

    /// <summary>
    /// Called every frame when a node is being dragged.
    /// </summary>
    public virtual void OnNodeMoved(EditorObjectNode node) {
    }
    protected virtual void OnObjectMoved() {
    }
    protected virtual void OnObjectRotated() {
    }
    protected virtual void OnObjectScaled() {
    }
    protected virtual void OnObjectSetup() {
    }
    public virtual void OnSettingChanged<T>(string settingName, T value) {

    }

    private Color[] originalColors;

    private bool initialized = false;

    private bool nodesLoaded = false;

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

        // initialize nodes
        if (canHaveNodes && !nodesLoaded) { // if the object can have nodes
            // create nodes
            for (int i = 0; i < defaultNodeAmount; i++) {
                CreateNode(Vector2.up * i,i);
            }
            nodesLoaded = true;
        }

        OnObjectSetup(); // call the on object setup function

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
        if (selected == isSelected) return; // do not do anything if the object is already selected
        isSelected = selected;
        for (int i = 0; i < spriteRenderers.Length; i++) {
            Color color = isSelected ? selectedColor : isHovered ? hoveredColor : originalColors[i];
            spriteRenderers[i].color = color;
        }
        if(selected) {
            ShowNodes();
        } else {
            HideNodes();
        }
        OnSelectedChanged?.Invoke(this, isSelected);
    }
    
    public EditorObjectData UpdateAndGetEditorObjectData() {
        editorObjectData.SetBaseSettings(this);
        editorObjectData.editorObjectNodes = GetEditorObjectNodeDatas();
        
        return editorObjectData;

    }


    public void AddRotation(float angle) {
        currentSubRotation += angle;

        float rot = 0;

        float minIncrement = LevelEditorInputManager.instance.GetShiftButtonPressed() ? Mathf.Max(minRotationIncrement,shiftAddedAngleIncrementing) : minRotationIncrement;

        if (minIncrement != 0) {
            // clamp to closest increment of minRotationIncrement
            float remainder = currentSubRotation % minIncrement;
            rot = currentSubRotation - remainder;
        } else {
            rot = currentSubRotation;
        }

        transform.rotation = Quaternion.Euler(0, 0, rot);

        
        if (angle != 0) OnObjectRotated(); // call the on object rotated function
        if (nodesVisible) UpdateLineRenderer(); // update the line renderer if the nodes are visible
    }

    public void AddPosition(Vector2 position) {

        currentSubPosition += position;

        float minIncrement = LevelEditorInputManager.instance.GetShiftButtonPressed() ? Mathf.Max(minPositionIncrement,shiftAddedPositionIncrementing) : minPositionIncrement;

        Vector2 pos;
        if (minIncrement != 0f) {
            // clamp to closest increment of minPositionIncrement
            float xRemainder = currentSubPosition.x % minIncrement;
            float yRemainder = currentSubPosition.y % minIncrement;
            pos = new Vector2(currentSubPosition.x - xRemainder, currentSubPosition.y - yRemainder) + (halfStepGridOffset ? new Vector2(-0.5f, -0.5f) : Vector2.zero);
        } else {
            pos = currentSubPosition;
        }

        transform.position = new Vector3(pos.x, pos.y, transform.position.z);

        if (position != Vector2.zero) OnObjectMoved(); // call the on object moved function
        if (nodesVisible) UpdateLineRenderer(); // update the line renderer if the nodes are visible

    }

    public void MultiplyScale(Vector2 scale) {
        currentSubScale = new Vector2(currentSubScale.x * scale.x, currentSubScale.y * scale.y);

        float minIncrement = LevelEditorInputManager.instance.GetShiftButtonPressed() ? Mathf.Max(minScaleIncrement,shiftAddedScaleIncrementing) : minScaleIncrement;

        Vector2 newScale;
        if (minIncrement != 0f) {
            // clamp to closest increment of minScaleIncrement
            float xRemainder = currentSubScale.x % minIncrement;
            float yRemainder = currentSubScale.y % minIncrement;
            newScale = new Vector2(currentSubScale.x - xRemainder, currentSubScale.y - yRemainder);
        } else {
            newScale = currentSubScale;
        }

        Vector2 clampedScale = new Vector2(Mathf.Clamp(newScale.x, minScale.x, maxScale.x), Mathf.Clamp(newScale.y, minScale.y, maxScale.y));
        transform.localScale = new Vector3(clampedScale.x, clampedScale.y, transform.localScale.z);

        if (scale != Vector2.one) OnObjectScaled(); // call the on object scaled function
        if (nodesVisible) UpdateLineRenderer(); // update the line renderer if the nodes are visible

    }

    /// <summary>
    /// Shows the nodes of this object. Called when the object is selected.
    /// </summary>
    private void ShowNodes() {
        foreach (EditorObjectNode node in nodes) {
            node.SetVisualsEnabled(true);
        }
        UpdateLineRenderer(); // show the line renderer
        nodesVisible = true;
    }

    /// <summary>
    /// Hides the nodes of this object. Called when the object is deselected.
    /// </summary>
    private void HideNodes() {
        foreach (EditorObjectNode node in nodes) {
            node.SetVisualsEnabled(false);
        }
        HideLineRenderer(); // hide the line renderer
        nodesVisible = false;
    }

    /// <summary>
    /// Deletes all nodes of this object.
    /// </summary>
    public void DeleteNodes() {
        foreach (EditorObjectNode node in nodes) {
            Destroy(node.gameObject);
        }
        nodes.Clear();
    }

    private void CreateNode(Vector2 position, int index) {
        GameObject nodeObject = Instantiate(GeneralEditorPrefabs.instance.GetEditorObjectNodePrefab(), position, Quaternion.identity, EditorObjectNodeParent.instance.transform);
        EditorObjectNode node = nodeObject.GetComponent<EditorObjectNode>();
        nodes.Add(node);
        EditorObjectNode previousNode  = index > 1 ? nodes[index - 2] : null;
        node.Setup(gameObject, position,index,previousNode);
        nodesLoaded = true;
    }


    private List<EditorObjectNodeData> GetEditorObjectNodeDatas() {
        List<EditorObjectNodeData> nodeDatas = new List<EditorObjectNodeData>();
        foreach (EditorObjectNode node in nodes) {
            nodeDatas.Add(node.GetNodeData());
        }
        return nodeDatas;
    }

    /// <summary>
    /// Loads the node data into the object.
    /// </summary>
    /// <param name="nodeDatas"></param>
    private void LoadEditorObjectNodeData(List<EditorObjectNodeData> nodeDatas) {
        for (int i = 0; i < nodeDatas.Count; i++) {
            EditorObjectNodeData nodeData = nodeDatas[i];
            CreateNode(nodeData.relativePosition,i);
        }
    }

    public EditorObjectNode GetNodeAtIndex(int index) {
        if (index < 0 || index >= nodes.Count) return null;
        return nodes[index];
    }

    public void UpdateLineRenderer() {
        if (nodeLineRenderer == null) return;
        if (nodes.Count == 0) return;
        Vector3[] positions = new Vector3[nodes.Count];
        for (int i = 0; i < nodes.Count; i++) {
            positions[i] = nodes[i].relativePosition + (Vector2)transform.position;
        }
        nodeLineRenderer.positionCount = nodes.Count;
        nodeLineRenderer.SetPositions(positions);
    }

    public void HideLineRenderer() {
        if (nodeLineRenderer == null) return;
        nodeLineRenderer.positionCount = 0;
    }

    


    public string GetObjectID() {
        return editorObjectData.SpawnnableObjectID;
    }

    public void SetEditorObjectData(EditorObjectData editorObjectData) {
        this.editorObjectData = editorObjectData;
        LoadEditorObjectNodeData(editorObjectData.editorObjectNodes);
    }

    public EditorObjectData GetEditorObjectData() {
        return editorObjectData;
    }

    /// <summary>
    /// the sprite of this object.
    /// </summary>
    [SerializeField] private Sprite sprite;
    public Sprite GetSprite() {
        return sprite;
    }

}
