using UnityEngine;

public class GeneralEditorPrefabs : MonoBehaviour
{
 
    public static GeneralEditorPrefabs instance { get; private set; }

    [SerializeField] private GameObject editorObjectNodePrefab;

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogError("There is already an instance of GeneralEditorPrefabs in the scene.");
        }
    }

    public GameObject GetEditorObjectNodePrefab()
    {
        return editorObjectNodePrefab;
    }

}