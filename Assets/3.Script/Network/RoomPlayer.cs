using UnityEngine;
using Mirror;
using System;

// �� ��ũ��Ʈ�� Mirror Network�� Room Manager�� �����ϰ�,
// ��Ƽ�÷��� �κ� (Game Room Scene) ���� �÷��̾ �����ϱ� ���� ��ũ��Ʈ�Դϴ�.

public class RoomPlayer : NetworkRoomPlayer {
    private PlayerColorType playerColor;

    public override void Start() {
        base.Start();
        if(isServer) SpawnRoomPlayer();
    }

    private void SpawnRoomPlayer() {
        // ���� �κ񿡼� �÷��̾ ����� �÷��̾� ĳ���͸� ���� �� Instantiate �մϴ�.

        Vector3 spawnPosition = GetSpawnPosition();

        var player = Instantiate(RoomManager.singleton.spawnPrefabs[0], spawnPosition, Quaternion.identity);
        player.GetComponent<PlayerColor>().playerColor = GetSpawnColor();

        NetworkServer.Spawn(player, connectionToClient);
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

        /*
        for (int i = 0; i < Enum.GetValues(typeof(PlayerColorType)).Length; i++) {
            bool isSameColor = false;
            foreach(var roomplayer in roomSlots) {
                var player = roomplayer as RoomPlayer;
                if (player.playerColor == (PlayerColorType)i &&
                    roomplayer.netId != netId) {                    // ���� �ƴ� �ٸ� �÷��̾ 
                    isSameColor = true;
                    break;
                }
            }
            if (isSameColor) continue;
            else {
                playerColor = (PlayerColorType)i;
                break;
            }
        }
        */

        return playerColor;
    }

    private Vector3 GetSpawnPosition() {
        // ���� �κ� �÷��̾ �����Ǵ� ��ġ�� �����մϴ�.
        return new Vector3(UnityEngine.Random.Range(-6f, 6f), 6f, 0);
    }
}
