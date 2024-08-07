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
    public SettingValueType valueType;
}


public enum SettingValueType {
    INT,
    FLOAT,
    VECTOR2,
    BOOL,
}