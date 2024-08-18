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

// �÷��̾� ������ �����ϴ� ��ũ��Ʈ�Դϴ�.
public class PlayerColor : NetworkBehaviour {
    [SyncVar(hook = nameof(SetPlayerColor_Hook))]
    public PlayerColorType playerColor = PlayerColorType.nullColor;
    private SpriteRenderer spriteRenderer;

    public void SetPlayerColor_Hook(PlayerColorType oldColor, PlayerColorType newColor) {
        // Server���� playerColor ������ ���� �ٲ� �� �����ϸ� Hook �޼��带 ȣ���մϴ�.
        // �� �޼���� �÷��̾� Material�� ������ �ٲ� ������ ���Ӱ� �����մϴ�.
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
        // �÷��̾ �������� ����, ��밡���� ������ ����Ʈ�� return �մϴ�.

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

    // �Ʒ��� Color Const ���� ���Դϴ�. ���� ����.
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
