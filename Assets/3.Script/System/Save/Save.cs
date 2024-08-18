using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StageSaveData {
    // 스테이지 달성 여부 bool
    public bool[] stage1 = new bool[4]; // 현재 1개 Stage만 있음
    public bool[] stage2 = new bool[4]; // 
    public bool[] stage3 = new bool[4]; // 
    public bool[] stage4 = new bool[4]; // 

    // Save 확인용
    //public bool hasSave;
}

[System.Serializable]
public class StageSaveDataList {    // Multi list 저장
    public List<StageSaveData> MultiDatas;
}


public class Save : MonoBehaviour {
    // 1. 목적 : 게임 플레이 저장
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

    // single mode 확인용 
    private bool isSingleMode; //TODO: 모드 선택할때 변경해야함
    public bool GetPlayMode() { return isSingleMode; }
    public void SetPlayMode(bool isSinglePlayMode) { isSingleMode = isSinglePlayMode; }//TODO:[김수주] 방 생성할 경우 모드 확인용 -> 넣어야 저장됨


    private bool isHostPlayer;                                      // 방생성하기를 누른 플레이어만 저장해야함?
    public void SetHostPlayer() { isHostPlayer = true; }            //TODO:[김수주] 방 생성할 경우 호스트 확인용 -> 넣어야 저장됨

    // 경로 저장
    public StageSaveData SaveData = new StageSaveData();

    private string SingleplayerSaveJsonFilePath;
    private string MultiplayerSaveJsonFilePath;

    
    private void Start() {
        SingleplayerSaveJsonFilePath = Path.Combine(Application.persistentDataPath, "Save/SingleSaveData.json");
        if (!Directory.Exists(Path.GetDirectoryName(SingleplayerSaveJsonFilePath))) {
            Directory.CreateDirectory(Path.GetDirectoryName(SingleplayerSaveJsonFilePath));
        }
        Debug.Log("single 파일 위치 : " + SingleplayerSaveJsonFilePath);

        MultiplayerSaveJsonFilePath = Path.Combine(Application.persistentDataPath, "Save/MultiSaveData.json");
        if (!Directory.Exists(Path.GetDirectoryName(MultiplayerSaveJsonFilePath))) {
            Directory.CreateDirectory(Path.GetDirectoryName(MultiplayerSaveJsonFilePath));
        }
        Debug.Log("multy 파일 위치 : " + MultiplayerSaveJsonFilePath);
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
        if (!isHostPlayer) return;      // 방생성하기를 누른 본인이 아니면 저장 안함
                                        
        if (SaveData == null) {         // SaveData가 null일 경우 새 StageSaveData 객체 생성
            SaveData = new StageSaveData();
        }

        StageSaveDataList saveDataList = MultiLoad();
        if (saveDataList == null) {
            Debug.Log("SaveDataList null");

            // 새 StageSaveDataList 객체 초기화
            saveDataList = new StageSaveDataList { MultiDatas = new List<StageSaveData>() };
            File.WriteAllText(MultiplayerSaveJsonFilePath, JsonUtility.ToJson(saveDataList)); 
        }

        if (saveDataList.MultiDatas == null) {
            Debug.Log(saveDataList);
            Debug.Log(saveDataList.MultiDatas);
        }
        GetTargetSaveData(); //TODO: [김수주] 씬 성공할 때마다 자동저장 // 중복제거는 일단 나중에

        saveDataList.MultiDatas.Add(SaveData);

        File.WriteAllText(MultiplayerSaveJsonFilePath, JsonUtility.ToJson(saveDataList));
    }

    // 멀티일 경우 불러오기
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

    // 처음 게임 시작할때
    //TODO: 방생성 새로 만들때 사용해야함
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
 세이브를 내가 하는게 맞는지 모르겠군 일단일단일단
 1. 목적 : 게임 플레이 저장 => singleton
 2. 내용
    2-1. 싱글, 멀티 구분 X => 방생성 주체만 저장
    2-2. 처리 내용
        1. 저장 경로 생성
        2. 저장
            2-1. 저장 요소
                1) stages key -> bool[]
            2-2. 저장 시점 : 씬 성공시 자동 저장됨
            2-3. 저장 장소 : local 
======================================================
1. 방생성하기를 누르면 isHostPlayer = true : 저장
2. 세이브 파일 생성
 */