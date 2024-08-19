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

    private bool isLocalGame = false;

    public bool IsLocalGame { get { return isLocalGame; } }
    public void SetIsLocalGame(bool yn) { isLocalGame = yn; }

    [SerializeField] GameObject networkGameCore;

    private void Awake() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else {
            Destroy(gameObject);
            return;
        }

        int localYn = PlayerPrefs.GetInt("localGame");
        if (localYn == 1) {
            isLocalGame = true;
            networkGameCore.gameObject.SetActive(false);
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

    public void OpenDetailListReturn() {
        canvas[0].gameObject.SetActive(false);
        canvas[1].gameObject.SetActive(true);
    }

    public void AllCloseUI() {
        canvas[0].gameObject.SetActive(false);
        canvas[1].gameObject.SetActive(false);
    }

    public void ClearAndOpenList() {
        LoadDataManager.instance.ClearStage(majorStageIndex, minorStageIndex);
        if (isLocalGame) {
            SceneManager.LoadScene("Game_List");
            OpenDetailListReturn();
            Save.instance.MakeSingleSave();
        }
        else {
            OpenGameListScene();
            StageSaveData tempStageData = Save.instance.SaveData;
            if (majorStageIndex == 0) {
                tempStageData.stage1[minorStageIndex] = true;
            }
            else if (majorStageIndex == 1) {
                tempStageData.stage2[minorStageIndex] = true;
            }
            else if (minorStageIndex == 2) {
                tempStageData.stage3[minorStageIndex] = true;
            }
            else {
                tempStageData.stage4[minorStageIndex] = true;
            }
            Save.instance.SaveData = tempStageData;
            Save.instance.MakeMultiSave();
        }
        canvas[0].gameObject.SetActive(false);
        canvas[1].gameObject.SetActive(true);
    }

    public void OpenGame(int index) {
        minorStageIndex = index;
        if (isLocalGame) {
            AllCloseUI();
            SceneManager.LoadScene($"Game_{majorStageIndex + 1}-{minorStageIndex + 1}");
        }
        else {
            AllCloseUI();
            OpenGameScene();
        }
    }

    private void OpenGameScene() {
        var roomManager = NetworkManager.singleton as RoomManager;
        if (RoomManager.ConnectedPlayer < roomManager.minPlayers) return;

        foreach (RoomPlayer player in roomManager.roomSlots)
            player.ReadyStateChanged(false, true);
        roomManager.ServerChangeScene($"Game_{majorStageIndex + 1}-{minorStageIndex + 1}");
    }

    private void OpenGameListScene() {
        var roomManager = NetworkManager.singleton as RoomManager;
        if (RoomManager.ConnectedPlayer < roomManager.minPlayers) return;

        foreach (RoomPlayer player in roomManager.roomSlots)
            player.ReadyStateChanged(false, true);
        roomManager.ServerChangeScene(roomManager.GameListScene);
    }
}
