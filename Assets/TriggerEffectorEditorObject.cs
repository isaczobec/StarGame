using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerEffectorEditorObject : LevelEditorObject
{
    [SerializeField] private TMPro.TextMeshPro triggerEffectorIndexText;
    /// <summary>
    /// The index of the trigger effector. This is used to identify the trigger effector in the level editor.
    /// </summary>
    private int index;

    System.Random colorRandom;

    protected override void OnObjectSetup()
    {
        colorRandom = new System.Random();
        index = editorObjectData.GetSetting<int>("Index");
        UpdateIndexText();
    }



    public override void OnSettingChanged<T>(string settingName, T value)
    {
        if (settingName == "Index")
        {
            index = (int)(object)value;
            UpdateIndexText();
        }
    }

    /// <summary>
    /// Updates index text and color
    /// </summary>
    private void UpdateIndexText()
    {
        triggerEffectorIndexText.text = index.ToString();
    }



}
