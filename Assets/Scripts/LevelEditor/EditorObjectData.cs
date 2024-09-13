using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EditorObjectData {

    /// <summary>
    /// An ID used to identify which prefab this object should spawn.
    /// </summary>
    public string SpawnnableObjectID;

    /// <summary>
    /// the position in the world of this object
    /// </summary>
    public Vector2 position;
    /// <summary>
    /// Z rotation of this object.
    /// </summary>
    public float rotation;
    /// <summary>
    /// X and Y scale of this object.
    /// </summary>
    public Vector2 scale;

    /// <summary>
    /// THEESE SETTINGS GO OFF STRINGS SO MAKE SURE TO GET THEM RIGHT
    /// </summary>
    public List<EditorObjectSetting> settings = new List<EditorObjectSetting>();

    /// <summary>
    /// A list of all the editor object nodes that this object has.
    /// </summary>
    public List<EditorObjectNodeData> editorObjectNodes = new List<EditorObjectNodeData>();

    /// <summary>
    /// Sets a setting value for this object.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="settingName"></param>
    /// <param name="value"></param>
    public void SetSetting<T>(string settingName, SettingValueType settingValueType, T value) {

        foreach (EditorObjectSetting setting in settings) {
            if (setting.settingName == settingName) {
                if (setting.valueType != settingValueType) {
                    throw new Exception("Setting value type mismatch.");
                }
                string s = value.ToString();
                setting.value = s;
                return;
            }
        }
    }

    public T GetSetting<T>(string settingName) {
        foreach (EditorObjectSetting setting in settings) {
            if (setting.settingName == settingName) {
                return (T)Convert.ChangeType(setting.value, typeof(T));
            }
        }
        return default;
    }

    /// <summary>
    /// Returns the [min, max, increment] for a setting.
    /// </summary>
    /// <param name="settingName"></param>
    /// <returns></returns>
    public float[] GetChangeSettings(string settingName) {
        foreach (EditorObjectSetting setting in settings) {
            if (setting.settingName == settingName) {
                return new float[] { setting.min, setting.max, setting.increment };
        }
    }
    return new float[] { 0, 0 , 0};
    }

    /// <summary>
    /// Sets the position, rotation and scale of this object.
    /// </summary>
    /// <param name="levelEditorObject"></param>
    public void SetBaseSettings(LevelEditorObject levelEditorObject) {
        position = levelEditorObject.transform.position;
        rotation = levelEditorObject.transform.rotation.eulerAngles.z;
        scale = levelEditorObject.transform.localScale;
    }

}

[Serializable]
public class EditorObjectSetting {
    public string settingName;
    public string value;
    /// <summary>
    /// The amount this setting can be increased or decreased by, given that it is a float, int ocr vector.
    /// </summary>
    public float increment = 1;
    public float min = 0;
    public float max = 100;
    public SettingValueType valueType;
}


public enum SettingValueType {
    INT,
    FLOAT,
    VECTOR2,
    BOOL,
}