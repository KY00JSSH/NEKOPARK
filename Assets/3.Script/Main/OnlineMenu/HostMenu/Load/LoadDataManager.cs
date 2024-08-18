using UnityEngine;

public class LoadDataManager : MonoBehaviour
{
    public static LoadDataManager instance = null;
    private bool isSingle = true;
    public bool IsSingle { get { return isSingle; } }
    public void SetIsSingleMode(bool yn) { isSingle = yn; }

    private bool[,] stageData = new bool[4, 4];
    public bool[,] StageData { get { return stageData; } }

    private void Awake() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else {
            Destroy(gameObject);
            return;
        }
    }

    private void Start() {
        stageDataiInit();
    }

    private void stageDataiInit() {
        for (int i = 0; i < 4; i++) {
            for (int j = 0; j < 4; j++) {
                stageData[i, j] = false;
            }
        }
    }

    public void SetLoadData(StageSaveData saveData) {
        for (int i = 0; i < saveData.stage1.Length; i++) {
            if (saveData.stage1[i]) {
                stageData[0,i] = saveData.stage1[i];
            }
        }
        for(int i = 0;i < saveData.stage2.Length; i++) {
            if (saveData.stage2[i]) {
                stageData[1, i] = saveData.stage1[i];
            }
        }
        for (int i = 0; i < saveData.stage3.Length; i++) {
            if (saveData.stage3[i]) {
                stageData[2, i] = saveData.stage1[i];
            }
        }
        for (int i = 0; i < saveData.stage4.Length; i++) {
            if (saveData.stage4[i]) {
                stageData[3, i] = saveData.stage1[i];
            }
        }
    }

    public void ClearStage(int majorIndex, int minorIndex) {
        stageData[majorIndex, minorIndex] = true;
    }
}
