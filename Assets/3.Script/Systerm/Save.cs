using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleSaveData {   // 싱글은 단일 저장
    // 스테이지 달성 여부 bool
    public bool[] stage1 = new bool[4]; // 전홍현
    public bool[] stage2 = new bool[4]; // 박지훈
    public bool[] stage3 = new bool[4]; // 김수진
    public bool[] stage4 = new bool[4]; // 김수주
}

public class MultiSaveData {
    // 스테이지 달성 여부 bool
    public bool[] stage1 = new bool[4]; // 전홍현
    public bool[] stage2 = new bool[4]; // 박지훈
    public bool[] stage3 = new bool[4]; // 김수진
    public bool[] stage4 = new bool[4]; // 김수주

    // Multi Save 확인용
    public bool hasSave;
}
public class MultiSaveDataList {    // multi list 저장
    public List<MultiSaveDataList> MultiDataList;
}


public class Save : MonoBehaviour {
    // 1. 목적 : 게임 플레이 저장
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

    // single mode 확인용 
    private  bool isSingleMode; //TODO: 모드 선택할때 변경해야함
    public bool GetPlayMode() { return isSingleMode; }

    // 경로 저장
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
 세이브를 내가 하는게 맞는지 모르겠군 일단일단일단
 1. 목적 : 게임 플레이 저장 => singleton
 2. 내용
    2-1. 싱글, 멀티 구분 => 버튼으로 구분?
    2-2. 처리 내용
        1. 저장 경로 생성
        2. 저장
            2-1. 저장 요소
                1) stages key -> bool[]
                2) Invitation Code -> 초대코드 : 멀티일 경우는 들어가기 위한 방검색이 필요함
            2-2. 저장 시점 : 주기적으로 저장됨
            2-3. 저장 장소 : local 
 */