using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[System.Serializable]
public class StageSaveData {
    // 스테이지 달성 여부 bool
    public bool[] stage1 = new bool[4]; // 현재 1개 Stage만 있음
    public bool[] stage2 = new bool[4]; // 
    public bool[] stage3 = new bool[4]; // 
    public bool[] stage4 = new bool[4]; // 
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

    public StageSaveData SaveData = new StageSaveData();

    public Dictionary<string, StageSaveData> MultiSaveData { get; private set; }

    private bool doesMultiSaveDataExist;
    // 경로 저장
    private string multiplayerSaveDirectory;

    private string SingleplayerSaveJsonFilePath;
    private string MultiplayerSaveJsonFilePath;

    private void Start() {
        // 싱글 플레이어 저장 파일 경로 설정 : 단일로 저장
        SingleplayerSaveJsonFilePath = Path.Combine(Application.persistentDataPath, "SingleSave/SingleSaveData.json");
        if (!Directory.Exists(Path.GetDirectoryName(SingleplayerSaveJsonFilePath))) {
            Directory.CreateDirectory(Path.GetDirectoryName(SingleplayerSaveJsonFilePath));
        }
        Debug.Log("single 파일 위치 : " + SingleplayerSaveJsonFilePath);

        // 멀티플레이어 저장 파일 경로 설정
        multiplayerSaveDirectory = Path.Combine(Application.persistentDataPath, "MultiSave");
        if (!Directory.Exists(multiplayerSaveDirectory)) {
            Directory.CreateDirectory(multiplayerSaveDirectory);
        }

        MultiSaveData = new Dictionary<string, StageSaveData>();
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

        File.WriteAllText(SingleplayerSaveJsonFilePath, JsonUtility.ToJson(SaveData));
    }

    // Multi Save =============================================================
    public void MakeMultiSave() {
        if (!isHostPlayer) return;      // 방생성하기를 누른 본인이 아니면 저장 안함

        if (SaveData == null) {         // SaveData가 null일 경우 새 StageSaveData 객체 생성
            SaveData = new StageSaveData();
        }

        StageSaveData saveData = MultiFileCheck();
        if (saveData == null) {
            CreateMultiFile();
        }
        else {
            if (!doesMultiSaveDataExist) {                                                           // 멀티파일에서 원하는 파일이 아니면 생성            
                CreateMultiFile();
            }
        }

        File.WriteAllText(MultiplayerSaveJsonFilePath, JsonUtility.ToJson(SaveData));
    }

    //TODO:[김수주] 멀티일 경우 불러오기 ! multi를 선택했을 경우 처음에만 사용하는 메소드
    // 멀티 파일을 선택하고 string 파일명으로 파일이 있는지 확인
    public bool SetMultiSaveDataExistCheck(string fileDataTime) {

        string saveFileName = $"MultiSaveData_{fileDataTime}.json";

        MultiplayerSaveJsonFilePath = Path.Combine(multiplayerSaveDirectory, saveFileName);

        // 파일 존재 여부를 확인
        if (File.Exists(MultiplayerSaveJsonFilePath)) {
            doesMultiSaveDataExist = true;
            SaveData = JsonUtility.FromJson<StageSaveData>(File.ReadAllText(MultiplayerSaveJsonFilePath)); // 데이터 덮어쓰기
            
            //Debug.Log("Save Data 덮어쓰기 확인 ");
            //GetTargetSaveData(); // 디버깅
        }
        else {
            doesMultiSaveDataExist = false;
        }

        return doesMultiSaveDataExist;
    }


    // 멀티 신규 생성
    public void CreateMultiFile() {
        string dateTimeString = DateTime.Now.ToString("yyyy-MM-dd_HH-mm");                              
        string saveFileName = $"MultiSaveData_{dateTimeString}.json";

        MultiplayerSaveJsonFilePath = Path.Combine(multiplayerSaveDirectory, saveFileName);
        if (!File.Exists(MultiplayerSaveJsonFilePath)) {
            File.WriteAllText(MultiplayerSaveJsonFilePath, JsonUtility.ToJson(SaveData));
        }
    }

    //폴더 경로에 파일이 있는지확인
    public StageSaveData MultiFileCheck() {
        if (File.Exists(MultiplayerSaveJsonFilePath)) {
            return JsonUtility.FromJson<StageSaveData>(File.ReadAllText(MultiplayerSaveJsonFilePath));
        }
        return null;
    }

    public Dictionary<string, StageSaveData> LoadMultiFiles() {
        MultiSaveData.Clear(); 

        if (Directory.Exists(multiplayerSaveDirectory)) {
            string[] files = Directory.GetFiles(multiplayerSaveDirectory, "*.json");    // 해당 경로 전체 파일 들고오기

            foreach (string file in files) {                                            // 각 파일을 읽어 StageSaveData로 변환한 후 딕셔너리에 추가
                string fileContents = File.ReadAllText(file);
                StageSaveData saveData = JsonUtility.FromJson<StageSaveData>(fileContents);
                string fileName = Path.GetFileName(file);
                MultiSaveData[fileName] = saveData;  // 파일명을 키로, 데이터를 값으로 저장
            }
        }

        return MultiSaveData;
    }
    /*
     멀티
    1. load할때 파일 이름으로 찾기 : 매개변수 스트링값
    2. 없으면 새로운 파일을 생성하기
    3. 있으면 해당 파일 덮어쓰기
     
     */

    // ========================================================================
    public void GetTargetSaveData() {   // 디버깅
        Debug.Log("=================");
        for (int i = 0; i < SaveData.stage1.Length; i++) {
            Debug.Log("save stage1 Clear" + SaveData.stage1[i]);
        }
        for (int i = 0; i < SaveData.stage1.Length; i++) {
            Debug.Log("save stage2 Clear" + SaveData.stage2[i]);
        }
        for (int i = 0; i < SaveData.stage1.Length; i++) {
            Debug.Log("save stage3 Clear" + SaveData.stage3[i]);
        }
        for (int i = 0; i < SaveData.stage1.Length; i++) {
            Debug.Log("save stage4 Clear" + SaveData.stage4[i]);
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