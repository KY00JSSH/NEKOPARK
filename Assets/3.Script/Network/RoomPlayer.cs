using UnityEngine;
using Mirror;
using System;
using UnityEngine.SceneManagement;

// �� ��ũ��Ʈ�� Mirror Network�� Room Manager�� �����ϰ�,
// ��Ƽ�÷��� �κ� (Game Room Scene) ���� �÷��̾ ����, �����ϱ� ���� ��ũ��Ʈ�Դϴ�.
// �÷��̾� ĳ������ ��Ʈ��ũ ���� �� �⺻ ���� �����մϴ�.

public class RoomPlayer : NetworkRoomPlayer {
    private static RoomPlayer myRoomPlayer;
    public static RoomPlayer MyRoomPlayer {
        get {
            if(myRoomPlayer == null) {
                var players = FindObjectsOfType<RoomPlayer>();
                foreach(var player in players) 
                    if(player.isOwned) {
                        myRoomPlayer = player;
                        break;
                    }
            }
            return myRoomPlayer;
        }
    }
    [SyncVar] public string Nickname;
    public PlayerColorType playerColor;

    private void Update() {
        if (Input.GetKeyDown(KeyCode.F1)) {
            (NetworkRoomManager.singleton as RoomManager).ServerChangeScene(SceneManager.GetActiveScene().name);
        }
    }
    public override void Start() {
        base.Start();

        playerColor = (PlayerColorType)PlayerPrefs.GetInt("HostColor");
        var lobbyMenuManager = FindObjectOfType<LobbyMenuManager>();
        if (lobbyMenuManager != null) lobbyMenuManager.SetPlayerIconColor();

        if (isServer) SpawnRoomPlayer();
        RoomManager.UpdateConnenctedPlayerCount();
    }
    
    private void OnDestroy() {
        RoomManager.UpdateConnenctedPlayerCount();
    }

    private void SpawnRoomPlayer() {
        // ���� �κ񿡼� �÷��̾ ����� �÷��̾� ĳ���͸� ���� �� Instantiate �մϴ�.
        CheckSpawned();

        Vector3 spawnPosition = GetSpawnPosition();

        var player = Instantiate(RoomManager.singleton.spawnPrefabs[0], spawnPosition, Quaternion.identity);
        player.GetComponent<PlayerColor>().playerColor = PlayerColorType.nullColor;
        player.GetComponent<PlayerColor>().playerColor = playerColor;

        if (CheckSpawned()) {
            Destroy(player);
            Destroy(gameObject);
        }

        var clickEffect = Instantiate(RoomManager.singleton.spawnPrefabs[1]);
        clickEffect.GetComponent<PlayerMouseCommunication>().effectColor = PlayerColorType.nullColor;
        clickEffect.GetComponent<PlayerMouseCommunication>().effectColor = playerColor;

        NetworkServer.Spawn(player, connectionToClient);
        NetworkServer.Spawn(clickEffect, connectionToClient);
    }

    private bool CheckSpawned() {
        var players = FindObjectsOfType<RoomPlayer>();
        int count = 0;
        foreach (var player in players) {
            if (player.isOwned) count++;
            if (count > 1) return true;
        }
        return false;
    }

    private Vector3 GetSpawnPosition() {
        // ���� �κ� �÷��̾ �����Ǵ� ��ġ�� �����մϴ�.
        return new Vector3(UnityEngine.Random.Range(-6f, 6f), 6f, 0);
    }
}
