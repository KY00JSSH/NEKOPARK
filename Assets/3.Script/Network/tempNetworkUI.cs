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
}