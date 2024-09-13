using System;
using System.Collections.Generic;
using UnityEngine;

public class EditorObjectSettingButton : MonoBehaviour {

    /// <summary>
    /// The leveleditorobject which this button is associated with.
    /// </summary>
    private LevelEditorObject levelEditorObject; 

    [SerializeField] protected SettingValueType settingValueType;
    private Vector2 offsetFromObject;
    protected string settingName;

    [SerializeField] private TMPro.TextMeshProUGUI settingNameText;


    private void Start(){
        Setup();
    }

    public void Initialize(LevelEditorObject levelEditorObject, string settingName, Vector2 offsetFromObject) {
        this.levelEditorObject = levelEditorObject;

        // Set text
        this.settingName = settingName;
        settingNameText.text = settingName;
        
        this.offsetFromObject = offsetFromObject;

        levelEditorObject.OnSelectedChanged += OnSelectedChanged;
    }


    private void Update() {
        transform.position = Camera.main.WorldToScreenPoint(levelEditorObject.transform.position) + (Vector3)offsetFromObject;
    }

    protected void SetValue<T>(string setting, T value) {
        levelEditorObject.GetEditorObjectData().SetSetting(setting, settingValueType, value);
    }

    protected T GetValue<T>(string setting) {
        return levelEditorObject.GetEditorObjectData().GetSetting<T>(setting);
    }

    protected float[] GetChangeSettings(string setting) {
        return levelEditorObject.GetEditorObjectData().GetChangeSettings(setting);
    }

    /// <summary>
    /// Overridable method that returns a list of UIButtons that this setting button uses.
    /// </summary>
    /// <returns></returns>
    public virtual List<UIButton> GetUIButtons() {
        return new List<UIButton>();
    }

    /// <summary>
    /// Called when this button is created.
    /// </summary>
    protected virtual void Setup() {

    }

    /// <summary>
    /// Destroy this object when the object is deselected.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="selected"></param>
    private void OnSelectedChanged(object sender, bool selected)
    {
        if (!selected && gameObject != null) {
            levelEditorObject.OnSelectedChanged -= OnSelectedChanged; // remove the event listener
            Destroy(gameObject);
        }
    }
}