using UnityEngine;
using Mirror;
using System;

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
    public PlayerColorType playerColor { get; private set; }

    public override void Start() {
        base.Start();

        if(isServer) SpawnRoomPlayer();
        RoomManager.UpdateConnenctedPlayerCount();
    }
    
    private void OnDestroy() {
        RoomManager.UpdateConnenctedPlayerCount();
    }

    private void SpawnRoomPlayer() {
        // ���� �κ񿡼� �÷��̾ ����� �÷��̾� ĳ���͸� ���� �� Instantiate �մϴ�.

        Vector3 spawnPosition = GetSpawnPosition();

        var player = Instantiate(RoomManager.singleton.spawnPrefabs[0], spawnPosition, Quaternion.identity);
        var playerColor = FindObjectOfType<RoomManager>().MyPlayerColor;
        player.GetComponent<PlayerColor>().playerColor = PlayerColorType.nullColor;
        player.GetComponent<PlayerColor>().playerColor = playerColor;

        var clickEffect = Instantiate(RoomManager.singleton.spawnPrefabs[1]);
        clickEffect.GetComponent<PlayerMouseCommunication>().effectColor = playerColor;

        NetworkServer.Spawn(player, connectionToClient);
        NetworkServer.Spawn(clickEffect, connectionToClient);
    }

    private PlayerColorType GetSpawnColor() {
        // ���� �κ� ������ �� �÷��̾�� �ٸ� ������ �����մϴ�.
        var roomSlots = (NetworkManager.singleton as RoomManager).roomSlots;

        foreach (PlayerColorType spawnColor in Enum.GetValues(typeof(PlayerColorType))) {
            bool isUsedColor = false;
            foreach (var roomPlayer in roomSlots) {
            var player = roomPlayer as RoomPlayer;
                if (player.playerColor == spawnColor &&
                    roomPlayer.netId != netId) {
                    isUsedColor = true;
                    break;
                }
            }
            if(!isUsedColor) {
                playerColor = spawnColor;
                break;
            }
        }
        return playerColor;
    }

    private Vector3 GetSpawnPosition() {
        // ���� �κ� �÷��̾ �����Ǵ� ��ġ�� �����մϴ�.
        return new Vector3(UnityEngine.Random.Range(-6f, 6f), 6f, 0);
    }
}
