using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class LevelEditorObjectManager : MonoBehaviour {

    [SerializeField] private List<EditorObjectCategory> editorObjectCategories;


    // selected objects

    private List<LevelEditorObject> selectedEditorObjects = new List<LevelEditorObject>();
    public List<LevelEditorObject> GetSelectedEditorObjects() {
        return selectedEditorObjects;
    }
    private List<LevelEditorObject> hoveredEditorObjects = new List<LevelEditorObject>();
    [SerializeField] private string levelEditorObjectTag = "levelEditorObject";

    // events

    /// <summary>
    /// Called when objects are selected. The LevelEditorObjects are passed in a list.
    /// </summary>
    public event EventHandler<List<LevelEditorObject>> OnLevelEditorObjectsSelected;

    /// <summary>
    /// Called when objects are deselected. The LevelEditorObjects are passed in a list.
    /// </summary>
    public event EventHandler<List<LevelEditorObject>> OnLevelEditorObjectsDeselected;


    public List<EditorObjectCategory> GetEditorObjectCategories() {
        return editorObjectCategories;
    }
    public GameObject[] GetLevelEditorObjectPrefabs() {
        List<GameObject> prefabs = new List<GameObject>();
        foreach (EditorObjectCategory category in editorObjectCategories) {
            foreach (GameObject prefab in category.objectsInCategoryPrefabs) {
                prefabs.Add(prefab);
            }
        }
        return prefabs.ToArray();
    }
    private GameObject currentlySelectedObjectToPlace;

    public void SetCurrentlySelectedObjectToPlace(GameObject objectToPlace) {
        currentlySelectedObjectToPlace = objectToPlace;
    }



    private List<LevelEditorObject> levelEditorObjects = new List<LevelEditorObject>();

    public static LevelEditorObjectManager instance {get; private set;}

    private void Awake() {
        if (instance == null) {
            instance = this;
        } else {
            Debug.LogError("There is already an instance of LevelEditorObjectManager in the scene.");
        }
    }

    private void Update() {
        HandleHoverObjects();
    }

    /// <summary>
    /// Tries to place an editor object at the given position. Returns the object if it was placed, otherwise null.
    /// </summary>
    /// <param name="position"></param>
    /// <param name="setSelectedByDefault"></param>
    /// <returns></returns>
    public LevelEditorObject TryPlaceEditorObject(Vector2 position, bool setSelectedByDefault = true, bool deselectOtherObjects = false) {
        if (currentlySelectedObjectToPlace != null) {
            GameObject newObject = Instantiate(currentlySelectedObjectToPlace, position, Quaternion.identity);
            LevelEditorObject levelEditorObject = newObject.GetComponent<LevelEditorObject>();
            if (levelEditorObject != null) {
                newObject.transform.position = new Vector3(position.x + levelEditorObject.offsetWhenPlace.x, position.y + levelEditorObject.offsetWhenPlace.y, 0f);

                levelEditorObjects.Add(levelEditorObject); // start "tracking" this object
                levelEditorObject.Initialize();
            }

            if (deselectOtherObjects) DeSelectAllObjects();
            if (setSelectedByDefault) {
            // select object
                selectedEditorObjects.Add(levelEditorObject);
                levelEditorObject.SetSelected(true);
            }

            return levelEditorObject;
        }
        return null;
    }

    public List<EditorObjectData> UpdateAndGetEditorObjectDatas() {
        List<EditorObjectData> editorObjectDatas = new List<EditorObjectData>();
        foreach (LevelEditorObject levelEditorObject in levelEditorObjects) {
            editorObjectDatas.Add(levelEditorObject.UpdateAndGetEditorObjectData());
        }
        return editorObjectDatas;
    }

    public void LoadLevelEditorObjectsIntoEditor(List<EditorObjectData> objectDatas) {
        foreach (EditorObjectData objectData in objectDatas) {
            foreach (GameObject prefab in GetLevelEditorObjectPrefabs()) {
                LevelEditorObject levelEditorObject = prefab.GetComponent<LevelEditorObject>();
                if (levelEditorObject != null && levelEditorObject.GetObjectID() == objectData.SpawnnableObjectID) {

                    // spawn the object
                    // set the base settings
                    GameObject newObject = Instantiate(prefab, objectData.position, Quaternion.identity);
                    newObject.transform.rotation = Quaternion.Euler(0f, 0f, objectData.rotation);
                    newObject.transform.localScale = new Vector3(objectData.scale.x, objectData.scale.y, 1f);

                    // set editorObject data and add to list
                    LevelEditorObject newLevelEditorObject = newObject.GetComponent<LevelEditorObject>();
                    if (newLevelEditorObject != null) {
                        newLevelEditorObject.SetEditorObjectData(objectData);
                        levelEditorObjects.Add(newLevelEditorObject);
                    }

                    
                }
            }
        }
    }

    /// <summary>
    /// Update the hovered objects and handle the hovering of objects.
    /// </summary>
    private void HandleHoverObjects() {
        List<LevelEditorObject> hoveredLevelEditorObjects = GetHoveredLevelEditorObjects();

        // unhover unhovered objectsd
        foreach (LevelEditorObject obj in hoveredEditorObjects) {
            if (!hoveredLevelEditorObjects.Contains(obj)) {
                obj.SetHovered(false);
            }
        }

        // hover hovered objects
        foreach (LevelEditorObject obj in hoveredLevelEditorObjects) {
            obj.SetHovered(true);
        }
        hoveredEditorObjects = hoveredLevelEditorObjects;
    }

    /// <summary>
    /// Gets all currently hovered level editor objects. Objects must have the tag leveleditorobject tag.
    /// </summary>
    /// <returns></returns>
    private List<LevelEditorObject> GetHoveredLevelEditorObjects() {
        List<LevelEditorObject> hoveredLevelEditorObjects = new List<LevelEditorObject>();
        RaycastHit2D[] hits = Physics2D.RaycastAll(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        // Debug.Log(hits.Length);
        foreach (RaycastHit2D hit in hits) {
            if (hit.transform.CompareTag(levelEditorObjectTag)) {
                LevelEditorObject levelEditorObject = hit.transform.GetComponent<LevelEditorObject>();
                hoveredLevelEditorObjects.Add(levelEditorObject);
            }
        }
        return hoveredLevelEditorObjects;
    }

    /// <summary>
    /// Adds the hovered objects to the selected objects.
    /// </summary>
    public void AddHoveredObjectsToSelected() {
        
        List<LevelEditorObject> newSelectedObjects = new List<LevelEditorObject>(); // keep track of new objects

        foreach (LevelEditorObject obj in hoveredEditorObjects) {
            Debug.Log("Adding object to selected");
            if (!selectedEditorObjects.Contains(obj)) {
                selectedEditorObjects.Add(obj);
                obj.SetSelected(true);
                newSelectedObjects.Add(obj);
            }
        }

        if (newSelectedObjects.Count > 0) OnLevelEditorObjectsSelected?.Invoke(this, newSelectedObjects);
    }

    /// <summary>
    /// Deselects an object from the selected objects.
    /// </summary>
    /// <param name="obj"></param>
    public void DeSelectObject(LevelEditorObject obj) {
        selectedEditorObjects.Remove(obj);
        obj.SetSelected(false);

        // invoke event
        List<LevelEditorObject> deselectedObjects = new List<LevelEditorObject>();
        deselectedObjects.Add(obj);
        OnLevelEditorObjectsDeselected?.Invoke(this, deselectedObjects);
    }

    /// <summary>
    /// Deselects all objects.
    /// </summary>
    public void DeSelectAllObjects() {
        foreach (LevelEditorObject obj in selectedEditorObjects) {
            obj.SetSelected(false);
        }

        OnLevelEditorObjectsDeselected?.Invoke(this, selectedEditorObjects); // invoke event

        selectedEditorObjects.Clear();
    }

    /// <summary>
    /// Deletes, removes and destroys the object from the level editor.
    /// </summary>
    /// <param name="levelEditorObject"></param>
    public void DeleteObject(LevelEditorObject levelEditorObject) {
        levelEditorObjects.Remove(levelEditorObject);
        Destroy(levelEditorObject.gameObject);
    }

    /// <summary>
    /// Deletes all hovered objects.
    /// </summary>
    public void DeleteHoveredObjects() {
        foreach (LevelEditorObject obj in hoveredEditorObjects) {
            DeleteObject(obj);
        }
        hoveredEditorObjects.Clear();
    }

    public void DeSelectCurrentObjectToPlace() {
        currentlySelectedObjectToPlace = null;
        LevelEditorObjectPanel.instance.DeSelectObjectButton();
    }

}

[Serializable]
public class EditorObjectCategory
{
    public string categoryName;
    public List<GameObject> objectsInCategoryPrefabs;
}