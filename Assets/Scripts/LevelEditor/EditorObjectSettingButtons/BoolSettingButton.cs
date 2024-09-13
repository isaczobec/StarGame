using System;
using System.Collections.Generic;
using UnityEngine;

public class BoolSettingButton : EditorObjectSettingButton {

    [SerializeField] private UIButton checkButton;
    [SerializeField] private TMPro.TextMeshProUGUI valueText;

    bool isChecked;



    protected override void Setup()
    {
        checkButton.OnUIButtonPressed += ChangeChecked;
        isChecked = GetValue<bool>(settingName);
        valueText.text = isChecked.ToString();
    }


    private void ChangeChecked(object sender, EventArgs e)
    {
        isChecked = !isChecked;
        SetValue<bool>(settingName, isChecked);
        valueText.text = isChecked.ToString();
    }

    public override List<UIButton> GetUIButtons()
    {
        return new List<UIButton> { checkButton };
    }
}