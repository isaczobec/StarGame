using UnityEngine;

public class TileColorChangeEditorObject : LevelEditorObject {

    private const string colorRef = "Color";

    [SerializeField] private SpriteRenderer colorSpriteRenderer;

    protected override void OnObjectSetup() {
        SetImageColor(editorObjectData.GetSetting<int>(colorRef));
    }   

    public override void OnSettingChanged<T>(string settingName, T value) {
        if (value.GetType() == typeof(int) && settingName == colorRef) {
            SetImageColor((int)(object)value);
        }
    }

    private void SetImageColor(int colorInt) {
        colorSpriteRenderer.color = ColorTransform.IntToColor(colorInt);
    }
}
