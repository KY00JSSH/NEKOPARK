using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum StageNum { // 씬 식별 테스트
    stage1,
    stage2,
    stage3,
    stage4
}

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
    //private Object_NextStageController Object_NextStage;
    //private bool[] nowStage = new bool[4];
    

    //private void Update() {
    //    if (Object_NextStageController.IsStageClear) {
    //        Object_NextStage = FindObjectOfType<Object_NextStageController>();
    //        if (Object_NextStage != null) {
    //            nowStage = GetStageClear(Object_NextStage.StageNum);  //
    //            SaveStage1Clear(nowStage);
    //        }
    //    }
    //}

    public void Temp_OnButtonClick() {
        bool[] vs = new bool[4];

        SaveStage1Clear(vs);
    }

    public void SaveStage1Clear( bool[] StageClearFlag) {
        Debug.Log("Save stage Clear");
        //Save.instance.SetHostPlayer();
        for (int i = 0; i < StageClearFlag.Length; i++) {
            Save.instance.SaveData.stage1[i] = StageClearFlag[i];
            Debug.Log("save stage1 Clear" + Save.instance.SaveData.stage1[i]);
        }
        Save.instance.MakeSave();
    }

    public bool[] GetStageClear(StageNum stageNum) {
        switch (stageNum) {
            case StageNum.stage1:
                return Save.instance.SaveData.stage1;
            case StageNum.stage2:
                return Save.instance.SaveData.stage2;
            case StageNum.stage3:
                return Save.instance.SaveData.stage3;
            case StageNum.stage4:
                return Save.instance.SaveData.stage4;
        }

        return null;
    }

}


/*
 1. 씬 선택된 스테이지확인해서 
2. 해당 씬의 클리어를 
 
 */