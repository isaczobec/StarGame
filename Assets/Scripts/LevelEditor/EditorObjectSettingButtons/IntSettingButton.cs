using System;
using System.Collections.Generic;
using UnityEngine;

public class IntSettingButton : EditorObjectSettingButton {
    [SerializeField] private UIButton increaseButton;
    [SerializeField] private UIButton decreaseButton;
    [SerializeField] private TMPro.TextMeshProUGUI valueText;

    private int minValue;
    private int maxValue;

    protected override void Setup()
    {
        increaseButton.OnUIButtonPressed += IncreaseValue;
        decreaseButton.OnUIButtonPressed += DecreaseValue;
        valueText.text = GetValue<int>(settingName).ToString();
        var minMax = GetChangeSettings(settingName);
        minValue = (int)minMax[0];
        maxValue = (int)minMax[1];
    }

    private void DecreaseValue(object sender, EventArgs e)
    {
        int newVal = GetValue<int>(settingName) - 1;
        if (newVal < minValue) { newVal = minValue; }
        SetValue<int>(settingName, newVal);
        valueText.text = newVal.ToString();
    }

    private void IncreaseValue(object sender, EventArgs e)
    {
        int newVal = GetValue<int>(settingName) + 1;
        if (newVal > maxValue) { newVal = maxValue; }
        SetValue<int>(settingName, newVal);
        valueText.text = newVal.ToString();
    }

    public override List<UIButton> GetUIButtons()
    {
        return new List<UIButton> { increaseButton, decreaseButton };
    }
}
