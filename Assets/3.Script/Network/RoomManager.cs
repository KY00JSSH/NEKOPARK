using UnityEngine;
using Mirror;
public class RoomManager : NetworkRoomManager {

    public override void OnRoomServerConnect(NetworkConnectionToClient conn) {
        base.OnRoomServerConnect(conn);

    }

}
