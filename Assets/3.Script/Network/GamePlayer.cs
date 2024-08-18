using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Org.BouncyCastle.Crypto.Operators;

// �ΰ��ӿ��� ���Ǵ� GamePlayer �������� ������Ʈ�Դϴ�.

public class GamePlayer : NetworkBehaviour {
    private RoomPlayer myRoomplayer;
    private PlayerNickname playerNickname;
    private PlayerColor playerColor;


    private void Awake() {
        playerColor = GetComponent<PlayerColor>();
    }

    private void Start() {
        if(isServer) {
            var clickEffect = Instantiate(RoomManager.singleton.spawnPrefabs[1]);
            clickEffect.GetComponent<PlayerMouseCommunication>().effectColor = PlayerColorType.nullColor;
            clickEffect.GetComponent<PlayerMouseCommunication>().effectColor = playerColor.playerColor;
            NetworkServer.Spawn(clickEffect, connectionToClient);
        }
    }
}