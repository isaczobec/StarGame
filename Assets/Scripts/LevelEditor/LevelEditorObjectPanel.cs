using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LevelEditorObjectPanel : MonoBehaviour {


    [SerializeField] private float buttonOffsetWidth = 2f;
    [SerializeField] private float buttonOffsetHeight = 2f;
    [SerializeField] private Vector2 buttonOffset = new Vector2(-50f, 50f);

    [SerializeField] private GameObject objectButtonPrefab;
    [SerializeField] private TextMeshProUGUI text;

    [SerializeField] private UIButton switchCategoryLeftButton;
    [SerializeField] private UIButton switchCategoryRightButton;

    [Header("Visuals")]
    [SerializeField] private Color buttonUnpressedColor = Color.white;
    [SerializeField] private Color buttonPressedColor = Color.green;


    private EditorObjectCategory activeCategory;
    private GameObject activeObject;

    private int maxColumns = 8;

    private List<EditorObjectCategory> editorObjectCategories;
    private int currentCategoryIndex = 0;

    private Dictionary<UIButton, GameObject> objectButtonPrefabDictionary = new Dictionary<UIButton, GameObject>(); 

    private Dictionary<EditorObjectCategory, GameObject> categoryButtonParentsDictionary = new Dictionary<EditorObjectCategory, GameObject>();

    private UIButton activeButton;

    public static LevelEditorObjectPanel instance {get; private set;}

    private void Awake() {
        if (instance == null) {
            instance = this;
        } else {
            Debug.LogError("There is already an instance of LevelEditorObjectPanel in the scene.");
        }
    }

    private void Start() {
        switchCategoryLeftButton.OnUIButtonClicked += OnSwitchCategoryLeftButtonClicked;
        switchCategoryRightButton.OnUIButtonClicked += OnSwitchCategoryRightButtonClicked;

        EditorBuildingManager.instance.AddEditorUIButton(switchCategoryLeftButton);
        EditorBuildingManager.instance.AddEditorUIButton(switchCategoryRightButton);
    }


    public void InitializeButtons(List<EditorObjectCategory> editorObjectCategories, bool setDefaultActive = true) {
        this.editorObjectCategories = editorObjectCategories;

        for (int i = 0; i < editorObjectCategories.Count; i++) {

            // create a new parent for the category
            GameObject newCategoryButtonParent = new GameObject();
            newCategoryButtonParent.transform.SetParent(transform);
            newCategoryButtonParent.transform.localPosition = buttonOffset;
            categoryButtonParentsDictionary.Add(editorObjectCategories[i], newCategoryButtonParent);
            
            for (int j = 0; j < editorObjectCategories[i].objectsInCategoryPrefabs.Count; j++) {

                // create the button gameobject and offset it
                GameObject newButton = Instantiate(objectButtonPrefab, newCategoryButtonParent.transform);
                int column = j % maxColumns;
                int row = j / maxColumns;
                newButton.transform.localPosition = new Vector3(column * buttonOffsetWidth, row * buttonOffsetHeight, 0f);

                
                // add to dict to track buttons and sub to click event
                UIButtonAnimated button = newButton.GetComponent<UIButtonAnimated>();
                objectButtonPrefabDictionary.Add(button, editorObjectCategories[i].objectsInCategoryPrefabs[j]);
                button.OnUIButtonClicked += OnObjectButtonClicked;

                // set visuals
                button.SetIcon(editorObjectCategories[i].objectsInCategoryPrefabs[j].GetComponent<LevelEditorObject>()?.GetSprite());


                // add the object to the buildingmanager
                EditorBuildingManager.instance.AddEditorUIButton(button);
            }
        }

        // set the first category active
        if (setDefaultActive) {
            SetActiveCategory(editorObjectCategories[currentCategoryIndex],true);
            SetActiveGameObjectThroughButtonClick(objectButtonPrefabDictionary.Keys.ElementAt(0));
        }

    }


    public void SetActiveCategory(EditorObjectCategory category, bool SetAllOthersInactive = false) {
        if (categoryButtonParentsDictionary.ContainsKey(category)) {
            foreach (KeyValuePair<EditorObjectCategory, GameObject> pair in categoryButtonParentsDictionary) {
                if (pair.Key == category) {
                    foreach (Transform child in pair.Value.transform) {
                        UIButtonAnimated button = child.GetComponent<UIButtonAnimated>();
                        button.showByDefault = true;
                        button.ChangeVisible(true);
                    }
                    continue;
                } 
                if (pair.Key == activeCategory || (SetAllOthersInactive && pair.Key != category)) {
                    Debug.Log("Setting inactive");
                    foreach (Transform child in pair.Value.transform) {
                        UIButtonAnimated button = child.GetComponent<UIButtonAnimated>();
                        button.ChangeVisible(false);
                    }
                    continue;
                }
            }
            activeCategory = category;
            text.text = category.categoryName;
        }
    }

    private void OnObjectButtonClicked(object sender, EventArgs e)
    {
        SetActiveGameObjectThroughButtonClick((UIButton)sender);
    }

    private void SetActiveGameObjectThroughButtonClick(UIButton clickedButton) {
        if (objectButtonPrefabDictionary.ContainsKey(clickedButton)) {

            // find and set the active object
            GameObject prefab = objectButtonPrefabDictionary[clickedButton];
            activeObject = prefab;

            // send the object to the editor building manager
            LevelEditorObjectManager.instance.SetCurrentlySelectedObjectToPlace(activeObject);

            // update visuals
            if (activeButton != null) {
                activeButton.SetColor(buttonUnpressedColor);
            }
            activeButton = clickedButton; // set the new active button
            activeButton.SetColor(buttonPressedColor);

            // deselect others
            TileArrayManager.instance.DeSelectTileArray();
            EditorBuildingManager.instance.EnableBuildingMode();
        }
    }
    private void OnSwitchCategoryRightButtonClicked(object sender, EventArgs e)
    {
        currentCategoryIndex++;
        if (currentCategoryIndex >= editorObjectCategories.Count) {
            currentCategoryIndex = 0;
        }
        SetActiveCategory(editorObjectCategories[currentCategoryIndex]);
    }

    private void OnSwitchCategoryLeftButtonClicked(object sender, EventArgs e)
    {
        currentCategoryIndex--;
        if (currentCategoryIndex < 0) {
            currentCategoryIndex = editorObjectCategories.Count - 1;
        }
        SetActiveCategory(editorObjectCategories[currentCategoryIndex]);
    }

    public void DeSelectObjectButton() {
        if (activeButton != null) {
            activeButton.SetColor(buttonUnpressedColor);
        }
        activeButton = null;
        activeObject = null;
    }

}