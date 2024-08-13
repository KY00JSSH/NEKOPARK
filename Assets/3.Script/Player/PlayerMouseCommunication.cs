using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using Mirror;

public class PlayerMouseCommunication : NetworkBehaviour {
    // �� ��ũ��Ʈ�� �÷��̾ ȭ�鿡 ���콺 Ŭ���� �� ��,
    // Click Effect�� �ִϸ��̼��� Ȱ��ȭ�ϰ� Ŭ�� ��ġ�� transform�� �̵���ŵ�ϴ�.

    private Animator effectAnimator;
    private void Awake() {
        effectAnimator = GetComponent<Animator>();
    }

    [ClientCallback]
    private void Update() {
        if(Input.GetMouseButtonDown(0)) {
            if (EventSystem.current.IsPointerOverGameObject()) return;
            
            Vector3 clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            clickPosition.z = 0;
            CmdSpawnClickEffect(clickPosition);
        }
    }

    [Command]
    private void CmdSpawnClickEffect (Vector3 position) {
        RpcSpawnClickEffect(position);
    }

    [ClientRpc]
    private void RpcSpawnClickEffect(Vector3 position) {
        transform.position = position;
        effectAnimator.Play("ClickEffect");
    }
}