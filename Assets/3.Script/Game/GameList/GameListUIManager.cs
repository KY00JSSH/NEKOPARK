using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameListUIManager : MonoBehaviour {
    private Canvas[] canvas;
      
    private void Awake() {
        canvas = GetComponentsInChildren<Canvas>();
    }

    private void Start() {
        OpenMainList();
    }

    public void OpenMainList() {
        canvas[0].gameObject.SetActive(true);
        canvas[1].gameObject.SetActive(false);
        canvas[2].gameObject.SetActive(false);
    }

    public void OpenDetailList(int index) {
        canvas[0].gameObject.SetActive(false);
        canvas[1].gameObject.SetActive(true);
        canvas[2].gameObject.SetActive(false);
        GameListManager.instance.SetMajorStageIndex(index);
    }

    public void OpenDetailListReturn() {
        canvas[0].gameObject.SetActive(false);
        canvas[1].gameObject.SetActive(true);
        canvas[2].gameObject.SetActive(false);
    }

    public void AllCloseUI() {
        canvas[0].gameObject.SetActive(false);
        canvas[1].gameObject.SetActive(false);
        canvas[2].gameObject.SetActive(false);
    }

    public void CloseDLCDialog() {
        canvas[2].gameObject.SetActive(false);
    }

    public void OpenGame(int index) {
        GameListManager.instance.SetMinorStageIndex(index);
        if ((GameListManager.instance.MajorStageIndex == 1 && index > 0) || GameListManager.instance.MajorStageIndex > 1) {
            canvas[2].gameObject.SetActive(true);
        }
        else {
            if (GameListManager.instance.IsLocalGame) {
                AllCloseUI();

                AudioManager.instance.StopBGM(0);
            
                SceneManager.LoadScene($"Game_{GameListManager.instance.MajorStageIndex + 1}-{GameListManager.instance.MinorStageIndex + 1}");
            }
            else {
                AllCloseUI();
                OpenGameScene();
            }
        }
    }

    private void OpenGameScene() {
        var roomManager = NetworkManager.singleton as RoomManager;
        //if (RoomManager.ConnectedPlayer < roomManager.minPlayers) return;

        foreach (RoomPlayer player in roomManager.roomSlots)
            player.ReadyStateChanged(false, true);

        roomManager.ServerChangeScene($"Game_{GameListManager.instance.MajorStageIndex + 1}-{GameListManager.instance.MinorStageIndex + 1}");
        //roomManager.ServerChangeScene(roomManager.Game_1_1);
    }

    private void OpenGameListScene() {
        var roomManager = NetworkManager.singleton as RoomManager;
        if (RoomManager.ConnectedPlayer < roomManager.minPlayers) return;

        foreach (RoomPlayer player in roomManager.roomSlots)
            player.ReadyStateChanged(false, true);
        roomManager.ServerChangeScene(roomManager.GameListScene);
    }
}
