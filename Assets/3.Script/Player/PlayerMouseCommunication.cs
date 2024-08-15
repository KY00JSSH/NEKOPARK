using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using Mirror;

public class PlayerMouseCommunication : NetworkBehaviour {
    // �� ��ũ��Ʈ�� �÷��̾ ȭ�鿡 ���콺 Ŭ���� �� ��,
    // Click Effect�� �ִϸ��̼��� Ȱ��ȭ�ϰ� Ŭ�� ��ġ�� transform�� �̵���ŵ�ϴ�.
    [SyncVar(hook = nameof(SetClickEffectColor_Hook))]
    public PlayerColorType effectColor = PlayerColorType.nullColor;

    private Animator effectAnimator;
    private SpriteRenderer spriteRenderer;
    private void Awake() {
        effectAnimator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start() {
        //transform.SetParent(RoomPlayer.MyRoomPlayer.transform);   
    }

    public void SetClickEffectColor_Hook(PlayerColorType oldColor, PlayerColorType newColor) {
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();

        spriteRenderer.material.SetColor("_PlayerColor", PlayerColor.GetColor(newColor));
    }

    [ClientCallback]
    private void Update() {
        if (!isOwned) return;
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