using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoinGameManager : MonoBehaviour {
    private MainManager mainManager;
    private JoinButtonHoverController[] joinButtonHovers;
    private JoinRoomManager joinRoomManager;
    private bool[] options = new bool[3] { true, true, true };
    private List<RoomData> data = new List<RoomData>();
    //option - world, battel, endless

    private void Awake() {
        mainManager = FindObjectOfType<MainManager>();
        joinButtonHovers = FindObjectsOfType<JoinButtonHoverController>();
        joinRoomManager = FindObjectOfType<JoinRoomManager>();
    }

    private void Start() {
        //data = JsonUtility.FromJson<List<RoomData>>(FindObjectOfType<TCPclient>().SendRequest(RequestType.Request));
        //TODO: ROOM 갯수 TEST용도
        testDataSetting();
        if (data.Count > 6) {
            List<RoomData> firstPageData = new List<RoomData>();
            for (int i = 0; i < data.Count && i < 6; i++) {
                firstPageData.Add(data[i]);
            }
            joinRoomManager.OpenRoomCount(firstPageData);
        }
        else {
            joinRoomManager.OpenRoomCount(data);
        }
    }

    //TODO: 테스트 데이터 체크
    private void testDataSetting() {
        RoomData test1 = new RoomData();
        test1.isStart = false;
        test1.hostName = "전홍현";
        test1.currentConnected = 3;
        test1.maxConnected = 6;
        RoomData test2 = new RoomData();
        test2.isStart = true;
        test2.hostName = "김수주";
        test2.currentConnected = 2;
        test2.maxConnected = 2;
        data.Add(test1);
        data.Add(test2);
    }

    private void Update() {
        if (Input.GetButtonDown("Cancel")) {
            mainManager.OpenOnlineCanvas();
        }
        
        //TODO: 키보드로 메뉴 이동 추후 추가
    }

    public void CheckOptions(string objName, bool yn) {
        if (objName.Equals("World")) {
            options[0] = yn;
        }else if (objName.Equals("Battle")) {
            options[1] = yn;
        }else {
            options[2] = yn;
        }
    }

    public void GetHoverComponent(string name) {
        foreach (var component in joinButtonHovers) {
            if (component.gameObject.name.Equals(name)) {
                component.EnableImage();
            }
            else {
                component.DisEnableImage();
            }
        }
    }
}
