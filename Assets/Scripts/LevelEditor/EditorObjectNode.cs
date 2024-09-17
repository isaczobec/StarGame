using System;
using System.Data.Common;
using UnityEditor;
using UnityEngine;

/// <summary>
/// A class for a node belonging to an editor object
/// </summary>
public class EditorObjectNode : MonoBehaviour {

    [SerializeField] private GameObject visualObject;
    [SerializeField] private UIButton nodeButton; // the button that is used to click and drag this node
    [SerializeField] private TMPro.TextMeshProUGUI indexText; // the text that displays the index of the node
    private GameObject parentEditorObject;
    private LevelEditorObject levelEditorObject; // the object that this node belongs to
    private EditorObjectNode previousNode; // the previous node in the list

    public Vector2 relativePosition = Vector2.zero;

    private bool isDragging = false;
    private bool isVisible = false;
    private int index;

    public void Setup(GameObject parentEditorObject, Vector2 relativePosition, int index, EditorObjectNode previousNode = null) {
        this.parentEditorObject = parentEditorObject;
        levelEditorObject = parentEditorObject.GetComponent<LevelEditorObject>();
        this.relativePosition = relativePosition;
        this.index = index + 1; // add 1 to the index to make it 1-indexed
        this.previousNode = previousNode;
        SetTextVisuals();
        SetVisualsEnabled(false);
    }

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

    private void SetTextVisuals() {
        indexText.text = index.ToString();
    }


    private void Update() {
        if (isVisible) {
            if (isDragging) { // set the position of the node to the mouse position relative to the parent object
                Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                relativePosition = mousePos - (Vector2)parentEditorObject.transform.position;
                levelEditorObject.UpdateLineRenderer(); // update the line renderer
                levelEditorObject.OnNodeMoved(this); // call on node moved function
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