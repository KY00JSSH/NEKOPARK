using UnityEngine;
using UnityEngine.UI;

public class SaveDataButtonController : MonoBehaviour{
    private Text[] texts;
    private string thisKey = string.Empty;
    private StageSaveData thisSaveData = null;

    private void Awake() {
        texts = GetComponentsInChildren<Text>();
    }

    public void SetLoadDataText(string key, StageSaveData saveData) {
        texts[0].text = key;

        thisSaveData = saveData;
        thisKey = key;

        int clearStageNum = 0;
        int notClearStageNum = 0;

        for (int i = 0; i < saveData.stage1.Length; i++) {
            if (saveData.stage1[i]) {
                clearStageNum++;
            }
            else {
                notClearStageNum++;
            }
        }

        for (int i = 0; i < saveData.stage2.Length; i++) {
            if (saveData.stage2[i]) {
                clearStageNum++;
            }
            else {
                notClearStageNum++;
            }
        }

        for (int i = 0; i < saveData.stage3.Length; i++) {
            if (saveData.stage3[i]) {
                clearStageNum++;
            }
            else {
                notClearStageNum++;
            }
        }

        for (int i = 0; i < saveData.stage4.Length; i++) {
            if (saveData.stage4[i]) {
                clearStageNum++;
            }
            else {
                notClearStageNum++;
            }
        }

        texts[1].text = $"{clearStageNum}/{notClearStageNum}";
    }

    public void ClickLoadButton() {
        LoadDataManager.instance.SetLoadData(thisSaveData);
    }
}
