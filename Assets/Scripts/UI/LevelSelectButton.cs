using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectButton : UIButton
{

    // serializefield for testing
    private LevelSO levelSO;


    [SerializeField] private TMPro.TextMeshProUGUI levelNameText;
    [SerializeField] private TMPro.TextMeshProUGUI levelDifficultyText;
    [SerializeField] private TMPro.TextMeshProUGUI authorText;
    [SerializeField] private TMPro.TextMeshProUGUI secondsPlayedText;
    [SerializeField] private TMPro.TextMeshProUGUI timesDiedText;

    [Header("Visuals")]
    [SerializeField] private Animator buttonAnimator;
    [SerializeField] private Shader buttonShader;
    [SerializeField] private Image buttonImage;

    private const string isHoveredRef = "_IsHovered";

    private const string begingHoverRef = "BeginHover";
    private const string endHoverRef = "EndHover";
    private const string cycleOffsetRef = "CycleOffset";

    // instead of start
    public override void InitButton()
    {
        
    }

    public void SetupButton(LevelSO levelSO)
    {
        this.levelSO = levelSO;

        // Instantiate material
        buttonImage.material = new Material(buttonShader);

        // set the text
        levelNameText.text = levelSO.levelName;
        levelDifficultyText.text = levelSO.levelDifficulty;
        authorText.text = levelSO.author;
        secondsPlayedText.text = ParseSeconds(levelSO.secondsPlayed);
        timesDiedText.text = levelSO.timesDied.ToString();

        
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
        LevelHandler.Insance.LoadLevel(levelSO.sceneToLoadRefString);
    }


    // --------- VISUALS

    public override void OnHoverEnter()
    {
        buttonAnimator.SetTrigger(begingHoverRef);
        buttonImage.material.SetFloat(isHoveredRef, 1f);
    }

    public override void OnHoverExit()
    {
        buttonAnimator.SetTrigger(endHoverRef);
        buttonImage.material.SetFloat(isHoveredRef, 0f);
    }

}
