using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
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


    /// <summary>
    /// The level data manager, used for saving and loading level data
    /// </summary>
    public LevelDataManager levelDataManager {get; private set;}

    public event EventHandler OnLevelLoaded;
    public event EventHandler OnReturnToMenu;



    private void Awake()
    {
        Insance = this;
        levelDataManager = new LevelDataManager();
    }

    // Start is called before the first frame update
    void Start()
    {
        // make sure objects are not destroyed on load
        foreach (GameObject obj in dontDestroyOnLoadObjects)
        {
            DontDestroyOnLoad(obj);
        }

        // subscribe to screen cover events
        ScreenCoverer.instance.OnCoverComplete += OnCoverComplete;

        // setup level data manager
        levelDataManager.Setup();
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
        // LOADING A LEVEL
        if (isCurrentlyLoadingLevel && covered) {


            // load level
            LoadLevel(levelToLoad);

            // end cover
            ScreenCoverer.instance.EndCover(screenUnCoverTime);

            // set variables
            isCurrentlyLoadingLevel = false;
            currentLevel = levelToLoad;
            levelToLoad = null;

            // start tracking the level Data
            levelDataManager.StartTrackingLevelData(currentLevel, Time.time);


            // invoke event
            OnLevelLoaded?.Invoke(this, EventArgs.Empty);
            return;
        }

        // RETURNING TO MENU
        if (isCurrentlyReturningToMenu && covered) {

            // set camera target and reset its settings
            PlayerCameraHandler.Instance.SetCameraTargetObject(Player.Instance.transform);
            PlayerCameraHandler.Instance.ResetToDefaultValues();

            // save and untrack level data
            levelDataManager.SaveLevelData(currentLevel, Time.time);
            levelDataManager.StopTrackingLevelData();

            // unload level
            UnloadLevel(currentLevel);
            currentLevel = null;

            // create a new spawn point for the menu
            Instantiate(menuSpawnPointPrefab, Vector3.zero, Quaternion.identity, transform); // create a new spawn point for the menu

            // end cover
            ScreenCoverer.instance.EndCover(screenUnCoverTime);

            // set variables
            isCurrentlyReturningToMenu = false;

            Player.Instance.ExitPlayerFinishedLevelMode();

            // invoke event
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

    public void BeginLevelCompletedSequence(Transform levelCompleteObjectTransform) {

        if (!Player.Instance.GetPracticeModeEnabled()) { // dont save data if in practice mode
        // save completed data instantly to prevent rage moments from computer crashes or similair
            levelDataManager.SetLevelCompleted();
            levelDataManager.SaveLevelData(currentLevel, Time.time);
        }

        // start the level completed sequence   
        LevelCompletedSequence.Instance.StartLevelCompletedSequence(levelCompleteObjectTransform);

    }

}
