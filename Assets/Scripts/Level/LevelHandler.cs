using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelHandler : MonoBehaviour
{

    [SerializeField] private GameObject[] dontDestroyOnLoadObjects;

    public static LevelHandler Insance { get; private set; }    

    private void Awake()
    {
        Insance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        foreach (GameObject obj in dontDestroyOnLoadObjects)
        {
            DontDestroyOnLoad(obj);
        }
    }

    public void LoadLevel(string levelName)
    {
        Debug.Log("Loading level: " + levelName);
        UnityEngine.SceneManagement.SceneManager.LoadScene(levelName);
        
    }

}
