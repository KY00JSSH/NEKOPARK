using UnityEngine;
using System.Collections;
using Mirror;
using System.Collections.Generic;
using Org.BouncyCastle.Ocsp;

public class HostCreateController : MonoBehaviour
{
    private void Start() {
        CloseCreateLoading();
    }

    public void OpenCreateLoading() {
        gameObject.SetActive(true);
        StartCoroutine(delayStart());
    }

    public void CloseCreateLoading() {
        gameObject.SetActive(false);
    }

    private void compeleteCreate() {
        CreateHostRoom();
    }

    private IEnumerator delayStart() {
        yield return new WaitForSeconds(1f);
        compeleteCreate();
    }

    private void CreateHostRoom() {
        // ȣ��Ʈ�� ���� ����� �޼����Դϴ�.
        var roomManager = NetworkManager.singleton as RoomManager;

        roomManager.minPlayers = NetworkManager.singleton.DebuggingOverride ? 1 : 2;

        int maxPlayerCount = PlayerPrefs.GetInt("MaxPlayer");
        roomManager.maxConnections = maxPlayerCount;
        roomManager.SetRoomPassword();  //TODO: �� ��й�ȣ ���� �� �ʿ�

        //TODO: �� ���� �޼��� ó�� �ʿ�
        TCPclient.Instance.SendRequest(RequestType.Create);
        roomManager.StartHost();
    }
}
