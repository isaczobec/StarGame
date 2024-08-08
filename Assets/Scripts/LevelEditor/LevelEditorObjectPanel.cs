using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class LevelEditorObjectPanel : MonoBehaviour {


    [SerializeField] private float buttonOffsetWidth = 2f;
    [SerializeField] private float buttonOffsetHeight = 2f;

    [SerializeField] private GameObject objectButtonPrefab;
    [SerializeField] private TextMeshProUGUI text;

    [Header("Visuals")]
    [SerializeField] private Color buttonUnpressedColor = Color.white;
    [SerializeField] private Color buttonPressedColor = Color.green;


    private EditorObjectCategory activeCategory;
    private GameObject activeObject;

    private int maxColumns = 7;

    private List<EditorObjectCategory> editorObjectCategories;

    private Dictionary<UIButton, GameObject> objectButtonPrefabDictionary = new Dictionary<UIButton, GameObject>(); 

    private Dictionary<EditorObjectCategory, GameObject> categoryButtonParentsDictionary = new Dictionary<EditorObjectCategory, GameObject>();

    private UIButton activeButton;
    

    public void InitializeButtons(List<EditorObjectCategory> editorObjectCategories, bool setDefaultActive = true) {
        this.editorObjectCategories = editorObjectCategories;

        for (int i = 0; i < editorObjectCategories.Count; i++) {

            // create a new parent for the category
            GameObject newCategoryButtonParent = new GameObject();
            newCategoryButtonParent.transform.SetParent(transform);
            newCategoryButtonParent.transform.localPosition = Vector3.zero;
            categoryButtonParentsDictionary.Add(editorObjectCategories[i], newCategoryButtonParent);
            
            for (int j = 0; j < editorObjectCategories[i].objectsInCategoryPrefabs.Count; j++) {

                // create the button gameobject and offset it
                GameObject newButton = Instantiate(objectButtonPrefab, newCategoryButtonParent.transform);
                newButton.transform.localPosition = new Vector3(j * buttonOffsetWidth, -i * buttonOffsetHeight, 0f);

                
                // add to dict to track buttons and sub to click event
                UIButton button = newButton.GetComponent<UIButton>();
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
            SetActiveGameObjectThroughButtonClick(objectButtonPrefabDictionary.Keys.ElementAt(0));
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
        }
    }

}