using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerColorType {
    red, orange, yellow, 
    green, blue, purple,
    pink, gray
}
public class PlayerColor : MonoBehaviour {
    private static Color[] colors = new Color[] {
        new Color(1f, 0.4f, 0.44f),
        new Color(1f, 0.6f, 0.4f),
        new Color(1f, 0.95f, 0.4f),
        new Color(0.35f, 1f, 0.6f),
        new Color(0.48f, 0.8f, 1f),
        new Color(0.73f, 0.6f, 1f),
        new Color(1f, 0.65f, 1f),
        new Color(0.8f, 0.8f, 0.8f),
    };

    public static Color GetColor(PlayerColorType playerColor) { return colors[(int)playerColor]; }
    public static Color Red { get { return colors[(int)PlayerColorType.red]; } }
    public static Color Orange { get { return colors[(int)PlayerColorType.orange]; } }
    public static Color Yellow { get { return colors[(int)PlayerColorType.yellow]; } }
    public static Color Green { get { return colors[(int)PlayerColorType.green]; } }
    public static Color Blue { get { return colors[(int)PlayerColorType.blue]; } }
    public static Color Pruple { get { return colors[(int)PlayerColorType.purple]; } }
    public static Color Pink { get { return colors[(int)PlayerColorType.pink]; } }
    public static Color Gray { get { return colors[(int)PlayerColorType.gray]; } }
}
