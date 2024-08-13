using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;

public class PlayerNickname : NetworkBehaviour {
    // �÷��̾��� �г����� ����, ��Ʈ��ũ�� ����ȭ�մϴ�.

    // !! NOTE !! �÷��̾� ������ ���� Canvas�� Text�� �������� ����
    // Child ������ ����ϴ�, �߰� Text ������Ʈ�� �� ��� ���� ����

    [SyncVar(hook =nameof(SetNickname_Hook))] public string Nickname;
    private Text textNickname;

    public void SetNickname_Hook(string _, string value) {
        textNickname.text = value;
    }

    private void Start() {
        // Player�� ������ �� �г����� ����, ������ ����ȭ�մϴ�.
        if (isLocalPlayer) CmdSetNickname("�����");
    }

    [Command]
    public void CmdSetNickname(string name) {
        Nickname = name;
    }
}