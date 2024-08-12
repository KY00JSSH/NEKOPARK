using UnityEngine;
using Mirror;
using System;

// �� ��ũ��Ʈ�� Mirror Network�� Room Manager�� �����ϰ�,
// ��Ƽ�÷��� �κ� (Game Room Scene) ���� �÷��̾ �����ϱ� ���� ��ũ��Ʈ�Դϴ�.

public class RoomPlayer : NetworkRoomPlayer {
    [SyncVar (hook = nameof(SetPlayerColor_Hook))] 
    public PlayerColorType playerColor;
    private SpriteRenderer spriteRenderer;

    public void SetPlayerColor_Hook(PlayerColorType oldColor, PlayerColorType newColor) {
        // Server���� playerColor ������ ���� �ٲ� �� �����ϸ� Hook �޼��带 ȣ���մϴ�.
        // �� �޼���� �÷��̾� Material�� ������ �ٲ� ������ ���Ӱ� �����մϴ�.
        if (spriteRenderer == null) spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.material.SetColor("_PlayerColor", PlayerColor.GetColor(newColor));
    }

    private void SpawnRoomPlayer() {
        // ���� �κ񿡼� �÷��̾ ����� �÷��̾� ĳ���͸� ���� �� Instantiate �մϴ�.

        playerColor = GetSpawnColor();
        Vector3 spawnPosition = GetSpawnPosition();

        var player = Instantiate(RoomManager.singleton.spawnPrefabs[0], spawnPosition, Quaternion.identity);
        NetworkServer.Spawn(player, connectionToClient);
    }

    private PlayerColorType GetSpawnColor() {
        // ���� �κ� ������ �� �÷��̾�� �ٸ� ������ �����մϴ�.

        var roomSlots = (NetworkManager.singleton as RoomManager).roomSlots;
        PlayerColorType playerColor = PlayerColorType.red;

        for (int i = 0; i < Enum.GetValues(typeof(PlayerColorType)).Length; i++) {
            bool isSameColor = false;
            foreach(var roomplayer in roomSlots) {
                var player = roomplayer as RoomPlayer;
                if (player.playerColor == (PlayerColorType)i &&
                    roomplayer.netId != netId) {
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
        return playerColor;
    }

    private Vector3 GetSpawnPosition() {
        // ���� �κ� �÷��̾ �����Ǵ� ��ġ�� �����մϴ�.
        return new Vector3(UnityEngine.Random.Range(-6f, 6f), 6f, 0);
    }
}
