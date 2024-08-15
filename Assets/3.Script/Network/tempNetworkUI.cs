using UnityEngine;
using Mirror;

public class tempNetworkUI : MonoBehaviour {
    // �ӽ÷� ��Ʈ��ũ ��� �׽�Ʈ�ϱ� ���ؼ�
    // ��ư �Ҵ� �޼��带 �ۼ��� ��ũ��Ʈ

    public void OnJoinButtonClicked() {
        // ���� �� ���� ��ư�� �Ҵ�Ǵ� �޼���
        JoinRoom();
    }

    private void JoinRoom() {
        // Ŭ���̾�Ʈ�� �濡 ���� �޼����Դϴ�.
        var roomManager = RoomManager.singleton;
        roomManager.StartClient();
    }

    public void OnExitRoomButtonClicked() {
        // �κ񿡼� �� ������ ��ư�� �Ҵ�Ǵ� �޼���
        var roomManager = RoomManager.singleton;
        if (NetworkServer.active) roomManager.StopHost();
        else roomManager.StopHost();
    }

    public void OnStartGameButtonClicked() {
        // �κ񿡼� ���� ���� ��ư�� �Ҵ�Ǵ� �޼���
        var roomManager = NetworkManager.singleton as RoomManager;
        if (RoomManager.ConnectedPlayer < roomManager.minPlayers) return;

        foreach (RoomPlayer player in roomManager.roomSlots)
            player.ReadyStateChanged(false, true);

        roomManager.ServerChangeScene(roomManager.GameplayScene);
    }
}
