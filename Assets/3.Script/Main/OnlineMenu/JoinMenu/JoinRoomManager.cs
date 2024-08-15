using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JoinRoomManager : MonoBehaviour {
    private Button[] roomButtons;
    private Text statusText;

    private void Awake() {
        roomButtons = GetComponentsInChildren<Button>();
        statusText = GetComponentInChildren<Text>();
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
        }
        else {
            statusText.gameObject.SetActive(false);
            for (int i = 0; i < data.Count; i++) {
                roomButtons[i].gameObject.SetActive(true);
                roomButtons[i].gameObject.GetComponent<JoinRoomController>().RoomTextSetting(data[i]);
            }
        }
    }
}
