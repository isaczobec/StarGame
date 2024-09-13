using System;
using UnityEditor;
using UnityEngine;

/// <summary>
/// A class for a node belonging to an editor object
/// </summary>
public class EditorObjectNode : MonoBehaviour {

    [SerializeField] private GameObject visualObject;
    [SerializeField] private UIButton nodeButton; // the button that is used to click and drag this node

    private GameObject parentEditorObject;

    private Vector2 relativePosition = Vector2.zero;

    private bool isDragging = false;
    private bool isVisible = false;

    public void SetVisualsEnabled(bool enabled) {
        isVisible = enabled;
        if (enabled) {
            nodeButton.OnUIButtonPressed += OnNodeButtonPressed;
            nodeButton.OnUIButtonReleased += OnNodeButtonReleased;
            EditorBuildingManager.instance.AddEditorUIButton(nodeButton);
        } else {
            nodeButton.OnUIButtonPressed -= OnNodeButtonPressed;
            nodeButton.OnUIButtonReleased -= OnNodeButtonReleased;
            EditorBuildingManager.instance.RemoveEditorUIButton(nodeButton);
        }
        visualObject.SetActive(enabled);
    }    

    public void Setup(GameObject parentEditorObject, Vector2 relativePosition) {
        this.parentEditorObject = parentEditorObject;
        this.relativePosition = relativePosition;
        SetVisualsEnabled(false);
    }

    private void Update() {
        if (isVisible) {
            if (isDragging) { // set the position of the node to the mouse position relative to the parent object
                Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                relativePosition = mousePos - (Vector2)parentEditorObject.transform.position;
            }
            transform.position = Camera.main.WorldToScreenPoint(relativePosition + (Vector2)parentEditorObject.transform.position);
        }
    }

    private void OnNodeButtonPressed(object sender, EventArgs e)
    {
        isDragging = true;
    }

    private void OnNodeButtonReleased(object sender, EventArgs e)
    {
        isDragging = false;
    }

    /// <summary>
    /// Returns the data of this node for saving.
    /// </summary>
    /// <returns></returns>
    public EditorObjectNodeData GetNodeData() {
        EditorObjectNodeData data = new EditorObjectNodeData
        {
            relativePosition = relativePosition
        };
        return data;
    }
}

[Serializable]
public class EditorObjectNodeData {
    public Vector2 relativePosition;
}