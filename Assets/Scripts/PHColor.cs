using UnityEngine;

public static class PhColor
{
    static Color[] c = {
        new Color(1f,0f,0f),      // 0 red
        new Color(1f,0.2f,0.3f),  // 1 pink-red
        new Color(1f,0.4f,0f),    // 2 orange-red
        new Color(1f,0.55f,0f),   // 3 orange
        new Color(1f,0.7f,0f),    // 4 yellow-orange
        new Color(1f,0.92f,0f),   // 5 yellow
        new Color(0.7f,0.87f,0.1f),//6 yellow-green
        new Color(0f,0.6f,0f),    // 7 green
        new Color(0f,0.7f,0.35f), // 8 blue-green
        new Color(0f,0.6f,0.8f),  // 9 cyan-blue
        new Color(0f,0.47f,1f),   // 10 light blue
        new Color(0.3f,0.3f,0.7f),// 11 indigo
        new Color(0.35f,0.25f,0.7f),//12 purple-blue
        new Color(0.6f,0.15f,0.6f), //13 purple
        new Color(0.7f,0.1f,0.45f)  //14 magenta-purple
    };

    public static Color Get(float ph)
    {
        ph = Mathf.Clamp(ph, 0f, 14f);
        int i = Mathf.FloorToInt(ph);
        return (i == 14) ? c[14] : Color.Lerp(c[i], c[i+1], ph - i);
    }
}