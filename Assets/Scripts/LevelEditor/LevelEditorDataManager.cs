using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEditorDataManager : MonoBehaviour
{

    public static LevelEditorDataManager instance {get; private set;}


    public EditorLevelData editorLevelData {get; private set;} = new EditorLevelData();

    private void Awake() {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
