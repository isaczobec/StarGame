using System;
using System.Collections.Generic;
using UnityEngine;

public class FloatSettingButton : EditorObjectSettingButton {

    [SerializeField] private UIButton increaseButton;
    [SerializeField] private UIButton decreaseButton;
    [SerializeField] private TMPro.TextMeshProUGUI valueText;

    private float[] minMaxIncrement;



    protected override void Setup()
    {
        increaseButton.OnUIButtonPressed += IncreaseValue;
        decreaseButton.OnUIButtonPressed += DecreaseValue;
        valueText.text = GetValue<float>(settingName).ToString();
        minMaxIncrement = GetChangeSettings(settingName);
    }

    private void DecreaseValue(object sender, EventArgs e)
    {
        float newVal = GetValue<float>(settingName) - minMaxIncrement[2];
        if (newVal < minMaxIncrement[0]){newVal = minMaxIncrement[0];}
        SetValue<float>(settingName, newVal);
        valueText.text = newVal.ToString();
    }

    private void IncreaseValue(object sender, EventArgs e)
    {
        float newVal = GetValue<float>(settingName) + minMaxIncrement[2];
        if (newVal > minMaxIncrement[1]){newVal = minMaxIncrement[1];}
        SetValue<float>(settingName, newVal);
        valueText.text = newVal.ToString();
    }

    public override List<UIButton> GetUIButtons()
    {
        return new List<UIButton> { increaseButton, decreaseButton };
    }
}