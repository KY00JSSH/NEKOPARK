using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;

public class PlayerNickname : NetworkBehaviour {
    // 플레이어의 닉네임을 설정, 네트워크에 동기화합니다.

    // !! NOTE !! 플레이어 프리팹 하위 Canvas의 Text를 가져오기 위해
    // Child 순서를 사용하니, 추가 Text 오브젝트가 들어갈 경우 수정 유의

    [SyncVar(hook =nameof(SetNickname_Hook))] public string Nickname;
    private Text textNickname;

    private void Awake() {
        textNickname = GetComponentInChildren<Text>();
    }

    public void SetNickname_Hook(string _, string value) {
        textNickname.text = value;
    }

    private void Start() {
        // Player가 생성될 때 닉네임을 설정, 서버와 동기화합니다.
        if (isOwned) CmdSetNickname("김수주");
        //TODO: 방 입장시 UI에서 (DB에서) 닉네임을 받아와서,
        //TODO: RoomPlayer에 저장해뒀다가 가져와야 합니다.
    }

    [Command]
    public void CmdSetNickname(string name) {
        Nickname = name;
    }
}
