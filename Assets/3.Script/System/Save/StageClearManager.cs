using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum StageNum { // �� �ĺ� �׽�Ʈ
    stage1,
    stage2,
    stage3,
    stage4
}

public class StageClearManager : MonoBehaviour
{
    // ���������� Ŭ���� ���� ���
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


    // stage 1�� ����� ��� �׳� �ٷ� ������ �� : ���� �������� 1�ۿ� ����
    public void SaveStage1Clear( bool[] StageClearFlag) {
        Debug.Log("Save stage Clear");
        //Save.instance.SetHostPlayer();

        for (int i = 0; i < StageClearFlag.Length; i++) {
            Save.instance.SaveData.stage1[i] = StageClearFlag[i];
            Debug.Log("save stage1 Clear" + Save.instance.SaveData.stage1[i]);
        }

        Save.instance.MakeSave();
    }


    //TODO: [�����] ��ü bool �� �־ �����ϴ� ���
    public void SaveStageAllClear(bool[,] StageClearFlag) {
        Debug.Log("Save stage Clear");
        
        for (int i = 0; i < StageClearFlag.GetLength(0); i++) {                         // ù ��° ������ �������� ������ StageNum ���� �ش��ϴ� bool[] �迭�� ������
            StageNum stageNum = (StageNum)i;                                            // enum�� ���� ���� ������� StageNum�� ����
           
            for (int j = 0; j < StageClearFlag.GetLength(1); j++) {                     // �� ��° ������ ���� �ش� stageNum�� �����ϴ� bool[] �迭�� ����
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
 1. �� ���õ� ��������Ȯ���ؼ� 
2. �ش� ���� Ŭ����-> �������� bool�� ��ü ����
 
 */