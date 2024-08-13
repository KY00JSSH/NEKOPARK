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
        // ȣ��Ʈ�� ���� ����� �޼����Դϴ�.
        var roomManager = NetworkManager.singleton as RoomManager;

        roomManager.minPlayers = 2;
        roomManager.maxConnections = 5; //TODO: �ִ��ο� ���� �� �ʿ�
        roomManager.SetRoomPassword();  //TODO: �� ��й�ȣ ���� �� �ʿ�

        //TODO: �� ���� �޼��� ó�� �ʿ�
        roomManager.StartHost();
    }

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