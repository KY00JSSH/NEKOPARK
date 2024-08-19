using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;

public class GameListManager : NetworkBehaviour
{
    public static GameListManager instance;

    private int majorStageIndex = -1;
    public int MajorStageIndex { get { return majorStageIndex; } }
    public void SetMajorStageIndex(int num) { majorStageIndex = num; }

    private int minorStageIndex = -1;

    public int MinorStageIndex { get { return minorStageIndex; } }
    public void SetMinorStageIndex(int num) { minorStageIndex = num; }

    private bool isLocalGame = false;

    public bool IsLocalGame { get { return isLocalGame; } }
    public void SetIsLocalGame(bool yn) { isLocalGame = yn; }

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
        }
    }

    public void ClearAndOpenList() {
        LoadDataManager.instance.ClearStage(majorStageIndex, minorStageIndex);
        if (isLocalGame) {
            Save.instance.MakeSingleSave();
            SceneManager.LoadScene("Game_List");
        }
        else {
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

            var roomManager = NetworkManager.singleton as RoomManager;
            foreach (RoomPlayer player in roomManager.roomSlots)
                player.ReadyStateChanged(false, true);
            roomManager.ServerChangeScene("Game_List");
        }

        
    }
}
