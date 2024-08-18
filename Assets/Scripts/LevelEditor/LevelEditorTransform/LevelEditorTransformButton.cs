using System;
using System.Collections.Generic;
using UnityEngine;

public class LevelEditorTransformButton : MonoBehaviour {

    [SerializeField] private UIButton button;

    protected TransformButtonInfo latestTransformButtonInfo;

    bool isPressed = false;
    protected Vector2 lastMousePositionWorldPos;

    private void Start() {
        button.OnUIButtonPressed += OnUIButtonClicked;
        button.OnUIButtonReleased += OnUIButtonReleased;
    }

    /// <summary>
    /// Called from LevelEditorTransformHandler when the button is created.
    /// </summary>
    /// <param name="selectedObjects"></param>
    public void FrameUpdate(TransformButtonInfo transformButtonInfo) {
        if (isPressed) {
            FrameUpdateWhilePressed(transformButtonInfo);
        } else {
            FrameUpdateWhileUnpressed(transformButtonInfo);
        }

        if (transformButtonInfo != null) {
            latestTransformButtonInfo = transformButtonInfo;
        }
    }

    /// <summary>
    /// Called every frame when the button is not pressed. Override this method to add custom functionality.
    /// </summary>
    /// <param name="transformButtonInfo"></param>
    public virtual void FrameUpdateWhileUnpressed(TransformButtonInfo transformButtonInfo) {

    }

    /// <summary>
    /// Called every frame when the button is pressed. Override this method to add custom functionality.
    /// </summary>
    public virtual void FrameUpdateWhilePressed(TransformButtonInfo transformButtonInfo) {

    }

    public virtual void InitializeButtonInSubClass(TransformButtonInfo transformButtonInfo, int index) {
    }

    /// <summary>
    /// Initializes the button. Called from LevelEditorTransformHandler when the button is created. 
    /// </summary>
    /// <param name="index"></param>
    public void InitializeButton(TransformButtonInfo transformButtonInfo, int index) {
        if (transformButtonInfo != null) {
            latestTransformButtonInfo = transformButtonInfo;
            InitializeButtonInSubClass(transformButtonInfo, index);
        }
    }

    private void OnUIButtonClicked(object sender, EventArgs e)
    {
        SetIsPressed(true);
    }
    private void OnUIButtonReleased(object sender, EventArgs e)
    {
        SetIsPressed(false);
    }

    private void SetIsPressed(bool isPressed) {
        this.isPressed = isPressed;
    }

    /// <summary>
    /// Makes the button set its world position to match the average position of the selected objects.
    /// </summary>
    /// <param name="offset"></param>
    public void SetScreenPosition(Vector2 offset = new Vector2(), bool transformOffsetToScreenPos = false) {
        if (latestTransformButtonInfo != null) {
            Vector2 objectScreenPos = !transformOffsetToScreenPos? Camera.main.WorldToScreenPoint(latestTransformButtonInfo.averagePosition) : Camera.main.WorldToScreenPoint(latestTransformButtonInfo.averagePosition + offset);
            transform.position = !transformOffsetToScreenPos? objectScreenPos + offset : objectScreenPos;
        }
    }


    protected void SetLastMousePosition()
    {
        lastMousePositionWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    protected Vector2 GetMouseDeltaWorldPosition()
    {
        Vector2 currentMousePositionWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 deltaMousePosition = currentMousePositionWorldPos - lastMousePositionWorldPos;
        return deltaMousePosition;
    }





}