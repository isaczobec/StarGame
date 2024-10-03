using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;

public class BackgroundColorChangeEditorObject : LevelEditorObject {

    private const string color1Ref = "Color 1";
    private const string color2Ref = "Color 2";

    [SerializeField] private SpriteRenderer color1SpriteRenderer;
    [SerializeField] private SpriteRenderer color2SpriteRenderer;

    protected override void OnObjectSetup() {
        SetImageColor(1, editorObjectData.GetSetting<int>(color1Ref));
        SetImageColor(2, editorObjectData.GetSetting<int>(color2Ref));
    }   



    public override void OnSettingChanged<T>(string settingName, T value) {
        if (value.GetType() == typeof(int)) {
            switch (settingName) {
                case color1Ref:
                    SetImageColor(1, (int)(object)value);
                    break;
                case color2Ref:
                    SetImageColor(2, (int)(object)value);
                    break;
            }
        }
    }

    private void SetImageColor(int colorIndex, int colorInt) {
        switch (colorIndex) {
            case 1:
                color1SpriteRenderer.color = ColorTransform.IntToColor(colorInt);
                break;
            case 2:
                color2SpriteRenderer.color = ColorTransform.IntToColor(colorInt);
                break;
        }

    }



}