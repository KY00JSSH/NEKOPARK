using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Mirror;
using System.Text;
using UnityEditor;
using System;

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
    }

    private void StartClient() {
        client = new TcpClient();
        serverIP = new IPEndPoint(
            IPAddress.Parse(TCPserver.GetServerIP()), int.Parse(TCPserver.GetServerPort()));
    }

    public string SendRequest(RequestType requestType) {
        try {
            client.Connect(serverIP);
        }
        catch (Exception e) {
            if (e is ObjectDisposedException) {
                try {
                    StartClient();
                    client.Connect(serverIP);
                }
                catch (Exception f) {
                    throw;
                }
            }
        }
        StreamReader reader = new StreamReader(client.GetStream(), Encoding.UTF8);
        StreamWriter writer = new StreamWriter(client.GetStream(), Encoding.UTF8) { AutoFlush = true };

        TCPrequest request = new TCPrequest();
        switch (requestType) {
            case RequestType.Create:
                request.type = "Create";
                roomData.availableColor.Add(roomData.hostColor);
                roomData.hostColor = (PlayerColorType)PlayerPrefs.GetInt("HostColor");
                FindObjectOfType<RoomManager>().MyPlayerColor = roomData.hostColor;
                roomData.availableColor.Remove(roomData.hostColor);
                roomData.gameType = (GameType)PlayerPrefs.GetInt("GameType");

                var roomManager = NetworkManager.singleton as RoomManager;
                roomManager.maxConnections = PlayerPrefs.GetInt("MaxPlayer");
                roomData.maxConnected = roomManager.maxConnections;
                roomData.isStart = false;
                break;
            case RequestType.Remove:
                request.type = "Remove";
                break;
            case RequestType.Request:
                request.type = "Request";
                request.data = null;
                break;
            case RequestType.Start:
                request.type = "Start";
                break;
            case RequestType.Select:
                request.type = "Select";
                //TODO: 선택한 방목록의 RoomData 가져오기
                break;
            case RequestType.Enter:
                request.type = "Enter";
                //TODO: 선택한 방목록의 RoomData 가져오기
                //TODO: RoomData.hostColor를 선택한 플레이어 색상으로 설정하기
                break;
            case RequestType.Exit:
                request.type = "Exit";
                break;
            default:
                throw new UnassignedReferenceException("Unexpceted type");
        }
        request.data = JsonUtility.ToJson(roomData);

        writer.WriteLine(JsonUtility.ToJson(request));

        string response = reader.ReadLine();
        Debug.Log(response);

        client.Close();
        return response;
    }


}
