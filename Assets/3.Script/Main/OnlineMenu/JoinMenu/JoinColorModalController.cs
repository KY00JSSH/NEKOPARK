using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JoinColorModalController : MonoBehaviour {
    private JoinGameManager joinGameManager;
    private JoinFailController joinFailController;

    private void Awake() {
        joinGameManager = FindObjectOfType<JoinGameManager>();
        joinFailController = FindObjectOfType<JoinFailController>();
    }

    public void ConnectRoom() {
        string response = TCPclient.Instance.SendRequest(RequestType.Enter);
        if (response.Equals("EnterFailure")) {
            joinGameManager.OpenConnectFailModal();
            joinFailController.SetDescription(false);
        }
        else if (response.Equals("ColorFailrue")) {
            joinGameManager.OpenConnectFailModal();
            joinFailController.SetDescription(true);
        }
        else {
            JoinRoom();
        }
    }

    private void JoinRoom() {
        var roomManager = NetworkManager.singleton as RoomManager;
        var roomData = FindObjectOfType<JoinRoomManager>().GetButtonRoomData();
        FindObjectOfType<PlayerColorSetting>().CmdSetPlayerColor(roomData.hostColor);
        PlayerPrefs.SetInt("HostColor", (int)roomData.hostColor);
        roomManager.SetNetworkAddress(roomData.hostIP);
        roomManager.StartClient();
    }
}
