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

        //SaveStage1Clear(vs);
        bool[,] _vs = new bool[3, 4];

        _vs[1, 1] = true;
        _vs[1, 3] = true;
        _vs[2, 2] = true;
        SaveStageAllClear(_vs);

        //Save.instance.SetPlayMode(false);
        //Save.instance.SetHostPlayer();
        //Save.instance.SetMultiSaveDataExistCheck("2024-08-18_14-40");
    }


    // stage 1만 사용할 경우 그냥 바로 넣으면 됨 : 현재 스테이지 1밖에 없음
    public void SaveStage1Clear( bool[] StageClearFlag) {
        Debug.Log("Save stage Clear");
        //Save.instance.SetHostPlayer();

        for (int i = 0; i < StageClearFlag.Length; i++) {
            Save.instance.SaveData.stage1[i] = StageClearFlag[i];
            Debug.Log("save stage1 Clear" + Save.instance.SaveData.stage1[i]);
        }

        Save.instance.MakeSave();
    }


    //TODO: [김수주] 전체 bool 값 넣어서 저장하는 경우
    public void SaveStageAllClear(bool[,] StageClearFlag) {
        Debug.Log("Save stage Clear");
        
        for (int i = 0; i < StageClearFlag.GetLength(0); i++) {                         // 첫 번째 차원을 기준으로 열거형 StageNum 값에 해당하는 bool[] 배열을 가져옴
            StageNum stageNum = (StageNum)i;                                            // enum의 숫자 값을 기반으로 StageNum에 대응
           
            for (int j = 0; j < StageClearFlag.GetLength(1); j++) {                     // 두 번째 차원의 값을 해당 stageNum에 대응하는 bool[] 배열에 저장
                GetStageClear(stageNum)[j] = StageClearFlag[i, j];
                //Debug.Log("save stage Clear for " + stageNum + " at index " + j + ": " + Save.instance.SaveData.stage1[j]);
            }
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
2. 해당 씬의 클리어-> 스테이지 bool값 전체 변경
 
 */