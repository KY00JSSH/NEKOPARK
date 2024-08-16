using Newtonsoft.Json;
using System;
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

    private JoinFailController joinFailController;
    private JoinColorModalController joinColorModalController;

    private void Awake() {
        mainManager = FindObjectOfType<MainManager>();
        joinButtonHovers = FindObjectsOfType<JoinButtonHoverController>();
        joinRoomManager = FindObjectOfType<JoinRoomManager>();
        joinColorModalController = FindObjectOfType<JoinColorModalController>();
        joinFailController = FindObjectOfType<JoinFailController>();
    }

    private void Start() {
        CloseSelectColorModal();
        CloseConnectFailModal(false);
    }

    private bool requestRoomList() {
        try {
            string response = TCPclient.Instance.SendRequest(RequestType.Request);

            RoomList responseRoomData = JsonUtility.FromJson<RoomList>(response);
            data = responseRoomData.roomList;

            return true;
        }
        catch (Exception e) {
            Debug.Log(e.Message);
            return false;
        }
    }

    public bool JoinRoomSetting() {
        if (requestRoomList()) {
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
            return true;
        }
        else {
            return false;
        }
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
        }
        else if (objName.Equals("Battle")) {
            options[1] = yn;
        }
        else {
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

    public void OpenConnectFailModal() {
        CloseSelectColorModal();
        joinFailController.gameObject.SetActive(true);
    }

    public void CloseSelectColorModal() {
        joinColorModalController.gameObject.SetActive(false);
    }

    public void OpenSelectColorModal() {
        joinColorModalController.gameObject.SetActive(true);
    }

    public void CloseConnectFailModal(bool roomReset) {
        joinFailController.gameObject.SetActive(false);
        if(roomReset)
            requestRoomList();
    }

    public void SetSelectColorList(List<PlayerColorType> colorList) {
        FindObjectOfType<JoinColorChangeController>().GetSelectColor(colorList);
    }

    public void UpdateRoomList() {
        bool isGetList = JoinRoomSetting();
        if (!isGetList) {
            OpenConnectFailModal();
        }
    }

    private void OnDisable() {
        data = null;
    }
}
