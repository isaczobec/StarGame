


using System.Collections.Generic;
using UnityEngine;

public class PositionButton : LevelEditorTransformButton
{

    [SerializeField] private Vector2 positionOffset = new Vector2(0, -3f);

    
    

    public override void FrameUpdateWhilePressed(TransformButtonInfo transformButtonInfo)
    {
        SetScreenPosition(positionOffset);
        Vector2 deltaMousePosition = GetMouseDeltaWorldPosition();
        foreach (LevelEditorObject selectedObject in transformButtonInfo.selectedObjects)
        {


            selectedObject.AddPosition(deltaMousePosition);
        }

        SetLastMousePosition();

    }

    public override void FrameUpdateWhileUnpressed(TransformButtonInfo transformButtonInfo)
    {
        SetScreenPosition(positionOffset);

        SetLastMousePosition();
    }

    public override void InitializeButtonInSubClass(TransformButtonInfo transformButtonInfo, int index)
    {
        SetScreenPosition(positionOffset);

        SetLastMousePosition();

    }

    

    
}