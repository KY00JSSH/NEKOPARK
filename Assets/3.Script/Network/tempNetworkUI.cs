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
        // 호스트가 방을 만드는 메서드입니다.
        var roomManager = NetworkManager.singleton as RoomManager;

        roomManager.minPlayers = 2;
        roomManager.maxConnections = 5; //TODO: 최대인원 설정 값 필요
        roomManager.SetRoomPassword();  //TODO: 방 비밀번호 설정 값 필요

        //TODO: 방 설정 메서드 처리 필요
        roomManager.StartHost();
    }

    public void OnJoinButtonClicked() {
        // 게임 방 들어가기 버튼에 할당되는 메서드
        JoinRoom();
    }

    private void JoinRoom() {
        // 클라이언트가 방에 들어가는 메서드입니다.
        var roomManager = RoomManager.singleton;
        roomManager.StartClient();
    }

    public void OnExitRoomButtonClicked() {
        // 로비에서 방 나가기 버튼에 할당되는 메서드
        var roomManager = RoomManager.singleton;
        if (NetworkServer.active) roomManager.StopHost();
        else roomManager.StopHost();
    }

    public void OnStartGameButtonClicked() {
        // 로비에서 게임 시작 버튼에 할당되는 메서드
        var roomManager = NetworkManager.singleton as RoomManager;
        if (RoomManager.ConnectedPlayer < roomManager.minPlayers) return;

        foreach (RoomPlayer player in roomManager.roomSlots)
            player.ReadyStateChanged(false, true);

        roomManager.ServerChangeScene(roomManager.GameplayScene);
    }

}