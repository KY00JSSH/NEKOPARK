using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameListUIManager : MonoBehaviour {
    private Canvas[] canvas;

    public static GameListUIManager instance;

    private int majorStageIndex = -1;
    public int MajorStageIndex { get { return majorStageIndex; } }
    public void SetMajorStageIndex(int num) { majorStageIndex = num; }

    private int minorStageIndex = -1;

    public int MinorStageIndex { get { return minorStageIndex; } }
    public void SetMinorStageIndex(int num) { minorStageIndex = num; }

    private void Awake() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else {
            Destroy(gameObject);
            return;
        }

        canvas = GetComponentsInChildren<Canvas>();
    }

    private void Start() {
        OpenMainList();
    }

    public void OpenMainList() {
        canvas[0].gameObject.SetActive(true);
        canvas[1].gameObject.SetActive(false);
    }

    public void OpenDetailList(int index) {
        canvas[0].gameObject.SetActive(false);
        canvas[1].gameObject.SetActive(true);
        GameListUIManager.instance.SetMajorStageIndex(index);
    }

    public void ClearAndOpenList() {
        LoadDataManager.instance.ClearStage(majorStageIndex, minorStageIndex);
        OpenGameListScene();
        canvas[0].gameObject.SetActive(false);
        canvas[1].gameObject.SetActive(true);
    }

    public void OpenGame(int index) {
        minorStageIndex = index;
        OpenGameScene();
    }

    private void OpenGameScene() {
        var roomManager = NetworkManager.singleton as RoomManager;
        if (RoomManager.ConnectedPlayer < roomManager.minPlayers) return;

        foreach (RoomPlayer player in roomManager.roomSlots)
            player.ReadyStateChanged(false, true);
        roomManager.ServerChangeScene($"Game_{majorStageIndex + 1}-{minorStageIndex + 1}");
        TCPclient.Instance.SendRequest(RequestType.Start);
    }

    private void OpenGameListScene() {
        var roomManager = NetworkManager.singleton as RoomManager;
        if (RoomManager.ConnectedPlayer < roomManager.minPlayers) return;

        foreach (RoomPlayer player in roomManager.roomSlots)
            player.ReadyStateChanged(false, true);
        roomManager.ServerChangeScene(roomManager.GameListScene);
        TCPclient.Instance.SendRequest(RequestType.Start);
    }
}
