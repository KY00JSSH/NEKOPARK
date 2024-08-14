using UnityEngine;
using UnityEngine.UI;

public enum LobbyMenuType {
    START = 0,
    RETURN,
    EXIT
}

public class LobbyMenuController : MonoBehaviour {
    private Text menuText;
    private bool isConfirm = false;
    private LobbyMenuType lobbyMenuType = LobbyMenuType.START;
    private ConfirmManager confirmManager;
    private LobbyUIController lobbyUIController;
    private NetworkSceneManager networkSceneManager;
    private bool isExit = false;

    private void Awake() {
        menuText = gameObject.GetComponentInChildren<Text>();
        confirmManager = FindObjectOfType<ConfirmManager>();
        lobbyUIController = FindObjectOfType<LobbyUIController>();
        networkSceneManager = FindObjectOfType<NetworkSceneManager>();
    }

    private void Update() {

        if (!isConfirm && Input.GetButtonDown("Horizontal")) {
            float horizontalInput = Input.GetAxis("Horizontal");
            if (horizontalInput > 0) {
                RightButtonClick();
            }
            else {
                LeftButtonClick();
            }
            return;
        }

        if (!isConfirm && Input.GetButtonDown("Select") || !isConfirm && Input.GetButtonDown("menu")) {
            SelectMenu();
            return;
        }

        if (isConfirm && Input.GetButtonDown("Select") || isConfirm && Input.GetButtonDown("menu")) {
            OpenMenu();
            return;
        }

        checkMenuText();
    }

    public void OpenMenu() {
        if (isExit) {
            networkSceneManager.LoadMainScene();
        }
        else {
            //TODO: 게임 시작
        }
    }

    public void SelectMenu() {
        switch (lobbyMenuType) {
            case LobbyMenuType.START:
                isExit = false;
                openConfirm();
                break;
            case LobbyMenuType.RETURN:
                lobbyUIController.CloseMenu();
                break;
            case LobbyMenuType.EXIT:
                isExit = true;
                openConfirm();
                break;
        }
    }

    private void openConfirm() {
        confirmManager.gameObject.SetActive(true);
        confirmManager.ConfirmLobbyTextChage(lobbyMenuType);
        isConfirm = true;
    }

    public void CloseConfirm() {
        confirmManager.gameObject.SetActive(false);
        isConfirm = false;
    }

    public void RightButtonClick() {
        int menuNum = (int)lobbyMenuType;
        if (menuNum == 2) {
            menuNum = 0;
        }
        else {
            menuNum++;
        }
        lobbyMenuType = (LobbyMenuType)menuNum;
    }

    public void LeftButtonClick() {
        int menuNum = (int)lobbyMenuType;
        if (menuNum == 0) {
            menuNum = 2;
        }
        else {
            menuNum--;
        }
        lobbyMenuType = (LobbyMenuType)menuNum;
    }

    private void checkMenuText() {
        switch (lobbyMenuType) {
            case LobbyMenuType.START:
                menuText.text = "GAME START";
                break;
            case LobbyMenuType.RETURN:
                menuText.text = "RETURN";
                break;
            case LobbyMenuType.EXIT:
                menuText.text = "FINISH GAME";
                break;
        }
    }
}
