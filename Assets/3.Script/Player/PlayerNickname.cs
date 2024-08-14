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

    private void Awake() {
        textNickname = GetComponentInChildren<Text>();
    }

    public void SetNickname_Hook(string _, string value) {
        textNickname.text = value;
    }

    private void Start() {
        // Player�� ������ �� �г����� ����, ������ ����ȭ�մϴ�.
        if (isOwned) CmdSetNickname("�����");
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.F1))
            CmdSetNickname("������");
    }

    [Command]
    public void CmdSetNickname(string name) {
        Nickname = name;
    }
}