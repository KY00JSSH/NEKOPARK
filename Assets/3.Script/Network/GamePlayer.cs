using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

// 인게임에서 사용되는 GamePlayer 프리팹의 컴포넌트입니다.

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