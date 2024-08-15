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
        // 호스트가 방을 만드는 메서드입니다.
        var roomManager = NetworkManager.singleton as RoomManager;

        roomManager.minPlayers = NetworkManager.singleton.DebuggingOverride ? 1 : 2;

        int maxPlayerCount = PlayerPrefs.GetInt("MaxPlayer");
        roomManager.maxConnections = maxPlayerCount;
        roomManager.SetRoomPassword();  //TODO: 방 비밀번호 설정 값 필요

        //TODO: 방 설정 메서드 처리 필요
        TCPclient.Instance.SendRequest(RequestType.Create);
        roomManager.StartHost();
    }
}
