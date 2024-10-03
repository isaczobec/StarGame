using System;
using UnityEngine;

public class ColorTransform {
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

    public static Color HSVtoRGBColor(double hue, double saturation, double value)
    {
        var rgb = HSVtoRGB(hue, saturation, value);
        return new Color((float)rgb.R, (float)rgb.G, (float)rgb.B);
    }

    public static Vector3 RGBtoHSVColor(Color color)
    {
        var hsv = RGBtoHSV(color.r, color.g, color.b);
        return new Vector3((float)hsv.H, (float)hsv.S, (float)hsv.V);
    }

    public static int ColorToInt(Color color)
    {
        int r = (int)(color.r * 255);
        int g = (int)(color.g * 255);
        int b = (int)(color.b * 255);
        return (r << 16) | (g << 8) | b;
    }

    public static Color IntToColor(int color)
    {
        int r = (color >> 16) & 0xFF;
        int g = (color >> 8) & 0xFF;
        int b = color & 0xFF;
        return new Color(r / 255f, g / 255f, b / 255f);
    }
}