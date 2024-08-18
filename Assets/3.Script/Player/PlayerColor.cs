using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;

[System.Serializable]
public enum PlayerColorType {
    red, orange, yellow, 
    green, blue, purple,
    pink, gray, nullColor
}

// 플레이어 색상을 지정하는 스크립트입니다.
public class PlayerColor : NetworkBehaviour {
    [SyncVar(hook = nameof(SetPlayerColor_Hook))]
    public PlayerColorType playerColor = PlayerColorType.nullColor;
    private SpriteRenderer spriteRenderer;

    public void SetPlayerColor_Hook(PlayerColorType oldColor, PlayerColorType newColor) {
        // Server에서 playerColor 변수의 값이 바뀐 걸 감지하면 Hook 메서드를 호출합니다.
        // 이 메서드는 플레이어 Material의 색깔을 바뀐 색으로 새롭게 설정합니다.
        if (spriteRenderer == null) 
            spriteRenderer = GetComponent<SpriteRenderer>();

        spriteRenderer.material.SetColor("_PlayerColor", PlayerColor.GetColor(newColor));
        RoomPlayer.MyRoomPlayer.playerColor = newColor;
    }

    private void Start() {
        if (isOwned) 
            CmdSetPlayerColor((PlayerColorType)PlayerPrefs.GetInt("HostColor"));
    }

    [Command]
    public void CmdSetPlayerColor(PlayerColorType color) {
        playerColor = color;
    }

    public List<PlayerColorType> GetAvailableColor() {
        // 플레이어가 선택하지 않은, 사용가능한 색상의 리스트를 return 합니다.

        List<PlayerColorType> colors = new List<PlayerColorType>(
                (PlayerColorType[])Enum.GetValues(typeof(PlayerColorType)));
        var roomSlots = (NetworkRoomManager.singleton as RoomManager).roomSlots;

        foreach (PlayerColorType spawnColor in Enum.GetValues(typeof(PlayerColorType))) {
            foreach (var roomPlayer in roomSlots) {
                var player = roomPlayer as RoomPlayer;
                if (player.playerColor == spawnColor) {
                    colors.Remove(spawnColor);
                    break;
                }
            }
        }
        return colors;
    }

    // 아래는 Color Const 설정 값입니다. 수정 금지.
    private static Color[] colors = new Color[] {
        new Color(1f, 0.4f, 0.44f),
        new Color(1f, 0.6f, 0.4f),
        new Color(1f, 0.95f, 0.4f),
        new Color(0.35f, 1f, 0.6f),
        new Color(0.48f, 0.8f, 1f),
        new Color(0.73f, 0.6f, 1f),
        new Color(1f, 0.65f, 1f),
        new Color(0.8f, 0.8f, 0.8f),
        new Color(0.1f, 0.1f, 0.1f)
    };

    public static Color GetColor(PlayerColorType playerColor) { return colors[(int)playerColor]; }
    public static Color Red { get { return colors[(int)PlayerColorType.red]; } }
    public static Color Orange { get { return colors[(int)PlayerColorType.orange]; } }
    public static Color Yellow { get { return colors[(int)PlayerColorType.yellow]; } }
    public static Color Green { get { return colors[(int)PlayerColorType.green]; } }
    public static Color Blue { get { return colors[(int)PlayerColorType.blue]; } }
    public static Color Pruple { get { return colors[(int)PlayerColorType.purple]; } }
    public static Color Pink { get { return colors[(int)PlayerColorType.pink]; } }
    public static Color Gray { get { return colors[(int)PlayerColorType.gray]; } }
}
