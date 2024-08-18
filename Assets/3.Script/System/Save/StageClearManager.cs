using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageClearManager : MonoBehaviour
{
    // 스테이지가 클리어 됬을 경우
    public static StageClearManager instance = null;
    private void Awake() {
        if(instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else {
            Destroy(gameObject);
        }
    }

    public void Temp_OnButtonClick() {
        bool[] vs = new bool[4];
        vs[2] = true;
        SaveStage1Clear(vs);
    }

    public void SaveStage1Clear(bool[] StageClearFlag) {
        Debug.Log("save stage Clear");
        Save.instance.SetHostPlayer();
        for (int i = 0; i < StageClearFlag.Length; i++) {
            Save.instance.MultiSaveData.stage1[i] = StageClearFlag[i];
            Debug.Log("save stage1 Clear" + Save.instance.MultiSaveData.stage1[i]);
        }
        Save.instance.MakeSave();
    }
}
