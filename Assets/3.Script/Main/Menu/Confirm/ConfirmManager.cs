using UnityEngine;
using UnityEngine.UI;

public class ConfirmManager : MonoBehaviour {
    private Button[] buttons;
    private bool isYesHover = true;
    private int playerCount = 2;

    private Text confirmText;

    private void Awake() {
        buttons = GetComponentsInChildren<Button>();
        confirmText = gameObject.GetComponentInChildren<Text>();
    }

    public void ConfirmTextChage(MenuType type) {
        switch (type) {
            case MenuType.PLAY_LOCAL:
                confirmText.text = $"{playerCount}PLAYERS GAME ?";
                break;
            case MenuType.PLAY_ONLINE:
                confirmText.text = "ONLINE PLAY MODE ?";
                break;
            case MenuType.OPTION:
                confirmText.text = "";
                break;
            case MenuType.EXIT:
                confirmText.text = "EXIT GAME ?";
                break;
        }
    }

    private void Update() {
        if (Input.GetButtonDown("Horizontal")) {
            isYesHover = !isYesHover;
        }

        if (isYesHover) {
            buttons[0].image.enabled = true;
            buttons[1].image.enabled = false;
        }
        else {
            buttons[0].image.enabled = false;
            buttons[1].image.enabled = true;
        }
    }

    public void SetIsHoverYes(bool isYes) {
        isYesHover = isYes;
    }

    public bool GetIsHoverYes() {
        return isYesHover;
    }
}
