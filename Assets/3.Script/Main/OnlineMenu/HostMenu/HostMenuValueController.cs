using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class HostMenuValueController : MonoBehaviour, IPointerEnterHandler {
    private HostMenuController hostMenuController;
    private Text valueText;
    private int valueNum = 0;

    private void Awake() {
        hostMenuController = FindObjectOfType<HostMenuController>();
        valueText = gameObject.GetComponentInChildren<Text>();
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