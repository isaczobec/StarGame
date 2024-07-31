using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelHandler : MonoBehaviour
{

    [SerializeField] private GameObject[] dontDestroyOnLoadObjects;

    public static LevelHandler Insance { get; private set; }    

    /// <summary>
    /// A list of handlers for active level(s)
    /// </summary>
    private List<LevelWorldObjectsHandler> levelWorldObjectsHandlers = new List<LevelWorldObjectsHandler>();

    public bool isCurrentlyLoadingLevel { get; private set; } = false;
    public bool isCurrentlyReturningToMenu { get; private set; } = false;
    public LevelSO levelToLoad { get; private set; }
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


    public void LoadLevel(string levelName)
    {
        Debug.Log("Loading level: " + levelName);
        UnityEngine.SceneManagement.SceneManager.LoadScene(levelName);
        
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
        if (isCurrentlyLoadingLevel && covered) {
            LoadLevel(levelToLoad.sceneToLoadRefString);
            ScreenCoverer.instance.EndCover(screenUnCoverTime);
            isCurrentlyLoadingLevel = false;
            levelToLoad = null;
            OnLevelLoaded?.Invoke(this, EventArgs.Empty);
            return;
        }

        if (isCurrentlyReturningToMenu && covered) {
            DeleteObjectsInWorldObjectHandlers();
            UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
            Player.Instance.SetPlayerMenuState(PlayerMenuState.mainMenu);
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

        isCurrentlyReturningToMenu = true;
        this.screenUnCoverTime = screenUnCoverTime;
        ScreenCoverer.instance.BeginCover(screenCoverTime);

    }

    private void DeleteObjectsInWorldObjectHandlers() {
        foreach (LevelWorldObjectsHandler handler in levelWorldObjectsHandlers) {
            Destroy(handler.gameObject);
        }
        levelWorldObjectsHandlers.Clear();
    }
}
