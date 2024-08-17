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
        gameObject.SetActive(false);
        compeleteCreate();
    }

    private void CreateHostRoom() {
        // ȣ��Ʈ�� ���� ����� �޼����Դϴ�.
        var roomManager = NetworkManager.singleton as RoomManager;

        roomManager.minPlayers = NetworkManager.singleton.DebuggingOverride ? 1 : 2;

        int maxPlayerCount = PlayerPrefs.GetInt("MaxPlayer");
        roomManager.maxConnections = maxPlayerCount;
        roomManager.SetRoomPassword();  //TODO: �� ��й�ȣ ���� �� �ʿ�

        //FindObjectOfType<PlayerColorSetting>().playerColor = (PlayerColorType)PlayerPrefs.GetInt("HostColor");

        //TODO: �� ���� �޼��� ó�� �ʿ�
        roomManager.SetNetworkAddress();
        roomManager.StartHost();
        TCPclient.Instance.SendRequest(RequestType.Create);
    }
}
