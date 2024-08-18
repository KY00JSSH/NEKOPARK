using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StageSaveData {
    // �������� �޼� ���� bool
    public bool[] stage1 = new bool[4]; // ���� 1�� Stage�� ����
    public bool[] stage2 = new bool[4]; // 
    public bool[] stage3 = new bool[4]; // 
    public bool[] stage4 = new bool[4]; // 

    // Save Ȯ�ο�
    //public bool hasSave;
}

[System.Serializable]
public class StageSaveDataList {    // Multi list ����
    public List<StageSaveData> MultiDatas;
}


public class Save : MonoBehaviour {
    // 1. ���� : ���� �÷��� ����
    public static Save instance = null;

    private void Awake() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
            InitSaveFile();
        }
        else {
            Destroy(gameObject);
        }
    }

    // single mode Ȯ�ο� 
    private bool isSingleMode; //TODO: ��� �����Ҷ� �����ؾ���
    public bool GetPlayMode() { return isSingleMode; }
    public void SetPlayMode(bool isSinglePlayMode) { isSingleMode = isSinglePlayMode; }//TODO:[�����] �� ������ ��� ��� Ȯ�ο� -> �־�� �����


    private bool isHostPlayer;                                      // ������ϱ⸦ ���� �÷��̾ �����ؾ���?
    public void SetHostPlayer() { isHostPlayer = true; }            //TODO:[�����] �� ������ ��� ȣ��Ʈ Ȯ�ο� -> �־�� �����

    // ��� ����
    public StageSaveData SaveData = new StageSaveData();

    private string SingleplayerSaveJsonFilePath;
    private string MultiplayerSaveJsonFilePath;

    
    private void Start() {
        SingleplayerSaveJsonFilePath = Path.Combine(Application.persistentDataPath, "Save/SingleSaveData.json");
        if (!Directory.Exists(Path.GetDirectoryName(SingleplayerSaveJsonFilePath))) {
            Directory.CreateDirectory(Path.GetDirectoryName(SingleplayerSaveJsonFilePath));
        }
        Debug.Log("single ���� ��ġ : " + SingleplayerSaveJsonFilePath);

        MultiplayerSaveJsonFilePath = Path.Combine(Application.persistentDataPath, "Save/MultiSaveData.json");
        if (!Directory.Exists(Path.GetDirectoryName(MultiplayerSaveJsonFilePath))) {
            Directory.CreateDirectory(Path.GetDirectoryName(MultiplayerSaveJsonFilePath));
        }
        Debug.Log("multy ���� ��ġ : " + MultiplayerSaveJsonFilePath);
    }
    

    public void MakeSave() {
        if (isSingleMode) {
            MakeSingleSave();
        }
        else {
            MakeMultiSave();
        }
    }


    // Single Save =============================================================
    public void MakeSingleSave() {
        if (SaveData == null) {
            SaveData = new StageSaveData();
        }

        GetTargetSaveData();

        File.WriteAllText(SingleplayerSaveJsonFilePath, JsonUtility.ToJson(SaveData));
    }

    // Multi Save =============================================================
    public void MakeMultiSave() {
        if (!isHostPlayer) return;      // ������ϱ⸦ ���� ������ �ƴϸ� ���� ����
                                        
        if (SaveData == null) {         // SaveData�� null�� ��� �� StageSaveData ��ü ����
            SaveData = new StageSaveData();
        }

        StageSaveDataList saveDataList = MultiLoad();
        if (saveDataList == null) {
            Debug.Log("SaveDataList null");

            // �� StageSaveDataList ��ü �ʱ�ȭ
            saveDataList = new StageSaveDataList { MultiDatas = new List<StageSaveData>() };
            File.WriteAllText(MultiplayerSaveJsonFilePath, JsonUtility.ToJson(saveDataList)); 
        }

        if (saveDataList.MultiDatas == null) {
            Debug.Log(saveDataList);
            Debug.Log(saveDataList.MultiDatas);
        }
        GetTargetSaveData(); //TODO: [�����] �� ������ ������ �ڵ����� // �ߺ����Ŵ� �ϴ� ���߿�

        saveDataList.MultiDatas.Add(SaveData);

        File.WriteAllText(MultiplayerSaveJsonFilePath, JsonUtility.ToJson(saveDataList));
    }

    // ��Ƽ�� ��� �ҷ�����
    public StageSaveDataList MultiLoad() {
        if (File.Exists(MultiplayerSaveJsonFilePath)) {
            return JsonUtility.FromJson<StageSaveDataList>(File.ReadAllText(MultiplayerSaveJsonFilePath));
        }
        return null;
    }


    // ========================================================================
    public void GetTargetSaveData() {
        Debug.Log("=================");
        for (int i = 0; i < SaveData.stage1.Length; i++) {
            Debug.Log("save stage1 Clear" + SaveData.stage1[i]);
        }
    }

    // ó�� ���� �����Ҷ�
    //TODO: ����� ���� ���鶧 ����ؾ���
    public void InitSaveFile() {
        for (int i = 0; i < SaveData.stage1.Length; i++) {
            SaveData.stage1[i] = false;
        }
        for (int i = 0; i < SaveData.stage2.Length; i++) {
            SaveData.stage2[i] = false;
        }
        for (int i = 0; i < SaveData.stage3.Length; i++) {
            SaveData.stage3[i] = false;
        }
        for (int i = 0; i < SaveData.stage4.Length; i++) {
            SaveData.stage4[i] = false;
        }
    }
}

/*  
 ���̺긦 ���� �ϴ°� �´��� �𸣰ڱ� �ϴ��ϴ��ϴ�
 1. ���� : ���� �÷��� ���� => singleton
 2. ����
    2-1. �̱�, ��Ƽ ���� X => ����� ��ü�� ����
    2-2. ó�� ����
        1. ���� ��� ����
        2. ����
            2-1. ���� ���
                1) stages key -> bool[]
            2-2. ���� ���� : �� ������ �ڵ� �����
            2-3. ���� ��� : local 
======================================================
1. ������ϱ⸦ ������ isHostPlayer = true : ����
2. ���̺� ���� ����
 */