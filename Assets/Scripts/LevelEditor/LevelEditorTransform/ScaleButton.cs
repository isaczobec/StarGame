


using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class ScaleButton : LevelEditorTransformButton
{


    
    /// <summary>
    /// The index of the corner of this scalebutton. 0 = lower left, 1 = upper right, 2 = lower right, 3 = upper left
    /// </summary>
    private int cornerIndex = 0;
    

    public override void FrameUpdateWhilePressed(TransformButtonInfo transformButtonInfo)
    {
        SetScreenPositionFromCornerIndex(transformButtonInfo);
        Vector2 deltaMousePosition = GetMouseDeltaWorldPosition();

        Vector2 newUpperRightCorner = new Vector2();
        Vector2 newLowerLeftCorner = new Vector2();

        // calculate new bounds
        if (cornerIndex == 0) {
            newLowerLeftCorner = transformButtonInfo.lowerLeftCornerBounds + deltaMousePosition;
            newUpperRightCorner = transformButtonInfo.upperRightCornerBounds;
        } else if (cornerIndex == 1) {
            newLowerLeftCorner = transformButtonInfo.lowerLeftCornerBounds;
            newUpperRightCorner = transformButtonInfo.upperRightCornerBounds + deltaMousePosition;
        } else if (cornerIndex == 2) {
            newLowerLeftCorner = new Vector2(transformButtonInfo.lowerLeftCornerBounds.x, transformButtonInfo.lowerLeftCornerBounds.y + deltaMousePosition.y);
            newUpperRightCorner = new Vector2(transformButtonInfo.upperRightCornerBounds.x + deltaMousePosition.x, transformButtonInfo.upperRightCornerBounds.y);
        } else if (cornerIndex == 3) {
            newLowerLeftCorner = new Vector2(transformButtonInfo.lowerLeftCornerBounds.x + deltaMousePosition.x, transformButtonInfo.lowerLeftCornerBounds.y);
            newUpperRightCorner = new Vector2(transformButtonInfo.upperRightCornerBounds.x, transformButtonInfo.upperRightCornerBounds.y + deltaMousePosition.y);
        }

        foreach (LevelEditorObject selectedObject in transformButtonInfo.selectedObjects)
        {
        
            // change position
            Vector2 objectPosition = (Vector2)selectedObject.transform.position;
            float xBoundsPercentage = (transformButtonInfo.selectedObjects.Count>1)? Mathf.InverseLerp(transformButtonInfo.lowerLeftCornerBounds.x, transformButtonInfo.upperRightCornerBounds.x, objectPosition.x) : 0.5f;
            float yBoundsPercentage = (transformButtonInfo.selectedObjects.Count>1)? Mathf.InverseLerp(transformButtonInfo.lowerLeftCornerBounds.y, transformButtonInfo.upperRightCornerBounds.y, objectPosition.y) : 0.5f;

            float newX = Mathf.Lerp(newLowerLeftCorner.x, newUpperRightCorner.x, xBoundsPercentage);
            float newY = Mathf.Lerp(newLowerLeftCorner.y, newUpperRightCorner.y, yBoundsPercentage);

            selectedObject.transform.position = new Vector3(newX, newY, selectedObject.transform.position.z);

            // change scale
            float xScaleFactor = (newUpperRightCorner.x - newLowerLeftCorner.x) / (transformButtonInfo.upperRightCornerBounds.x - transformButtonInfo.lowerLeftCornerBounds.x);
            float yScaleFactor = (newUpperRightCorner.y - newLowerLeftCorner.y) / (transformButtonInfo.upperRightCornerBounds.y - transformButtonInfo.lowerLeftCornerBounds.y);

            // account for object's rotation

            // float angle = -selectedObject.transform.rotation.z * Mathf.Deg2Rad;
            // Vector2 rotatedScale = new Vector2(
            //     xScaleFactor * math.cos(angle) - yScaleFactor * math.sin(angle), 
            //     xScaleFactor * math.sin(angle) + yScaleFactor * math.cos(angle)
            // );
            

            Vector2 newScale = new Vector2(selectedObject.transform.localScale.x * xScaleFactor, selectedObject.transform.localScale.y * yScaleFactor);
            Vector2 clampedScale = new Vector2(Mathf.Clamp(newScale.x, selectedObject.minScale.x, selectedObject.maxScale.x), Mathf.Clamp(newScale.y, selectedObject.minScale.y, selectedObject.maxScale.y));
            selectedObject.transform.localScale = new Vector3(clampedScale.x, clampedScale.y, selectedObject.transform.localScale.z);

        }

        SetLastMousePosition();

    }

    public override void FrameUpdateWhileUnpressed(TransformButtonInfo transformButtonInfo)
    {
        SetScreenPositionFromCornerIndex(transformButtonInfo);

        SetLastMousePosition();
    }

    public override void InitializeButtonInSubClass(TransformButtonInfo transformButtonInfo, int index)
    {
        cornerIndex = index % 4;
        SetScreenPositionFromCornerIndex(transformButtonInfo);

        SetLastMousePosition();

    }

    private void SetScreenPositionFromCornerIndex(TransformButtonInfo transformButtonInfo) {
        Vector2 cornerPosition = new Vector2(0, 0);
        if (cornerIndex == 0) {
            cornerPosition = (Vector2)Camera.main.WorldToScreenPoint(transformButtonInfo.lowerLeftCornerBounds);
        } else if (cornerIndex == 1) {
            cornerPosition = (Vector2)Camera.main.WorldToScreenPoint(transformButtonInfo.upperRightCornerBounds);
        } else if (cornerIndex == 2) {
            cornerPosition = (Vector2)Camera.main.WorldToScreenPoint(new Vector2(transformButtonInfo.upperRightCornerBounds.x, transformButtonInfo.lowerLeftCornerBounds.y));
        } else if (cornerIndex == 3) {
            cornerPosition = (Vector2)Camera.main.WorldToScreenPoint(new Vector2(transformButtonInfo.lowerLeftCornerBounds.x, transformButtonInfo.upperRightCornerBounds.y));
        }


        Vector2 positionOffset = cornerPosition - (Vector2)Camera.main.WorldToScreenPoint(transformButtonInfo.averagePosition);
        Debug.Log("positionOffset: " +positionOffset);

        SetScreenPosition(positionOffset);
    }

    

    
}