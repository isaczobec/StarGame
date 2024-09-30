using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorSettingButton : EditorObjectSettingButton {

    [SerializeField] private Slider hueSlider;
    [SerializeField] private UIButtonAnimated hueButton;
    [SerializeField] private Slider saturationSlider;
    [SerializeField] private UIButtonAnimated saturationButton;
    [SerializeField] private Slider valueSlider;
    [SerializeField] private UIButtonAnimated valueButton;

    [SerializeField] private Image colorImage;

    private Vector3 colorVector;
    private Color color;

    protected override void Setup()
    {
        hueSlider.onValueChanged.AddListener(OnHueChanged);
        saturationSlider.onValueChanged.AddListener(OnSaturationChanged);
        valueSlider.onValueChanged.AddListener(OnValueChanged);

        // set slider and color image values
        colorVector = GetValue<Vector3>(settingName);
        color = new Color(colorVector.x, colorVector.y, colorVector.z);
        colorImage.color = color;
        var hsv = RGBtoHSVColor(color);
        hueSlider.value = hsv.x;
        saturationSlider.value = hsv.y;
        valueSlider.value = hsv.z;


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
        
        color = HSVtoRGBColor(hueSlider.value, saturationSlider.value, valueSlider.value);
        colorVector = new Vector3(color.r, color.g, color.b);
        colorImage.color = color;
        SetValue(settingName, colorVector);
    }

    public override List<UIButton> GetUIButtons() {
        return new List<UIButton> { hueButton, saturationButton, valueButton };
    }


    public static (double R, double G, double B) HSVtoRGB(double hue, double saturation, double value)
    {
        // Ensure that hue, saturation, and value are between 0 and 1
        hue = Math.Max(0, Math.Min(1, hue));
        saturation = Math.Max(0, Math.Min(1, saturation));
        value = Math.Max(0, Math.Min(1, value));
        
        double r = 0, g = 0, b = 0;
        
        // The hue is represented as a value between 0 and 1 (corresponding to 0-360 degrees)
        double h = hue * 360.0;
        double c = value * saturation;  // Chroma
        double x = c * (1 - Math.Abs((h / 60.0) % 2 - 1)); // Second largest component of this color
        double m = value - c;
        
        if (h >= 0 && h < 60)
        {
            r = c; g = x; b = 0;
        }
        else if (h >= 60 && h < 120)
        {
            r = x; g = c; b = 0;
        }
        else if (h >= 120 && h < 180)
        {
            r = 0; g = c; b = x;
        }
        else if (h >= 180 && h < 240)
        {
            r = 0; g = x; b = c;
        }
        else if (h >= 240 && h < 300)
        {
            r = x; g = 0; b = c;
        }
        else if (h >= 300 && h <= 360)
        {
            r = c; g = 0; b = x;
        }

        // Add m to match the lightness
        r += m;
        g += m;
        b += m;

        return (r, g, b);
    }

        public static (double H, double S, double V) RGBtoHSV(double r, double g, double b)
    {
        // Ensure that r, g, and b are between 0 and 1
        r = Math.Max(0, Math.Min(1, r));
        g = Math.Max(0, Math.Min(1, g));
        b = Math.Max(0, Math.Min(1, b));

        double max = Math.Max(r, Math.Max(g, b));
        double min = Math.Min(r, Math.Min(g, b));
        double delta = max - min;

        double hue = 0.0;
        double saturation = (max == 0) ? 0 : delta / max;
        double value = max;

        // Calculate hue
        if (delta != 0)
        {
            if (max == r)
            {
                hue = (g - b) / delta + (g < b ? 6 : 0);
            }
            else if (max == g)
            {
                hue = (b - r) / delta + 2;
            }
            else if (max == b)
            {
                hue = (r - g) / delta + 4;
            }
            hue /= 6;
        }

        return (hue, saturation, value);
    }

    public Color HSVtoRGBColor(double hue, double saturation, double value)
    {
        var rgb = HSVtoRGB(hue, saturation, value);
        return new Color((float)rgb.R, (float)rgb.G, (float)rgb.B);
    }

    public Vector3 RGBtoHSVColor(Color color)
    {
        var hsv = RGBtoHSV(color.r, color.g, color.b);
        return new Vector3((float)hsv.H, (float)hsv.S, (float)hsv.V);
    }



}
