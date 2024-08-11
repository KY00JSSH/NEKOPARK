using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class tempNetworkUI : MonoBehaviour {

    public void OnHostButtonClicked() {
        CreateHostRoom();
    }

    private void CreateHostRoom() {
        var roomManager = RoomManager.singleton;


        roomManager.StartHost();
    }

}