using UnityEngine;
using Mirror;

public class tempNetworkUI : MonoBehaviour {
    // 임시로 네트워크 기능 테스트하기 위해서
    // 버튼 할당 메서드를 작성한 스크립트

    public void OnJoinButtonClicked() {
        // 게임 방 들어가기 버튼에 할당되는 메서드
        JoinRoom();
    }

    private void JoinRoom() {
        // 클라이언트가 방에 들어가는 메서드입니다.
        var roomManager = RoomManager.singleton;
        roomManager.StartClient();
    }
}