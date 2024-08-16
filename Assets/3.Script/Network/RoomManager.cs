using UnityEngine;
using Mirror;
public class RoomManager : NetworkRoomManager {
    public static int ConnectedPlayer { get { return (NetworkManager.singleton as RoomManager).roomSlots.Count; } }
    public PlayerColorType MyPlayerColor;

    public string roomPassword { get; private set; }
    public void SetRoomPassword(string pwd = null) { roomPassword = pwd; }

    //새로운 클라이언트가 서버에 접속할 때 Game Room에 접속시키는 콜백 함수입니다.
    public override void OnRoomServerConnect(NetworkConnectionToClient conn) {
        base.OnRoomServerConnect(conn);
        Debug.Log(roomSlots);
    }

    public static void UpdateConnenctedPlayerCount () {
        // 현재 접속중인 플레이어 수를 확인, 갱신하는 메서드입니다.
        var networkManager = NetworkManager.singleton as RoomManager;
        int currentConnected = networkManager.roomSlots.Count;
        int maxConnected = networkManager.maxConnections;
        
        //TODO: UI TEXT를 연동해주세요
        //[CONNECTION_TEXT].text = $"{currentConnected} / {maxConnected}";
    }
}
