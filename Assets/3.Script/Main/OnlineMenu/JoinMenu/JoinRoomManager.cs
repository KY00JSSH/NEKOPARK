using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JoinRoomManager : MonoBehaviour {
    private Button[] roomButtons;
    private Text statusText;
    private JoinGameManager joinGameManager;
    private int selectRoomIndex;

    private void Awake() {
        roomButtons = GetComponentsInChildren<Button>();
        statusText = GetComponentInChildren<Text>();
        joinGameManager = FindObjectOfType<JoinGameManager>();
    }

    private void Start() {
        statusText.gameObject.SetActive(true);
        for (int i = 0; i < roomButtons.Length; i++) {
            roomButtons[i].gameObject.SetActive(false);
        }
    }

    public void OpenRoomCount(List<RoomData> data) {
        if (data.Count == 0) {
            statusText.gameObject.SetActive(true);
            for (int i = 0; i < roomButtons.Length; i++) {
                roomButtons[i].gameObject.SetActive(false);
            }
        }
        else {
            statusText.gameObject.SetActive(false);
            for (int i = 0; i < roomButtons.Length; i++) {
                if (i < data.Count) {
                    roomButtons[i].gameObject.SetActive(true);
                    roomButtons[i].gameObject.GetComponent<JoinRoomController>().SetRoomData(data[i]);
                }
                else {
                    roomButtons[i].gameObject.SetActive(false);
                }
            }
        }
    }

    public RoomData GetButtonRoomData() {
        return roomButtons[selectRoomIndex].gameObject.GetComponent<JoinRoomController>().GetSelectRoomData();
    }

    public void OpenSelectColorModal(int num) {
        TCPclient.Instance.SetSelectRoomIndex(num);
        requsetSelectColor();
    }

    public void SetSelectRoomIndex(int num) {
        selectRoomIndex = num;
    }

    public int GetSelectRoomIndex() {
        return selectRoomIndex;
    }

    private void requsetSelectColor() {
        string responseColor = TCPclient.Instance.SendRequest(RequestType.Select);
        joinGameManager.OpenSelectColorModal();

        ColorList colorData = JsonUtility.FromJson<ColorList>(responseColor);
        joinGameManager.SetSelectColorList(colorData.colorList);
    }
}
