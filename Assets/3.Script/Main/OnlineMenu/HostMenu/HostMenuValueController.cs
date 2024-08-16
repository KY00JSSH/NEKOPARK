using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class HostMenuValueController : MonoBehaviour, IPointerEnterHandler {
    private HostMenuController hostMenuController;
    private Text valueText;
    private int valueNum = 0;

    private Image[] images;
    private Image targetColor;

    private PlayerColorType[] colors = 
        (PlayerColorType[])Enum.GetValues(typeof(PlayerColorType));
    private int colorIndex = 0;

    private int ColorIndex(bool isRight) {
        if (isRight) {
            colorIndex++;
            if(colorIndex >= colors.Length - 1) 
                colorIndex = 0;
        }
        else {
            colorIndex--;
            if (colorIndex < 0)
                colorIndex = colors.Length - 2;
        }
        return colorIndex;
    }
    public int GetColorIndex() { return colorIndex; }

    private void Awake() {
        hostMenuController = FindObjectOfType<HostMenuController>();
        valueText = gameObject.GetComponentInChildren<Text>();
    }

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

    public void ChangeValueText(bool isRight) {
        string name = gameObject.name;
        if (name.Equals("TypeValue")) {
            if (isRight) {
                if (valueNum == 1) {
                    valueNum = 0;
                    valueText.text = "PUBLIC";
                }
                else {
                    valueNum++;
                    valueText.text = "FRIEND";
                }
            }
            else {
                if (valueNum == 0) {
                    valueNum = 1;
                    valueText.text = "FRIEND";
                }
                else {
                    valueNum--;
                    valueText.text = "PUBLIC";
                }
            }
        }
        else if (name.Equals("CountValue")) {
            if (isRight) {
                if (valueNum == 6) {
                    valueNum = 0;
                    valueText.text = $"{valueNum + 2}";
                }
                else {
                    valueNum++;
                    valueText.text = $"{valueNum + 2}";
                }
            }
            else {
                if (valueNum == 0) {
                    valueNum = 6;
                    valueText.text = $"{valueNum + 2}";
                }
                else {
                    valueNum--;
                    valueText.text = $"{valueNum + 2}";
                }
            }
        }
        else if (name.Equals("ColorValue")) {
            if (targetColor == null) { return; }

            targetColor.material.SetColor(
                "_PlayerColor", PlayerColor.GetColor(colors[ColorIndex(isRight)]));
        }
    }

    public int GetValueNum() {
        return valueNum;
    }

    public void OnPointerEnter(PointerEventData eventData) {
        if (gameObject.name.Equals("TypeValue")) {
            hostMenuController.SetSelectHostMenu(HostMenuType.TYPE);
        }
        else if (gameObject.name.Equals("ColorValue")) {
            hostMenuController.SetSelectHostMenu(HostMenuType.COLOR);
        }
        else {
            hostMenuController.SetSelectHostMenu(HostMenuType.COUNT);
        }
    }
}
