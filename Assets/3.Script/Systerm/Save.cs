using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleSaveData {   // �̱��� ���� ����
    // �������� �޼� ���� bool
    public bool[] stage1 = new bool[4]; // ��ȫ��
    public bool[] stage2 = new bool[4]; // ������
    public bool[] stage3 = new bool[4]; // �����
    public bool[] stage4 = new bool[4]; // �����
}

public class MultiSaveData {
    // �������� �޼� ���� bool
    public bool[] stage1 = new bool[4]; // ��ȫ��
    public bool[] stage2 = new bool[4]; // ������
    public bool[] stage3 = new bool[4]; // �����
    public bool[] stage4 = new bool[4]; // �����

    // Multi Save Ȯ�ο�
    public bool hasSave;
}
public class MultiSaveDataList {    // multi list ����
    public List<MultiSaveDataList> MultiDataList;
}


public class Save : MonoBehaviour {
    // 1. ���� : ���� �÷��� ����
    private static Save instance = null;

    private void Awake() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else {
            Destroy(gameObject);
        }
    }

    // single mode Ȯ�ο� 
    private  bool isSingleMode; //TODO: ��� �����Ҷ� �����ؾ���
    public bool GetPlayMode() { return isSingleMode; }

    // ��� ����
    public SingleSaveData singleSaveData = new SingleSaveData();
    public MultiSaveData multiSaveData = new MultiSaveData();

    private string playerSaveJsonFilePath;

    /*
    private void Start() {
        playerSaveJsonFilePath = Path.Combine(Application.persistentDataPath, "Save/singleSaveData.json");
        if (!Directory.Exists(Path.GetDirectoryName(playerSaveJsonFilePath))) {
            Directory.CreateDirectory(Path.GetDirectoryName(playerSaveJsonFilePath));
        }

        playerSaveJsonFilePath = Path.Combine(Application.persistentDataPath, "Save/multiSaveData.json");
        if (!Directory.Exists(Path.GetDirectoryName(playerSaveJsonFilePath))) {
            Directory.CreateDirectory(Path.GetDirectoryName(playerSaveJsonFilePath));
        }
    }
    */

    


}

/*  
 ���̺긦 ���� �ϴ°� �´��� �𸣰ڱ� �ϴ��ϴ��ϴ�
 1. ���� : ���� �÷��� ���� => singleton
 2. ����
    2-1. �̱�, ��Ƽ ���� => ��ư���� ����?
    2-2. ó�� ����
        1. ���� ��� ����
        2. ����
            2-1. ���� ���
                1) stages key -> bool[]
                2) Invitation Code -> �ʴ��ڵ� : ��Ƽ�� ���� ���� ���� ��˻��� �ʿ���
            2-2. ���� ���� : �ֱ������� �����
            2-3. ���� ��� : local 
 */