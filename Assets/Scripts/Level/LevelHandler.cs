using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelHandler : MonoBehaviour
{

    [SerializeField] private GameObject[] dontDestroyOnLoadObjects;
    [SerializeField] private GameObject menuSpawnPointPrefab;

    public static LevelHandler Insance { get; private set; }    

    /// <summary>
    /// A list of handlers for active level(s)
    /// </summary>
    private List<LevelWorldObjectsHandler> levelWorldObjectsHandlers = new List<LevelWorldObjectsHandler>();

    public bool isCurrentlyLoadingLevel { get; private set; } = false;
    public bool isCurrentlyReturningToMenu { get; private set; } = false;
    public LevelSO levelToLoad { get; private set; }
    public LevelSO currentLevel { get; private set; }
    private float screenUnCoverTime = 0.3f;


    public event EventHandler OnLevelLoaded;
    public event EventHandler OnReturnToMenu;



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

        ScreenCoverer.instance.OnCoverComplete += OnCoverComplete;
    }


    public void LoadLevel(LevelSO levelSO)
    {
        Debug.Log("Loading level: " + levelSO.name);
        SceneManager.LoadScene(levelSO.sceneToLoadRefString);
        
    }

    public void UnloadLevel(LevelSO levelSO)
    {
        Debug.Log("Unloading level: " + levelSO.name);
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
        SceneManager.UnloadSceneAsync(levelSO.sceneToLoadRefString);
    }

    public void LoadLevelScreenCovered(LevelSO levelToLoad, float screenCoverTime, float screenUnCoverTime)
    {
        if (isCurrentlyLoadingLevel)
        {
            Debug.LogWarning("Already loading a level");
            return;
        }

        this.levelToLoad = levelToLoad;
        isCurrentlyLoadingLevel = true;
        this.screenUnCoverTime = screenUnCoverTime;
        ScreenCoverer.instance.BeginCover(screenCoverTime);

    }

    private void OnCoverComplete(object sender, bool covered)
    {
        // loading a level
        if (isCurrentlyLoadingLevel && covered) {
            LoadLevel(levelToLoad);
            ScreenCoverer.instance.EndCover(screenUnCoverTime);
            isCurrentlyLoadingLevel = false;
            currentLevel = levelToLoad;
            levelToLoad = null;
            OnLevelLoaded?.Invoke(this, EventArgs.Empty);
            return;
        }

        // returning to menu
        if (isCurrentlyReturningToMenu && covered) {
            UnloadLevel(currentLevel);
            currentLevel = null;
            Instantiate(menuSpawnPointPrefab, Vector3.zero, Quaternion.identity, transform); // create a new spawn point for the menu
            ScreenCoverer.instance.EndCover(screenUnCoverTime);
            isCurrentlyReturningToMenu = false;
            OnReturnToMenu?.Invoke(this, EventArgs.Empty);
            return;
        }
    }

    public void AddLevelWorldObjectsHandler(LevelWorldObjectsHandler levelWorldObjectsHandler)
    {
        levelWorldObjectsHandlers.Add(levelWorldObjectsHandler);
    }



    public void ExitToMainMenuScreenCovered(float screenCoverTime, float screenUnCoverTime) {
        if (isCurrentlyReturningToMenu)
        {
            Debug.LogWarning("Already returning");
            return;
        }
    
        if (currentLevel == null) {
            Debug.LogWarning("No current level to return from");
            return;
        }


        isCurrentlyReturningToMenu = true;
        this.screenUnCoverTime = screenUnCoverTime;
        ScreenCoverer.instance.BeginCover(screenCoverTime);

    }

}
