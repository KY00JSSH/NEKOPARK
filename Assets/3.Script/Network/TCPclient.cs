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
        InitRoomData();
    }

    private void InitRoomData() {
        roomData = new RoomData();

        roomData.hostName = "김수주"; //TODO: 로그인 후 UI에서 닉네임 받아오기!!!
        roomData.hostIP = RoomManager.singleton.networkAddress;
        if (NetworkManager.singleton.DebuggingOverride) roomData.hostIP = "127.0.0.1";
        roomData.hostPort = 5555;
    }

    private void StartClient() {
        client = new TcpClient();
        serverIP = new IPEndPoint(
            IPAddress.Parse(NetworkManager.singleton.DebuggingOverride ? 
            "127.0.0.1" : TCPserver.GetServerIP()), int.Parse(TCPserver.GetServerPort()));
    }

    public string SendRequest(RequestType requestType) {
        try {
            StartClient();
            client.Connect(serverIP);
        }
        catch (Exception e) {
            throw;
        }

        StreamReader reader = new StreamReader(client.GetStream(), Encoding.UTF8);
        StreamWriter writer = new StreamWriter(client.GetStream(), Encoding.UTF8) { AutoFlush = true };

        TCPrequest request = new TCPrequest();
        switch (requestType) {
            case RequestType.Create:
                request.type = "Create";
                roomData.hostIP = RoomManager.singleton.networkAddress;

                roomData.availableColor.Add(roomData.hostColor);
                roomData.hostColor = (PlayerColorType)PlayerPrefs.GetInt("HostColor");

                roomData.availableColor.Remove(roomData.hostColor);
                roomData.gameType = (GameType)PlayerPrefs.GetInt("GameType");

                roomData.hostName = SQLManager.instance.UserData.userNick;
                PlayerPrefs.SetString("UserName", roomData.hostName);

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
                roomData = FindObjectOfType<JoinRoomManager>().GetButtonRoomData();
                PlayerPrefs.SetInt("SelectRoomMaxPlayer", roomData.maxConnected);
                if (NetworkManager.singleton.DebuggingOverride) roomData.hostIP = "127.0.0.1";
                break;
            case RequestType.Enter:
                request.type = "Enter";
                roomData.hostIP = RoomManager.singleton.networkAddress;
                if (NetworkManager.singleton.DebuggingOverride) roomData.hostIP = "127.0.0.1";
                roomData.hostColor = FindObjectOfType<JoinColorChangeController>().GetSelectClientColor();

                PlayerPrefs.SetString("UserName", SQLManager.instance.UserData.userNick);

                break;
            case RequestType.Exit:
                request.type = "Exit";
                SetSelectRoomIndex(-1);
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

    public void SetSelectRoomIndex(int num) {
        FindObjectOfType<JoinRoomManager>().SetSelectRoomIndex(num);
    }

    private void OnApplicationQuit() {
        Debug.Log("application Quit");
        StartClient();
        Debug.Log("Start Client");
        client.Connect(serverIP);
        Debug.Log("Client Connect");
        SendRequest(RequestType.Remove);
        client.Close();
    }
}
