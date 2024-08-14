using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

// �ΰ��ӿ��� ���Ǵ� GamePlayer �������� ������Ʈ�Դϴ�.

public class GamePlayer : NetworkBehaviour {
    private RoomPlayer myRoomplayer;
    private PlayerNickname playerNickname;
    private PlayerColor playerColor;

    private void Awake() {
        playerNickname = GetComponent<PlayerNickname>();
        playerColor = GetComponent<PlayerColor>();
        myRoomplayer = RoomPlayer.MyRoomPlayer; 
    }

    private void Start() {
        playerNickname.CmdSetNickname(myRoomplayer.Nickname);
        playerColor.playerColor = myRoomplayer.playerColor;
    }
}