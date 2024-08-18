using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using Mirror;

public class PlayerMouseCommunication : NetworkBehaviour {
    // 이 스크립트는 플레이어가 화면에 마우스 클릭을 할 때,
    // Click Effect의 애니메이션을 활성화하고 클릭 위치로 transform을 이동시킵니다.

    private static PlayerMouseCommunication myClickEffect;
    public static PlayerMouseCommunication MyClickEffect {
        get {
            if(myClickEffect == null) {
                var clickEffets = FindObjectsOfType<PlayerMouseCommunication>();
                foreach(var effect in clickEffets)
                    if(effect.isOwned) {
                        myClickEffect = effect;
                        break;
                    }
            }
            return myClickEffect;
        }
    }

    [SyncVar(hook = nameof(SetClickEffectColor_Hook))]
    public PlayerColorType effectColor = PlayerColorType.nullColor;

    private Animator effectAnimator;
    private SpriteRenderer spriteRenderer;
    private void Awake() {
        effectAnimator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start() {
        if (isOwned) CmdSetPlayerColor((PlayerColorType)PlayerPrefs.GetInt("HostColor"));
    }
    [Command]
    public void CmdSetPlayerColor(PlayerColorType color) {
        effectColor = color;
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