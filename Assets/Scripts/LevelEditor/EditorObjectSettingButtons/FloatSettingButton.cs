using System;
using System.Collections.Generic;
using UnityEngine;

public class FloatSettingButton : EditorObjectSettingButton {

    [SerializeField] private UIButton increaseButton;
    [SerializeField] private UIButton decreaseButton;
    [SerializeField] private TMPro.TextMeshProUGUI valueText;



    protected override void Setup()
    {
        increaseButton.OnUIButtonPressed += IncreaseValue;
        decreaseButton.OnUIButtonPressed += DecreaseValue;
        valueText.text = GetValue<float>(settingName).ToString();
    }

    private void DecreaseValue(object sender, EventArgs e)
    {
        float newVal = GetValue<float>(settingName) - 1;
        SetValue<float>(settingName, newVal);
        valueText.text = newVal.ToString();
    }

    private void IncreaseValue(object sender, EventArgs e)
    {
        float newVal = GetValue<float>(settingName) + 1;
        SetValue<float>(settingName, newVal);
        valueText.text = newVal.ToString();
    }

    public override List<UIButton> GetUIButtons()
    {
        return new List<UIButton> { increaseButton, decreaseButton };
    }
}