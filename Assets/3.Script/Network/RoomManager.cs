using UnityEngine;
using Mirror;
using System.Net;
using System.Net.Sockets;
using UnityEngine.UI;

public class RoomManager : NetworkRoomManager {
    public static int ConnectedPlayer { get { return (NetworkManager.singleton as RoomManager).roomSlots.Count; } }

    public string roomPassword { get; private set; }
    public void SetRoomPassword(string pwd = null) { roomPassword = pwd; }

    public void SetNetworkAddress() {
        IPAddress[] addresses = Dns.GetHostAddresses(Dns.GetHostName());
        foreach (IPAddress address in addresses)
            if (address.AddressFamily == AddressFamily.InterNetwork)
                networkAddress = address.ToString();
        if (NetworkManager.singleton.DebuggingOverride) networkAddress = "127.0.0.1";
    }

    public void SetNetworkAddress(string hostIP) {
        networkAddress = hostIP;
        if (NetworkManager.singleton.DebuggingOverride) networkAddress = "127.0.0.1";
    }

    //���ο� Ŭ���̾�Ʈ�� ������ ������ �� Game Room�� ���ӽ�Ű�� �ݹ� �Լ��Դϴ�.
    public override void OnRoomServerConnect(NetworkConnectionToClient conn) {
        base.OnRoomServerConnect(conn);
    }

    public static void UpdateConnenctedPlayerCount () {
        // ���� �������� �÷��̾� ���� Ȯ��, �����ϴ� �޼����Դϴ�.
        var networkManager = NetworkManager.singleton as RoomManager;
        int currentConnected = networkManager.roomSlots.Count;
        int maxConnected = networkManager.maxConnections;
        
        FindObjectOfType<LobbyMenuManager>().MaxCountText.text = $"{currentConnected} / {maxConnected}";
    }
}
