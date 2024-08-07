using System.Collections.Generic;
using UnityEngine;

public class LevelEditorObjectManager : MonoBehaviour {

    [SerializeField] private GameObject[] LevelEditorObjectPrefabs;
    private GameObject currentlySelectedObjectToPlace;



    private List<LevelEditorObject> levelEditorObjects = new List<LevelEditorObject>();

    public static LevelEditorObjectManager instance {get; private set;}

    private void Awake() {
        if (instance == null) {
            instance = this;
        } else {
            Debug.LogError("There is already an instance of LevelEditorObjectManager in the scene.");
        }
    }

    private void Start() {
        currentlySelectedObjectToPlace = LevelEditorObjectPrefabs[0];
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

}