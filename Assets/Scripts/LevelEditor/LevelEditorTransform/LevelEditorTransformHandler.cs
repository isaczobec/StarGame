using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEditorTransformHandler : MonoBehaviour
{

    [SerializeField] private GameObject positionButtonPrefab;
    [SerializeField] private GameObject rotationButtonPrefab;
    [SerializeField] private GameObject scaleButtonPrefab;

    public static LevelEditorTransformHandler instance {get; private set;}

    private List<LevelEditorTransformButton> levelEditorTransformButtons = new List<LevelEditorTransformButton>();

    private void Awake() {
        if (instance == null) {
            instance = this;
        } else {
            Debug.LogWarning("There are multiple LevelEditorTransform instances in the scene. Destroying the newest one.");
            Destroy(this);
        }
    }

    private void Start() {
        LevelEditorObjectManager.instance.OnLevelEditorObjectsSelected += OnLevelEditorObjectsSelected;
    }

    private void Update() {
        // update all buttons
        foreach (LevelEditorTransformButton button in levelEditorTransformButtons) {
            button.FrameUpdate(CalculateTransformButtonInfo(LevelEditorObjectManager.instance.GetSelectedEditorObjects()));
        }
    }



    // called when new objects are selected
    private void OnLevelEditorObjectsSelected(object sender, List<LevelEditorObject> selectedObjects)
    {
        // create button objects
        List<LevelEditorTransformButton> newButtons = CreateTransformButtons();

        // calculate info
        TransformButtonInfo transformButtonInfo = CalculateTransformButtonInfo(selectedObjects);

        // initialize buttons
        for (int i = 0; i < newButtons.Count; i++)
        {
            newButtons[i].InitializeButton(transformButtonInfo, i);
        }
    }

    private TransformButtonInfo CalculateTransformButtonInfo(List<LevelEditorObject> selectedObjects)
    {
        Vector2 averagePosition = CalculateAverageObjectPosition(selectedObjects);
        Vector2 upperRightCornerBounds = GetBounds(selectedObjects, true);
        Vector2 lowerLeftCornerBounds = GetBounds(selectedObjects, false);
        TransformButtonInfo transformButtonInfo = new TransformButtonInfo
        {
            selectedObjects = selectedObjects,
            averagePosition = averagePosition,
            upperRightCornerBounds = upperRightCornerBounds,
            lowerLeftCornerBounds = lowerLeftCornerBounds
        };
        return transformButtonInfo;
    }

    /// <summary>
    /// Creates gameObjects for the transform buttons and adds them to the levelEditorTransformButtons list. DOES NOT initialize the buttons, this is done in the InitializeButton method on the buttons themselves.
    /// </summary>
    private List<LevelEditorTransformButton> CreateTransformButtons() {
        List<LevelEditorTransformButton> transformButtons = new List<LevelEditorTransformButton>();
        GameObject positionButton = Instantiate(positionButtonPrefab, transform);
        // GameObject rotationButton = Instantiate(rotationButtonPrefab, transform);
        // GameObject scaleButton = Instantiate(scaleButtonPrefab, transform);
        transformButtons.Add(positionButton.GetComponent<LevelEditorTransformButton>());
        // transformButtons.Add(rotationButton.GetComponent<LevelEditorTransformButton>());
        // transformButtons.Add(scaleButton.GetComponent<LevelEditorTransformButton>());

        EditorBuildingManager.instance.AddEditorUIButton(positionButton.GetComponent<UIButton>());

        levelEditorTransformButtons.AddRange(transformButtons);
        return transformButtons;
    }

    private Vector2 CalculateAverageObjectPosition(List<LevelEditorObject> editorObjects) {
        Vector2 averagePosition = Vector2.zero;
        foreach (LevelEditorObject editorObject in editorObjects) {
            averagePosition += (Vector2)editorObject.transform.position;
        }
        averagePosition /= editorObjects.Count;
        return averagePosition;
    }

    private Vector2 GetBounds(List<LevelEditorObject> editorObjects, bool upperRight) {
        Vector2 bounds = new Vector2(float.MinValue, float.MinValue);
        foreach (LevelEditorObject editorObject in editorObjects) {
            if (upperRight) {
                bounds.x = Mathf.Max(bounds.x, editorObject.transform.position.x);
                bounds.y = Mathf.Max(bounds.y, editorObject.transform.position.y);
            } else {
                bounds.x = Mathf.Min(bounds.x, editorObject.transform.position.x);
                bounds.y = Mathf.Min(bounds.y, editorObject.transform.position.y);
            }
        }
        return bounds;
    }


}

/// <summary>
/// A class used to store information about the selected objects when a transform button is pressed.
/// </summary>
public class TransformButtonInfo {
    public List<LevelEditorObject> selectedObjects;
    public Vector2 averagePosition;
    public Vector2 upperRightCornerBounds;
    public Vector2 lowerLeftCornerBounds;
}