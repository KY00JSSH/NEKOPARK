using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;

public class JoinRoomController : MonoBehaviour
{
    //ID, CurrentCount, Slash, MaxCount, GameMode, GameStatus
    private Text[] childTexts;
    private Image hostImage;

    private RoomData roomData;

    private void Awake() {
        childTexts = GetComponentsInChildren<Text>();
        Image[] images = GetComponentsInChildren<Image>();

        foreach (Image Image in images) {
            if (Image.name.Contains("Player")) {
                hostImage = Image;
            }
        }
    }
   
    public void SetRoomData(RoomData data) {
        roomData = data;
        roomValueSetting();
    }

    public void roomValueSetting() {
        foreach (Text child in childTexts) {
            if (child.name.Equals("ID")) {
                child.text = roomData?.hostName;
            }
            else if (child.name.Equals("CurrentCount")) {
                child.text = roomData?.currentConnected.ToString();
            }
            else if (child.name.Equals("MaxCount")) {
                child.text = roomData?.maxConnected.ToString();
            }
            else if (child.name.Equals("GameMode")) {
                child.text = roomData?.gameType.ToString();
            }
            else if (child.name.Equals("GameStatus")) {
                if (roomData.isStart) {
                    child.text = "IN GAME";
                    gameObject.GetComponent<Button>().interactable = false;
                }
                else {
                    child.text = "IN LOBBY";
                }
            }
        }
        hostImage.color = PlayerColor.GetColor(roomData.hostColor);
    }

    public RoomData GetSelectRoomData() {
        return roomData;
    }
}
