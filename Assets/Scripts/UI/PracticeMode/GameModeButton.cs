using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameModeButton : UIButton
{
    [SerializeField] private PlayerGameModeState gameModeState;
    [SerializeField] private Image image;

    public override void InitButton() {
        image.color = DefaultPlayerColors.defaultColorsDict[gameModeState];
    }

    public override void OnClick()
    {
        if (Player.Instance.GetPracticeModeEnabled()) {
            Player.Instance.SetGameModeStatePracticeMode(gameModeState);
        }
    }
}
