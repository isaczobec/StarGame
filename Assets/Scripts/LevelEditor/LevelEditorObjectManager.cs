using System;
using System.Collections.Generic;
using UnityEngine;

public class LevelEditorObjectManager : MonoBehaviour {

    [SerializeField] private List<EditorObjectCategory> editorObjectCategories;

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


    public void TryPlaceEditorObject(Vector2 position) {
        if (currentlySelectedObjectToPlace != null) {
            GameObject newObject = Instantiate(currentlySelectedObjectToPlace, position, Quaternion.identity);
            LevelEditorObject levelEditorObject = newObject.GetComponent<LevelEditorObject>();
            if (levelEditorObject != null) {
                newObject.transform.position = new Vector3(position.x + levelEditorObject.offsetWhenPlace.x, position.y + levelEditorObject.offsetWhenPlace.y, 0f);

                levelEditorObjects.Add(levelEditorObject); // start "tracking" this object
            }
        }
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

}

[Serializable]
public class EditorObjectCategory
{
    public string categoryName;
    public List<GameObject> objectsInCategoryPrefabs;
}