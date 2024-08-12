using UnityEngine;
using Mirror;
using System;

// 이 스크립트는 Mirror Network의 Room Manager를 설정하고,
// 멀티플레이 로비 (Game Room Scene) 에서 플레이어를 생성하기 위한 스크립트입니다.

public class RoomPlayer : NetworkRoomPlayer {
    [SyncVar (hook = nameof(SetPlayerColor_Hook))] 
    public PlayerColorType playerColor;
    private SpriteRenderer spriteRenderer;

    public void SetPlayerColor_Hook(PlayerColorType oldColor, PlayerColorType newColor) {
        // Server에서 playerColor 변수의 값이 바뀐 걸 감지하면 Hook 메서드를 호출합니다.
        // 이 메서드는 플레이어 Material의 색깔을 바뀐 색으로 새롭게 설정합니다.
        if (spriteRenderer == null) spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.material.SetColor("_PlayerColor", PlayerColor.GetColor(newColor));
    }

    private void SpawnRoomPlayer() {
        // 게임 로비에서 플레이어가 사용할 플레이어 캐릭터를 설정 및 Instantiate 합니다.

        playerColor = GetSpawnColor();
        Vector3 spawnPosition = GetSpawnPosition();

        var player = Instantiate(RoomManager.singleton.spawnPrefabs[0], spawnPosition, Quaternion.identity);
        NetworkServer.Spawn(player, connectionToClient);
    }

    private PlayerColorType GetSpawnColor() {
        // 게임 로비에 참여한 각 플레이어마다 다른 색깔을 배정합니다.

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
        // 게임 로비에 플레이어가 생성되는 위치를 설정합니다.
        return new Vector3(UnityEngine.Random.Range(-6f, 6f), 6f, 0);
    }
}
