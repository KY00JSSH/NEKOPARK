using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class tempNetworkUI : MonoBehaviour {
    // �ӽ÷� ��Ʈ��ũ ��� �׽�Ʈ�ϱ� ���ؼ�
    // ��ư �Ҵ� �޼��带 �ۼ��� ��ũ��Ʈ

    public void OnHostButtonClicked() {
        // ���� �� ����� ��ư�� �Ҵ�Ǵ� �޼���
        CreateHostRoom();
    }

    private void CreateHostRoom() {
        var roomManager = RoomManager.singleton;
        //TODO: �� ���� �޼��� ó�� �ʿ�

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