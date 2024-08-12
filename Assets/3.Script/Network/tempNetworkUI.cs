using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class tempNetworkUI : MonoBehaviour {
    // 임시로 네트워크 기능 테스트하기 위해서
    // 버튼 할당 메서드를 작성한 스크립트

    public void OnHostButtonClicked() {
        // 게임 방 만들기 버튼에 할당되는 메서드
        CreateHostRoom();
    }

    private void CreateHostRoom() {
        var roomManager = RoomManager.singleton;
        //TODO: 방 설정 메서드 처리 필요

        roomManager.StartHost();
    }

    public void OnJoinButtonClicked() {
        JoinRoom();
    }

    private void JoinRoom() {
        var roomManager = RoomManager.singleton;
        roomManager.StartClient();
    }

}