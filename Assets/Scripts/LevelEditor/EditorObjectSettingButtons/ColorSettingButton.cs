using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorSettingButton : EditorObjectSettingButton {

    [SerializeField] private Slider hueSlider;
    [SerializeField] private UIButtonAnimated hueButton;
    [SerializeField] private Slider saturationSlider;
    [SerializeField] private Image saturationImage;
    [SerializeField] private UIButtonAnimated saturationButton;
    [SerializeField] private Slider valueSlider;
    [SerializeField] private Image valueImage;
    [SerializeField] private UIButtonAnimated valueButton;

    [SerializeField] private Image colorImage;

    [SerializeField] private Shader saturationShader;
    [SerializeField] private Shader valueShader;
    [SerializeField] private string colorSettingName = "_Color";

    private int colorInt;
    private Color color;

    protected override void Setup()
    {
        hueSlider.onValueChanged.AddListener(OnHueChanged);
        saturationSlider.onValueChanged.AddListener(OnSaturationChanged);
        valueSlider.onValueChanged.AddListener(OnValueChanged);

        // set slider and color image values
        colorInt = GetValue<int>(settingName);
        color = ColorTransform.IntToColor(colorInt);
        colorImage.color = color;
        var hsv = ColorTransform.RGBtoHSVColor(color);
        hueSlider.value = hsv.x;
        saturationSlider.value = hsv.y;
        valueSlider.value = hsv.z;

        // initialize materials for saturation and value
        saturationImage.material = new Material(saturationShader);
        valueImage.material = new Material(valueShader);
        saturationImage.material.SetColor(colorSettingName, color);
        valueImage.material.SetColor(colorSettingName, color);

    }

    private void OnValueChanged(float arg0)
    {
        UpdateColor();
    }

    private void OnSaturationChanged(float arg0)
    {
        UpdateColor();
    }

    private void OnHueChanged(float arg0)
    {
        UpdateColor();
    }

    private void UpdateColor() {
        
        color = ColorTransform.HSVtoRGBColor(hueSlider.value, saturationSlider.value, valueSlider.value);
        colorInt = ColorTransform.ColorToInt(color);
        colorImage.color = color;
        saturationImage.material.SetColor(colorSettingName, color);
        valueImage.material.SetColor(colorSettingName, color);
        SetValue(settingName, colorInt);
    }

    public override List<UIButton> GetUIButtons() {
        return new List<UIButton> { hueButton, saturationButton, valueButton };
    }

}
