using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelHandler : MonoBehaviour
{

    [SerializeField] private GameObject[] dontDestroyOnLoadObjects;
    [SerializeField] private GameObject menuSpawnPointPrefab;
    [SerializeField] private string editorLevelLoadSceneName = "LevelEditor";

    public static LevelHandler Insance { get; private set; }    

    /// <summary>
    /// A list of handlers for active level(s)
    /// </summary>
    private List<LevelWorldObjectsHandler> levelWorldObjectsHandlers = new List<LevelWorldObjectsHandler>();

    public bool isCurrentlyLoadingLevel { get; private set; } = false;
    public bool isCurrentlyReturningToMenu { get; private set; } = false;
    public LevelSO levelToLoad { get; private set; }
    public LevelStatsData levelDataToLoad { get; private set; }
    public LevelSO currentLevel { get; private set; }
    public LevelStatsData currentLevelData { get; private set; }
    [Header("Settings")]
    [SerializeField] private float screenUnCoverTime = 0.3f;
    [SerializeField] private float musicFadeOutTime = 0.5f;



    /// <summary>
    /// The level data manager, used for saving and loading level data
    /// </summary>
    public LevelDataManager levelDataManager {get; private set;}

    public event EventHandler OnLevelLoaded;
    public event EventHandler OnReturnToMenu;

    public bool isLoadingLevel;



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

        // load an editor level
        if (levelSO.loadLevelIDInstead) {
            SceneManager.LoadScene(editorLevelLoadSceneName);
        } else { // load a normal scene
            SceneManager.LoadScene(levelSO.sceneToLoadRefString);
        }
        
    }

    public void DestroyDontDestroyOnLoadObjects()
    {
        foreach (GameObject obj in dontDestroyOnLoadObjects)
        {
            Destroy(obj);
        }
    }


    public void UnloadLevel()
    {
        Debug.Log("Unloading level: " + currentLevelData.levelName);
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
        SceneManager.UnloadSceneAsync(editorLevelLoadSceneName);
    }

    public void LoadLevelScreenCovered(LevelStatsData levelDataToLoad, float screenCoverTime, float screenUnCoverTime)
    {
        if (isCurrentlyLoadingLevel)
        {
            Debug.LogWarning("Already loading a level");
            return;
        }

        this.levelDataToLoad = levelDataToLoad;
        isCurrentlyLoadingLevel = true;
        this.screenUnCoverTime = screenUnCoverTime;


        ScreenCoverer.instance.BeginCover(screenCoverTime);

    }

    private IEnumerator OnCoverCompleteLoadLevel() {

        isCurrentlyLoadingLevel = true;

        // load level loader scene and the level
        SceneManager.LoadScene(editorLevelLoadSceneName);
        yield return new WaitForSeconds(0.05f); // wait for scene to load
        EditorLevelDataLoader.instance.LoadToPlayableLevel(levelDataToLoad.levelID);
        yield return new WaitForSeconds(0.05f); // wait for scene to load

        isCurrentlyLoadingLevel = false;

            // end cover
            ScreenCoverer.instance.EndCover(screenUnCoverTime);

            // set variables
            isCurrentlyLoadingLevel = false;
            currentLevelData = levelDataToLoad;
            currentLevel = levelToLoad;
            levelDataToLoad = null;
            levelToLoad = null;

            // start tracking the level Data
            levelDataManager.StartTrackingLevelData(currentLevelData, Time.time);

            // play music
            MusicManager.insance.PlaySong(EditorLevelDataLoader.instance.currentEditorLevelData.songID);


            // invoke event
            OnLevelLoaded?.Invoke(this, EventArgs.Empty);

            yield return null;
    }

    private void OnCoverComplete(object sender, bool covered)
    {
        // LOADING A LEVEL
        if (isCurrentlyLoadingLevel && covered) {

            StartCoroutine(OnCoverCompleteLoadLevel());
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
            UnloadLevel();
            currentLevel = null;
            currentLevelData = null;

            // create a new spawn point for the menu
            Instantiate(menuSpawnPointPrefab, Vector3.zero, Quaternion.identity, transform); // create a new spawn point for the menu

            // end cover
            ScreenCoverer.instance.EndCover(screenUnCoverTime);

            // set variables
            isCurrentlyReturningToMenu = false;

            Player.Instance.ExitPlayerFinishedLevelMode();

            // play music
            MusicManager.insance.PlayMenuMusic();

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
    
        if (currentLevelData == null) {
            Debug.LogWarning("No current level to return from");
            return;
        }


        isCurrentlyReturningToMenu = true;
        this.screenUnCoverTime = screenUnCoverTime;

        // stop music
        if (currentLevel != null) MusicManager.insance.StopSong(currentLevel.levelSong, musicFadeOutTime);

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
