using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JoinColorChangeController : MonoBehaviour
{
    private Image[] images;
    private Image targetColor;

    private List<PlayerColorType> colors;
    private int colorIndex = 0;

    private void Start() {
        InitPlayerHeadColor();
    }

    public void InitPlayerHeadColor() {
        if (gameObject.name != "ColorValue") return;
        images = GetComponentsInChildren<Image>();
        targetColor = null;
        foreach (Image image in images)
            if (image.sprite.name == "playerHead") {
                targetColor = image;
                break;
            }
        targetColor.material = new Material(targetColor.material);
        targetColor.material.SetColor("_PlayerColor", PlayerColor.Red);

    }

    public void GetSelectColor(List<PlayerColorType> colorList) {
        colors = colorList;
    }

    private int ColorIndex(bool isRight) {
        if (isRight) {
            colorIndex++;
            if (colorIndex >= colors.Count - 1)
                colorIndex = 0;
        }
        else {
            colorIndex--;
            if (colorIndex < 0)
                colorIndex = colors.Count - 2;
        }
        return colorIndex;
    }

    public void ChangeColor(bool isRight) {
        if (targetColor == null) { return; }

        targetColor.material.SetColor(
            "_PlayerColor", PlayerColor.GetColor(colors[ColorIndex(isRight)]));
    }

    public PlayerColorType GetSelectClientColor() {
        return colors[colorIndex];
    }
}
