using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class PlayerColorSetting : NetworkBehaviour {
    public static PlayerColorSetting Instance = null;
    private void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }
    
    [SyncVar] public PlayerColorType playerColor;

    [Command]
    public void CmdSetPlayerColor(PlayerColorType color) {
        playerColor = color;
        RpcSetPlayerColor(color);
    }

    [ClientRpc]
    public void RpcSetPlayerColor(PlayerColorType color) {
        this.playerColor = color;
    }

    private void Update() {
        return;
    }
}