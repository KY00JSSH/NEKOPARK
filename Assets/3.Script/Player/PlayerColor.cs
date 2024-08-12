using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public enum PlayerColorType {
    red, orange, yellow, 
    green, blue, purple,
    pink, gray
}

// �÷��̾ spawn �� �� ������ �����ϴ� ��ũ��Ʈ�Դϴ�.
public class PlayerColor : NetworkBehaviour {
    [SyncVar(hook = nameof(SetPlayerColor_Hook))]
    public PlayerColorType playerColor = PlayerColorType.gray;
    private SpriteRenderer spriteRenderer;

    public void SetPlayerColor_Hook(PlayerColorType oldColor, PlayerColorType newColor) {
        // Server���� playerColor ������ ���� �ٲ� �� �����ϸ� Hook �޼��带 ȣ���մϴ�.
        // �� �޼���� �÷��̾� Material�� ������ �ٲ� ������ ���Ӱ� �����մϴ�.
        if (spriteRenderer == null) spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.material.SetColor("_PlayerColor", PlayerColor.GetColor(newColor));
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
