using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[System.Serializable]
public class StageSaveData {
    // �������� �޼� ���� bool
    public bool[] stage1 = new bool[4]; // ���� 1�� Stage�� ����
    public bool[] stage2 = new bool[4]; // 
    public bool[] stage3 = new bool[4]; // 
    public bool[] stage4 = new bool[4]; // 
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

    public StageSaveData SaveData = new StageSaveData();

    public Dictionary<string, StageSaveData> MultiSaveData { get; private set; }

    private bool doesMultiSaveDataExist;
    // ��� ����
    private string multiplayerSaveDirectory;

    private string SingleplayerSaveJsonFilePath;
    private string MultiplayerSaveJsonFilePath;

    private void Start() {
        // �̱� �÷��̾� ���� ���� ��� ���� : ���Ϸ� ����
        SingleplayerSaveJsonFilePath = Path.Combine(Application.persistentDataPath, "SingleSave/SingleSaveData.json");
        if (!Directory.Exists(Path.GetDirectoryName(SingleplayerSaveJsonFilePath))) {
            Directory.CreateDirectory(Path.GetDirectoryName(SingleplayerSaveJsonFilePath));
        }
        Debug.Log("single ���� ��ġ : " + SingleplayerSaveJsonFilePath);

        // ��Ƽ�÷��̾� ���� ���� ��� ����
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
        if (!isHostPlayer) return;      // ������ϱ⸦ ���� ������ �ƴϸ� ���� ����

        if (SaveData == null) {         // SaveData�� null�� ��� �� StageSaveData ��ü ����
            SaveData = new StageSaveData();
        }

        StageSaveData saveData = MultiFileCheck();
        if (saveData == null) {
            CreateMultiFile();
        }
        else {
            if (!doesMultiSaveDataExist) {                                                           // ��Ƽ���Ͽ��� ���ϴ� ������ �ƴϸ� ����            
                CreateMultiFile();
            }
        }

        File.WriteAllText(MultiplayerSaveJsonFilePath, JsonUtility.ToJson(SaveData));
    }

    //TODO:[�����] ��Ƽ�� ��� �ҷ����� ! multi�� �������� ��� ó������ ����ϴ� �޼ҵ�
    // ��Ƽ ������ �����ϰ� string ���ϸ����� ������ �ִ��� Ȯ��
    public bool SetMultiSaveDataExistCheck(string fileDataTime) {

        string saveFileName = $"MultiSaveData_{fileDataTime}.json";

        MultiplayerSaveJsonFilePath = Path.Combine(multiplayerSaveDirectory, saveFileName);

        // ���� ���� ���θ� Ȯ��
        if (File.Exists(MultiplayerSaveJsonFilePath)) {
            doesMultiSaveDataExist = true;
            SaveData = JsonUtility.FromJson<StageSaveData>(File.ReadAllText(MultiplayerSaveJsonFilePath)); // ������ �����
            
            //Debug.Log("Save Data ����� Ȯ�� ");
            //GetTargetSaveData(); // �����
        }
        else {
            doesMultiSaveDataExist = false;
        }

        return doesMultiSaveDataExist;
    }


    // ��Ƽ �ű� ����
    public void CreateMultiFile() {
        string dateTimeString = DateTime.Now.ToString("yyyy-MM-dd_HH-mm");                              
        string saveFileName = $"MultiSaveData_{dateTimeString}.json";

        MultiplayerSaveJsonFilePath = Path.Combine(multiplayerSaveDirectory, saveFileName);
        if (!File.Exists(MultiplayerSaveJsonFilePath)) {
            File.WriteAllText(MultiplayerSaveJsonFilePath, JsonUtility.ToJson(SaveData));
        }
    }

    //���� ��ο� ������ �ִ���Ȯ��
    public StageSaveData MultiFileCheck() {
        if (File.Exists(MultiplayerSaveJsonFilePath)) {
            return JsonUtility.FromJson<StageSaveData>(File.ReadAllText(MultiplayerSaveJsonFilePath));
        }
        return null;
    }

    public Dictionary<string, StageSaveData> LoadMultiFiles() {
        MultiSaveData.Clear(); 

        if (Directory.Exists(multiplayerSaveDirectory)) {
            string[] files = Directory.GetFiles(multiplayerSaveDirectory, "*.json");    // �ش� ��� ��ü ���� ������

            foreach (string file in files) {                                            // �� ������ �о� StageSaveData�� ��ȯ�� �� ��ųʸ��� �߰�
                string fileContents = File.ReadAllText(file);
                StageSaveData saveData = JsonUtility.FromJson<StageSaveData>(fileContents);
                string fileName = Path.GetFileName(file);
                MultiSaveData[fileName] = saveData;  // ���ϸ��� Ű��, �����͸� ������ ����
            }
        }

        return MultiSaveData;
    }
    /*
     ��Ƽ
    1. load�Ҷ� ���� �̸����� ã�� : �Ű����� ��Ʈ����
    2. ������ ���ο� ������ �����ϱ�
    3. ������ �ش� ���� �����
     
     */

    // ========================================================================
    public void GetTargetSaveData() {   // �����
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