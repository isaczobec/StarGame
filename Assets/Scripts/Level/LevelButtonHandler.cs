using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelButtonHandler : MonoBehaviour
{

    [SerializeField] private GameObject levelButtonPrefab;

    [SerializeField] private LevelSO[] levelsToDisplay;

    [Header("Level button display settings")]
    [SerializeField] private int maxLevelColumns = 2;
    [SerializeField] private int defaultRowBeginOffset = 2;
    [SerializeField] private float columnSpacing = 23f;
    [SerializeField] private float rowSpacing = 23f;

    [SerializeField] private float timeBetweenButtonAppearances = 0.1f;    
    [SerializeField] private Vector3 choosenLevelPositionOffset = new Vector3(0, 150f, 0);

    [Header("Level load settings")]
    [SerializeField] private float timeBetweenButtonDisappearances = 0.03f;
    [SerializeField] private float timeForButtonToMoveToCenter = 0.6f;
    [SerializeField] private float timeUntilClickedButtonDisappears = 1f;
    [SerializeField] private float timeUntilBeginScreenCover = 1.3f;
    [SerializeField] private float screenCoverTime = 0.5f;
    [SerializeField] private float screenUnCoverTime = 0.3f;




    private List<LevelSelectButton> levelButtons = new List<LevelSelectButton>();
    private LevelSO levelToLoad;

    private Coroutine loadLevelSequenceCoroutine;
    private bool currentlyStartingLevel = false; // if were currently starting a level from this script


    public static LevelButtonHandler Instance { get; private set; }

    private void Awake() {
        Instance = this;
    }
    

    // Start is called before the first frame update
    void Start()
    {
        SpawnLevelButtons(levelsToDisplay);

        ScreenCoverer.instance.OnCoverComplete += OnCoverComplete;

        LevelHandler.Insance.OnReturnToMenu += OnReturnToMenu;
    }


    private void SpawnSingleLevelButton(LevelSO levelSO, Vector3 position, float timeToAppear = 0) {
        GameObject levelButton = Instantiate(levelButtonPrefab, transform);
        levelButton.transform.localPosition = position;
        LevelSelectButton lButton = levelButton.GetComponent<LevelSelectButton>();
        levelButtons.Add(lButton);
        lButton.SetupButton(levelSO);
        lButton.ChangeVisible(true,timeToAppear);
    }


    private void SpawnLevelButtons(LevelSO[] levelSOs) {

        int currentColumn = 0;
        int currentRow = 0;
        int buttonIndex = 0;
        foreach (LevelSO levelSO in levelSOs) {

            // load the level data
            LevelHandler.Insance.levelDataManager.LoadLevelData(levelSO);

            // increase row and column if needed
            if (currentColumn >= maxLevelColumns) {
                currentColumn = 0;
                currentRow++;
            }

            // calculate position and offset spawn time
            Vector3 pos = new Vector3(((float)currentColumn - ((maxLevelColumns-1)/2f)) * columnSpacing, (currentRow-defaultRowBeginOffset) * -rowSpacing, 0);
            float timeUntilAppear = buttonIndex * timeBetweenButtonAppearances;
            SpawnSingleLevelButton(levelSO,pos,timeUntilAppear);

            // increment column and button index
            currentColumn++;
            buttonIndex++;
        }
    }

    /// <summary>
    /// Disappear all buttons after a certain time. 
    /// </summary>
    /// <param name="timeBetweenButtonDisppearances"></param>
    /// <param name="buttonsToSave">buttons not to make disappear.</param>
    private void DisappearButtons(float timeBetweenButtonDisppearances = 0.05f, List<LevelSelectButton> buttonsToSpare = null) {
        if (buttonsToSpare == null) {
            buttonsToSpare = new List<LevelSelectButton>();
        }


        for (int i = 0; i < levelButtons.Count; i++) {
            if (buttonsToSpare.Contains(levelButtons[i])) { continue; }
            levelButtons[i].ChangeVisible(false, i * timeBetweenButtonDisppearances);
        }
    }


    private void DestroyAllLevelButtons() {
        foreach (LevelSelectButton button in levelButtons) {
            Destroy(button.gameObject);
        }
        levelButtons.Clear();
    }

    public void LevelButtonClicked(LevelSelectButton levelButton) {
        StartLoadLevelSequenceFromButton(levelButton);
    }

    public void StartLoadLevelSequenceFromButton(LevelSelectButton levelButton) {
        levelToLoad = levelButton.levelSO;
        if (loadLevelSequenceCoroutine != null) {
            StopCoroutine(loadLevelSequenceCoroutine);
        }
        loadLevelSequenceCoroutine = StartCoroutine(LoadLevelSequence(levelButton));
    }


    private IEnumerator LoadLevelSequence(LevelSelectButton levelButton) {
        currentlyStartingLevel = true;
        levelButton.MoveToNewPosition(transform.position + choosenLevelPositionOffset, timeForButtonToMoveToCenter);
        DisappearButtons(timeBetweenButtonDisappearances, new List<LevelSelectButton> {levelButton});
        levelButton.ChangeVisible(false, timeUntilClickedButtonDisappears); // Make the clicked button disappear after a certain time


        yield return new WaitForSeconds(timeUntilBeginScreenCover);

        // loading level

        LevelHandler.Insance.LoadLevelScreenCovered(levelToLoad, screenCoverTime, screenUnCoverTime);

    }


    // subscribed to the OnCoverComplete event
    private void OnCoverComplete(object sender, bool covered)
    {
        if (covered && currentlyStartingLevel) {
            DestroyAllLevelButtons();
            currentlyStartingLevel = false;
        }
    }

    private void OnReturnToMenu(object sender, EventArgs e)
    {
        SpawnLevelButtons(levelsToDisplay);
    }
    

}
