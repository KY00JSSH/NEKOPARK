using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JoinRoomController : MonoBehaviour
{
    private Text[] childTexts;
    //ID, CurrentCount, Slash, MaxCount, GameMode, GameStatus

    private void Awake() {
        childTexts = GetComponentsInChildren<Text>();
    }
   
    public void RoomTextSetting(RoomData data) {
        foreach (Text child in childTexts) {
            if (child.name.Equals("ID")) {
                child.text = data.hostName;
            }else if (child.name.Equals("CurrentCount")) {
                child.text = data.currentConnected.ToString();
            }else if (child.name.Equals("MaxCount")) {
                child.text = data.maxConnected.ToString();
            }else if (child.name.Equals("GameMode")) {
                child.text = data.gameType.ToString();
            }else if (child.name.Equals("GameStatus")) {
                if (data.isStart) {
                    child.text = "IN GAME";
                    gameObject.GetComponent<Button>().interactable = false;
                }
                else {
                    child.text = "IN LOBBY";
                }
            }
        }
    }
}
