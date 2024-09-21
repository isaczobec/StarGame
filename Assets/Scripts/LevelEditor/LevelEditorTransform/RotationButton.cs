


using System.Collections.Generic;
using UnityEngine;

public class RotationButton : LevelEditorTransformButton
{

    [SerializeField] private float maxBoundsRadiusMultiplier = 1.2f;


    /// <summary>
    /// current rotation in degrees.
    /// </summary>
    private float currentRotation = 0;

    
    

    public override void FrameUpdateWhilePressed(TransformButtonInfo transformButtonInfo)
    {

        Vector2 deltaMousePosition = GetMouseDeltaWorldPosition();
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector2 axis1 = mousePosition - transformButtonInfo.averagePosition;
        Vector2 axis2 = axis1 - deltaMousePosition;

        float angle = Vector2.SignedAngle(axis1, axis2);
        

        foreach (LevelEditorObject selectedObject in transformButtonInfo.selectedObjects)
        {
            // rotate object around the average position of the selected objects
            Vector2 radius = (Vector2)selectedObject.transform.position - transformButtonInfo.averagePosition;
            Vector2 rotatedRadius = Quaternion.Euler(0, 0, -angle) * radius;
            Vector2 posDiff = rotatedRadius - radius;
            selectedObject.AddPosition(posDiff);

            // rotate the object around its own center
            selectedObject.AddRotation(-angle);

        }

        currentRotation -= angle;
        SetScreenPosition(GetPositionOffset(transformButtonInfo),true);

        SetLastMousePosition();

    }

    public override void FrameUpdateWhileUnpressed(TransformButtonInfo transformButtonInfo)
    {
        SetScreenPosition(GetPositionOffset(transformButtonInfo), true);

        SetLastMousePosition();
    }

    public override void InitializeButtonInSubClass(TransformButtonInfo transformButtonInfo, int index)
    {
        currentRotation = 180; // start on  left of object

        SetScreenPosition(GetPositionOffset(transformButtonInfo), true);

        SetLastMousePosition();

    }

    private Vector2 GetPositionOffset(TransformButtonInfo transformButtonInfo) {
        float maxBoundsRadius = transformButtonInfo.GetMaxBoundsRadius();
        float x = Mathf.Cos(currentRotation * Mathf.Deg2Rad);
        float y = Mathf.Sin(currentRotation * Mathf.Deg2Rad);
        Vector2 pos = new Vector2(x, y) * maxBoundsRadius * maxBoundsRadiusMultiplier;



        return pos;
    }

    

    
}