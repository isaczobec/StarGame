using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectButton : UIButton
{

    public LevelSO levelSO {get; private set;}
    public LevelStatsData levelStatsData {get; private set;}


    [SerializeField] private TMPro.TextMeshProUGUI levelNameText;
    [SerializeField] private TMPro.TextMeshProUGUI levelDifficultyText;
    [SerializeField] private TMPro.TextMeshProUGUI authorText;
    [SerializeField] private TMPro.TextMeshProUGUI secondsPlayedText;
    [SerializeField] private TMPro.TextMeshProUGUI timesDiedText;

    [Header("Visuals")]
    [Header("Animator")]
    [SerializeField] private Animator buttonAnimator;
    [SerializeField] private float cycleOffsetRandomMax = 3f;
    
    [Header("Shader and sprite")]
    [SerializeField] private Shader buttonShader;
    [SerializeField] private Image buttonImage;
    [SerializeField] private Image starImage;
    [SerializeField] private Color completedStarColor = Color.green;

    [Header("MoveToNewPosition")]
    [SerializeField] private AnimationCurve moveToNewPositionCurve;



    private const string isHoveredRef = "_IsHovered";
    private const string completedRef = "_Completed";

    private const string begingHoverRef = "BeginHover";
    private const string endHoverRef = "EndHover";
    private const string appearRef = "Appear";
    private const string disappearRef = "Disappear";
    private const string cycleOffsetRef = "CycleOffset";

    private const string appearSpeedRef = "AppearSpeed";


    // member variables

    private bool isVisible;

    // coroutines

    private Coroutine appearCoroutine;
    private Coroutine moveToNewPositionCoroutine;

    // instead of start
    public override void InitButton()
    {
        
    }

    public void SetupButton(LevelSO levelSO)
    {
        this.levelSO = levelSO;

        // Instantiate material
        buttonImage.material = new Material(buttonShader);
        buttonImage.material.SetFloat(completedRef, levelSO.levelData.completed ? 1f : 0f);

        // set the star color
        if (levelSO.levelData.completed)
        {starImage.color = completedStarColor;}

        Debug.Log("completed?" + levelSO.levelData.completed + " " + levelSO.levelName);

        // set the text
        levelNameText.text = levelSO.levelName;
        levelDifficultyText.text = levelSO.levelDifficulty;
        authorText.text = levelSO.author;
        secondsPlayedText.text = ParseSeconds(levelSO.levelData.secondsPlayed);
        timesDiedText.text = levelSO.levelData.timesDied.ToString();

        // set the cycle offset
        buttonAnimator.SetFloat(cycleOffsetRef, Random.Range(0f, cycleOffsetRandomMax));
        
    }

    public void SetupButtonFromLevelStatsData(LevelStatsData levelStatsData) {

        this.levelStatsData = levelStatsData;

        // Instantiate material
        buttonImage.material = new Material(buttonShader);
        buttonImage.material.SetFloat(completedRef, levelStatsData.completed ? 1f : 0f);

        // set the star color
        if (levelStatsData.completed) {starImage.color = completedStarColor;}

        // set the text
        levelNameText.text = levelStatsData.levelName;
        levelDifficultyText.text = levelStatsData.difficulty; 
        authorText.text = levelStatsData.author;
        secondsPlayedText.text = ParseSeconds(levelStatsData.secondsPlayed);
        timesDiedText.text = levelStatsData.timesDied.ToString();

        // set the cycle offset
        buttonAnimator.SetFloat(cycleOffsetRef, Random.Range(0f, cycleOffsetRandomMax));
    }

    /// <summary>
    /// Parse seconds to hours, minutes and seconds (hh:mm:ss)
    /// </summary>
    /// <param name="seconds"></param>
    /// <returns></returns>
    public string ParseSeconds(int seconds)
    {
        int minutes = seconds / 60;
        int remainingSeconds = seconds % 60;

        int hours = minutes / 60;
        minutes = minutes % 60;

        string minutesString = minutes.ToString();
        if (minutes < 10)
        {
            minutesString = "0" + minutesString;
        }
        string remainingSecondsString = remainingSeconds.ToString();
        if (remainingSeconds < 10)
        {
            remainingSecondsString = "0" + remainingSecondsString;
        }
        string hoursString = hours.ToString();
        if (hours < 10)
        {
            hoursString = "0" + hoursString;
        }

        return hoursString + ":" + minutesString + ":" + remainingSecondsString;
    }

    public override void OnClick()
    {
        if (!isVisible) return;
        LevelButtonHandler.Instance.LevelButtonClicked(this);
    }


    // --------- VISUALS

    public override void OnHoverEnter()
    {
        if (!isVisible) return;
        buttonAnimator.SetTrigger(begingHoverRef);
        buttonImage.material.SetFloat(isHoveredRef, 1f);
    }

    public override void OnHoverExit()
    {
        if (!isVisible) return;
        buttonAnimator.SetTrigger(endHoverRef);
        buttonImage.material.SetFloat(isHoveredRef, 0f);
    }


    /// <summary>
    /// Make the button appear. This is called by the IEnumerator WaitToAppearCoroutine coroutine.
    /// </summary>
    private void ChangeButtonVisualsVisible(bool visible, float speedMultiplier = 1f) {
        buttonAnimator.SetFloat(appearSpeedRef, speedMultiplier);
        if (visible) {
            buttonAnimator.SetTrigger(appearRef);
        } else {
            buttonAnimator.SetTrigger(disappearRef);
        }
        isVisible = visible;
    }

    private IEnumerator WaitToAppearCoroutine(bool visible, float timeUntilAppearStart, float speedMultiplier = 1f) {
        yield return new WaitForSeconds(timeUntilAppearStart);
        ChangeButtonVisualsVisible(visible, speedMultiplier);

        appearCoroutine = null;
    }

    /// <summary>
    /// Begin the wait for the button to appear. After the timeUntilAppearStart, the button will appear
    /// </summary>
    /// <param name="timeUntilAppearStart"></param>
    public void ChangeVisible(bool visible, float timeUntilStart = 0f, float speedMultiplier = 1f) {
        if (isVisible == visible) return;
        if (appearCoroutine != null) {
            StopCoroutine(appearCoroutine);
        }
        if (timeUntilStart <= 0) {
            ChangeButtonVisualsVisible(visible, speedMultiplier);
            return;
        }
        appearCoroutine = StartCoroutine(WaitToAppearCoroutine(visible,timeUntilStart));
    }


    public void MoveToNewPosition(Vector3 targetPosition, float timeToMove) {
        if (moveToNewPositionCoroutine != null) {
            StopCoroutine(moveToNewPositionCoroutine);
        }
        moveToNewPositionCoroutine = StartCoroutine(MoveToNewPositionCoroutine(targetPosition, timeToMove));
    }

    private IEnumerator MoveToNewPositionCoroutine(Vector3 targetPosition, float timeToMove) {
        Vector3 startPosition = transform.position;
        float timeElapsed = 0;
        while (timeElapsed < timeToMove) {
            timeElapsed += Time.deltaTime;
            float t = moveToNewPositionCurve.Evaluate(timeElapsed / timeToMove);
            transform.position = Vector3.Lerp(startPosition, targetPosition, t);
            yield return null;
        }
        transform.position = targetPosition;

        moveToNewPositionCoroutine = null;
    }

}
