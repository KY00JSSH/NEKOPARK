using UnityEngine;
using UnityEngine.UI;
using Mirror;
using UnityEngine.SceneManagement;

public enum LobbyMenuType {
    START = 0,
    RETURN,
    EXIT
}

public class LobbyMenuController : MonoBehaviour {
    private Text menuText;
    private bool isConfirm = false;
    private LobbyMenuType lobbyMenuType = LobbyMenuType.RETURN;
    private ConfirmManager confirmManager;
    private LobbyUIController lobbyUIController;
    private bool isExit = false;

    public object ScneneManager { get; private set; }

    private void Awake() {
        menuText = gameObject.GetComponentInChildren<Text>();
        confirmManager = FindObjectOfType<ConfirmManager>();
        lobbyUIController = FindObjectOfType<LobbyUIController>();
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

        if (!isConfirm && Input.GetButtonDown("menu")) {
            SelectMenu();
            return;
        }

        if (isConfirm && Input.GetButtonDown("menu")) {
            OpenMenu();
            return;
        }

        checkMenuText();
    }

    public void OpenMenu() {
        if (isExit) {
            OnExitRoomButtonClicked();
            SceneManager.LoadScene("Main");
        }
        else {
            OnStartGameButtonClicked();
        }
    }

    public void OnExitRoomButtonClicked() {
        // 로비에서 방 나가기 버튼에 할당되는 메서드
        var roomManager = RoomManager.singleton;
        if (NetworkServer.active) {
            TCPclient.Instance.SendRequest(RequestType.Remove);
            roomManager.StopHost();
        }
        else {
            TCPclient.Instance.SendRequest(RequestType.Exit);
            roomManager.StopHost();
        }
    }

    public void OnStartGameButtonClicked() {
        // 로비에서 게임 시작 버튼에 할당되는 메서드
        var roomManager = NetworkManager.singleton as RoomManager;
        if (RoomManager.ConnectedPlayer < roomManager.minPlayers) return;

        foreach (RoomPlayer player in roomManager.roomSlots)
            player.ReadyStateChanged(false, true);
        roomManager.ServerChangeScene(roomManager.GameListScene);
        TCPclient.Instance.SendRequest(RequestType.Start);
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
        menuNum = CheckStartAvailable(menuNum, true);
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
        menuNum = CheckStartAvailable(menuNum, false);
        lobbyMenuType = (LobbyMenuType)menuNum;
    }

    public int CheckStartAvailable(int menuNum, bool isRight) {
        if (menuNum == 0) {
            if (!(NetworkServer.active && 
                ((NetworkManager.singleton as RoomManager).roomSlots.Count >= 2))) {
                menuNum = isRight ? 1 : 2;
            }
        }
        return menuNum;
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
