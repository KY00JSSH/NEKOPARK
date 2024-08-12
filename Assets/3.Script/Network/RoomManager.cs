using UnityEngine;
using Mirror;
public class RoomManager : NetworkRoomManager {

    public override void OnRoomServerConnect(NetworkConnectionToClient conn) {
        base.OnRoomServerConnect(conn);

        var player = Instantiate(spawnPrefabs[0]);
        NetworkServer.Spawn(player, conn);
    }

}
