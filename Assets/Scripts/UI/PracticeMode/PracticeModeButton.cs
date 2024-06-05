using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PracticeModeButton : UIButton
{

    [SerializeField] private GameObject[] practiceModeButtons;

    public override void InitButton()
    {
        SetPracticeModeButtonsActive(Player.Instance.GetPracticeModeEnabled());
    }

    public override void OnClick()
    {
        if (Player.Instance.GetPracticeModeEnabled()) {
            Player.Instance.SetPracticeModeEnabled(false);
            SetPracticeModeButtonsActive(false);
        } else {
            Player.Instance.SetPracticeModeEnabled(true);
            SetPracticeModeButtonsActive(true);
        }
    }

    private void SetPracticeModeButtonsActive(bool active)
    {
        foreach (GameObject button in practiceModeButtons) {
            button.SetActive(active);
        }
    }

    

}
