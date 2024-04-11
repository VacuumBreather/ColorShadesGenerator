using System;
using System.Windows.Media;

namespace ColorShadesGenerator;

public struct HslColor
{
    public HslColor(double h, double s, double l)
    {
        H = h;
        S = s;
        L = l;
    }

    public static HslColor FromColor(Color color)
    {
        RgbToHsl(color.R, color.G, color.B, out var h, out var s, out var l);

        return new HslColor(h, s, l);
    }

    public Color ToColor()
    {
        HslToRgb(H, S, L, out var r, out var g, out var b);

        return Color.FromRgb((byte) r, (byte) g, (byte) b);
    }

    public double H { get; }

    public double S { get; }

    public double L { get; }

    // Convert an RGB value into an HLS value.
    private static void RgbToHsl(int r, int g, int b,
        out double h, out double s, out double l)
    {
        // Convert RGB to a 0.0 to 1.0 range.
        var doubleR = r / 255.0;
        var doubleG = g / 255.0;
        var doubleB = b / 255.0;

        // Get the maximum and minimum RGB components.
        var max = doubleR;
        if (max < doubleG) max = doubleG;
        if (max < doubleB) max = doubleB;

        var min = doubleR;
        if (min > doubleG) min = doubleG;
        if (min > doubleB) min = doubleB;

        var diff = max - min;
        l = (max + min) / 2;

        if (Math.Abs(diff) < 0.00001)
        {
            s = 0;
            h = 0; // H is really undefined.
        }
        else
        {
            if (l <= 0.5) s = diff / (max + min);
            else s = diff / (2 - max - min);

            var rDist = (max - doubleR) / diff;
            var gDist = (max - doubleG) / diff;
            var bDist = (max - doubleB) / diff;

            if (doubleR == max)
                h = bDist - gDist;
            else if (doubleG == max)
                h = 2 + rDist - bDist;
            else
                h = 4 + gDist - rDist;

            h = h * 60;

            if (h < 0) h += 360;
        }
    }


    private static void HslToRgb(double h, double s, double l,
        out int r, out int g, out int b)
    {
        double p2;

        p2 = l <= 0.5 ? l * (1 + s) : l + s - l * s;

        var p1 = 2 * l - p2;
        double doubleR;
        double doubleG;
        double doubleB;

        if (s == 0)
        {
            doubleR = l;
            doubleG = l;
            doubleB = l;
        }
        else
        {
            doubleR = QqhToRgb(p1, p2, h + 120);
            doubleG = QqhToRgb(p1, p2, h);
            doubleB = QqhToRgb(p1, p2, h - 120);
        }

        // Convert RGB to the 0 to 255 range.
        r = (int) (doubleR * 255.0);
        g = (int) (doubleG * 255.0);
        b = (int) (doubleB * 255.0);
    }

    private static double QqhToRgb(double q1, double q2, double hue)
    {
        switch (hue)
        {
            case > 360:
                hue -= 360;
                break;
            case < 0:
                hue += 360;
                break;
        }

        return hue switch
        {
            < 60 => q1 + (q2 - q1) * hue / 60,
            < 180 => q2,
            < 240 => q1 + (q2 - q1) * (240 - hue) / 60,
            _ => q1
        };
    }
}