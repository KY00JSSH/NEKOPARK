using UnityEngine;
using UnityEngine.UI;

public class ConfirmManager : MonoBehaviour {
    private Button[] buttons;
    private bool isYesHover = true;
    private int playerCount = 2;

    private Text confirmText;
    private Image playerIconImage;

    private void Awake() {
        buttons = GetComponentsInChildren<Button>();
        confirmText = gameObject.GetComponentInChildren<Text>();
    }

    private void Start() {
        gameObject.SetActive(false);

        playerIconImage.material = new Material(playerIconImage.material);
        SetPlayerIconColor();
    }

    public void ConfirmMainTextChage(MenuType type) {
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

    public void ConfirmLobbyTextChage(LobbyMenuType type) {
        switch (type) {
            case LobbyMenuType.START:
                confirmText.text = "GAME START?";
                break;
            case LobbyMenuType.RETURN:
                break;
            case LobbyMenuType.EXIT:
                confirmText.text = "FINISH?";
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

    private void SetPlayerIconColor() {
        playerIconImage.material.SetColor("_PlayerColor", PlayerColor.GetColor(RoomPlayer.MyRoomPlayer.playerColor));
    }
}
