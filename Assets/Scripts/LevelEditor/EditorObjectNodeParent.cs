using UnityEngine;

public class EditorObjectNodeParent : MonoBehaviour
{
    public static EditorObjectNodeParent instance { get; private set; }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogError("There is already an instance of EditorObjectNodeParent in the scene.");
        }
    }

    
}