using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


/// <summary>
/// Singleton class that allows for changing the settings of an object in the editor.
/// Spawns UI objects when an object is selected corresponding to its settings.
/// </summary>
public class EditorObjectSettingChanger : MonoBehaviour
{
   
    public static EditorObjectSettingChanger Instance { get; private set;}

    [Header("Setting Button Prefabs")]
    [SerializeField] private GameObject floatSettingButtonPrefab;
    [SerializeField] private GameObject intSettingButtonPrefab;
    [SerializeField] private GameObject vector2SettingButtonPrefab;
    [SerializeField] private GameObject boolSettingButtonPrefab;

    [Header("settings")]
    [SerializeField] private Vector2 baseOffsetFromObject;
    [SerializeField] private Vector2 offsetBetweenButtons;


    private Dictionary<SettingValueType,GameObject> settingButtonPrefabsDict = new Dictionary<SettingValueType, GameObject>();

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        } else {
            Debug.LogError("There are multiple EditorObjectSettingChanger instances in the scene.");
            Destroy(gameObject);
        }
    }

    private void Start() {
        // add the prefabs to the dictionary
        settingButtonPrefabsDict.Add(SettingValueType.FLOAT, floatSettingButtonPrefab);
        settingButtonPrefabsDict.Add(SettingValueType.INT, intSettingButtonPrefab);
        settingButtonPrefabsDict.Add(SettingValueType.VECTOR2, vector2SettingButtonPrefab);
        settingButtonPrefabsDict.Add(SettingValueType.BOOL, boolSettingButtonPrefab);
    
        LevelEditorObjectManager.instance.OnLevelEditorObjectsSelected += OnLevelEditorObjectsSelected;
    }

    private void OnLevelEditorObjectsSelected(object sender, List<LevelEditorObject> newlySelectedObjects)
    {
        // loop over all the newly selected objects and create the setting buttons for them
        for (int i = 0; i < newlySelectedObjects.Count; i++) {

            LevelEditorObject levelEditorObject = newlySelectedObjects[i];
            for (int j = 0; j < levelEditorObject.GetEditorObjectData().settings.Count; j++) {

                // create and init the button
                EditorObjectSetting setting = levelEditorObject.GetEditorObjectData().settings[j];
                GameObject settingButtonPrefab = settingButtonPrefabsDict[setting.valueType];
                GameObject settingButton = Instantiate(settingButtonPrefab, transform);
                EditorObjectSettingButton editorObjectSettingButton = settingButton.GetComponent<EditorObjectSettingButton>();
                editorObjectSettingButton.Initialize(levelEditorObject, setting.settingName, baseOffsetFromObject + offsetBetweenButtons * j);

                foreach (UIButton uiButton in editorObjectSettingButton.GetUIButtons()) {
                    EditorBuildingManager.instance.AddEditorUIButton(uiButton);
                }
            }
        }
    }
}