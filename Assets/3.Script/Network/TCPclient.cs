using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Mirror;
using System.Text;

public class TCPclient : MonoBehaviour {
    public static TCPclient Instance = null;
    private void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }

    private IPEndPoint serverIP;
    private TcpClient client;
    private RoomData roomData;

    private void Start() {
        StartClient();
        InitRoomData();
    }

    private void InitRoomData() {
        roomData = new RoomData();

        IPAddress[] addresses = Dns.GetHostAddresses(Dns.GetHostName());
        foreach (IPAddress address in addresses) 
            if (address.AddressFamily == AddressFamily.InterNetwork) 
                roomData.hostIP = address.ToString();
        if (NetworkManager.singleton.DebuggingOverride) roomData.hostIP = "127.0.0.1";
        roomData.hostPort = 5555;
        roomData.hostName = "김수주"; //TODO: 로그인 후 UI에서 닉네임 받아오기!!!

        roomData.hostColor = 
    }

    private void StartClient() {
        client = new TcpClient();
        serverIP = new IPEndPoint(
            IPAddress.Parse(TCPserver.GetServerIP()), int.Parse(TCPserver.GetServerPort()));
    }

    public string SendRequest(RequestType requestType) {
        client.Connect(serverIP);
        StreamReader reader = new StreamReader(client.GetStream(), Encoding.UTF8);
        StreamWriter writer = new StreamWriter(client.GetStream(), Encoding.UTF8) { AutoFlush = true };
        
        TCPrequest request = new TCPrequest();
        request.data = JsonUtility.ToJson(roomData);
        switch (requestType) {
            case RequestType.Create:
                request.type = "Create";
                break;
            case RequestType.Remove:
                request.type = "Remove";
                break;
            case RequestType.Request:
                request.type = "Request";
                break;
            default:
                throw new UnassignedReferenceException("Unexpceted type");
        }

        writer.WriteLine(JsonUtility.ToJson(request));

        string response = reader.ReadLine();
        Debug.Log(response);

        client.Close();
        return response;
    }
}
