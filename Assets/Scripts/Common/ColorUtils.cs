using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class ColorUtils
{
    /// <param name="color">格式： #rrggbbaa</param>
    /// <returns></returns>
    public static Color GetColor(string color)
    {
        int r = Convert.ToInt32(color.Substring(1, 2), 16);
        int g = Convert.ToInt32(color.Substring(3, 2), 16);
        int b = Convert.ToInt32(color.Substring(5, 2), 16);
        return GetColor(r, g, b);
    }

    public static Color GetColor(int r, int g, int b)
    {
        return new Color(r / 255f, g / 255f, b / 255f);
    }

    public static string GetHtmlColor(Color color)
    {
        string r = Convert.ToString(Convert.ToInt32(color.r * 255), 16);
        string g = Convert.ToString(Convert.ToInt32(color.g * 255), 16);
        string b = Convert.ToString(Convert.ToInt32(color.b * 255), 16);

        return "#" + GetX2Format(r) + GetX2Format(g) + GetX2Format(b);
    }

    // 取得16进制2位数字格式
    static string GetX2Format(string text)
    {
        if (text.Length == 1)
        {
            return "0" + text;
        }

        return text;
    }

    public static Color ColorLerp(Color color1, Color color2, float f)
    {
        return Color.Lerp(color1, color2, f);
    }
}
