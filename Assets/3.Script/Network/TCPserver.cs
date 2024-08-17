using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Collections.Generic;
using LitJson;
using UnityEngine;
using System.IO;
using System.Text;
using UnityEditor;
using Org.BouncyCastle.Ocsp;
using Mirror;

[System.Serializable]
public class RoomList {
    public List<RoomData> roomList;
}

[System.Serializable]
public class ColorList {
    public List<PlayerColorType> colorList;
}

[System.Serializable]
public class RoomData {
    public string hostName;
    public string hostIP;
    public int hostPort;

    public List<PlayerColorType> availableColor;
    public PlayerColorType hostColor;
    public GameType gameType;
    public bool isStart;
    public int currentConnected;
    public int maxConnected;

    public RoomData() {
        hostName = "Default";
        hostIP = "127.0.0.1";
        hostPort = 5555;

        hostColor = PlayerColorType.red;
        availableColor = new List<PlayerColorType>(
            (PlayerColorType[])Enum.GetValues(typeof(PlayerColorType)));
        availableColor.Remove(PlayerColorType.red);

        gameType = GameType.Public;
        isStart = false;
        currentConnected = 1;
        maxConnected = 8;
    }
}

[System.Serializable]
public class TCPrequest {
    public string type;
    public string data;
}

public enum RequestType {
    Create, Remove, Request,
    Start, Select, Enter, Exit
}

public enum GameType {
    Friend, Public
}

public class TCPserver : MonoBehaviour {
    private List<RoomData> roomList = new List<RoomData>();
    private TcpListener server;
    private Thread serverThread;
    private bool isRun;

    private Queue<string> Log;
    private int maxLogCount = 100;

    public void AddLog(string log) {
        log = DateTime.Now.ToString("[yyyy-MM-dd HH:mm:ss] ") + log;
        Debug.Log(log);
        Log.Enqueue(log);
        if (Log.Count > maxLogCount) Log.Dequeue();
    }
    private void Start() {
        Log = new Queue<string>();
        try {
            StartServer(GetServerIP(), GetServerPort());
            if (server == null)
                AddLog("ERROR : SERVER CONFIG LOADING FALIURE.");

        }
        catch (Exception e) {
            AddLog("ERROR : SERVER CONFIG FALIURE.\n" + e.Message);
        }
    }

    public void StartServer(string serverIP, string PORT) {
        try {
            serverIP = "127.0.0.1"; //TODO: For Debug. Remove this on release.

            server = new TcpListener(IPAddress.Parse(serverIP), int.Parse(PORT));
            server.Start(); isRun = true;
            AddLog("Server Starting...");

            serverThread = new Thread(Run);
            serverThread.IsBackground = true;
            serverThread.Start();
        }
        catch (Exception e) {
            AddLog("ERROR : SERVER INITIALIZE FALIURE.\n" + e.Message);
        }
    }

    private void Run() {
        TcpClient client = new TcpClient();
        try {
            while (isRun) {
                client = server.AcceptTcpClient();
                IPEndPoint clientIP = (IPEndPoint)client.Client.RemoteEndPoint;
                AddLog($"Client Connected : {clientIP.Address.ToString()}");

                StreamReader reader = new StreamReader(client.GetStream(), Encoding.UTF8);
                StreamWriter writer = new StreamWriter(client.GetStream(), Encoding.UTF8) { AutoFlush = true };

                try {
                    string request = reader.ReadLine();
                    AddLog($"Client Request : {request}");

                    ProcessRequest(request, writer);
                }
                catch (Exception e) {
                    AddLog("ERROR : CLIENT REQUEST PROCESSING FALIURE.\n" + e.Message);

                }
            }
        }
        catch (Exception e) {
            AddLog("ERROR : CLIENT CONNECTION FALIURE.\n" + e.Message);
        }
        finally {
            client.Close();
        }
    }

    private void ProcessRequest(string req, StreamWriter response) {
        Debug.Log(roomList.Count);
        if (roomList.Count > 0)
            Debug.Log(roomList[0]?.hostIP);

        TCPrequest request = JsonUtility.FromJson<TCPrequest>(req);
        RoomData room = JsonUtility.FromJson<RoomData>(request.data);
        string json;

        if (room == null) {
            AddLog($"Room Data Null Exception");
            response.WriteLine("Status : Room Data Receive Failed.");
            return;
        }

        switch (request.type) {
            case "Create":
                roomList.Add(room);
                AddLog($"Room Created : {room.hostName}");
                response.WriteLine("Status : Room Created Successfully.");
                break;

            case "Remove":
                if (roomList.Remove(FindRoom(room))) {
                    AddLog($"Room Removed : {room.hostName}");
                    response.WriteLine("Status : Room Removed Successfully.");
                }
                else {
                    AddLog($"Room Removed Failure : {room.hostName}");
                    response.WriteLine("Status : Room Removed Failed.");
                }
                break;

            case "Request":
                json = JsonUtility.ToJson(new RoomList { roomList = roomList });
                response.WriteLine(json);
                break;

            case "Start":
                if (FindRoom(room) == null) {
                    AddLog($"Room Start Failure : {room.hostName}");
                    response.WriteLine("Status : Room Started Failed.");
                }
                else {
                    roomList[roomList.IndexOf(FindRoom(room))].isStart = true;
                    AddLog($"Room Started : {room.hostName}");
                    response.WriteLine("Status : Room Started Successfully.");
                }
                break;

            case "Select":
                if (FindRoom(room) == null) {
                    AddLog($"Client Select Failure : {room.hostName}");
                    response.WriteLine(JsonUtility.ToJson(null));
                }
                else {
                    List<PlayerColorType> availableColor = roomList[roomList.IndexOf(FindRoom(room))].availableColor;
                    ColorList colorList = new ColorList() { colorList = availableColor };
                    json = JsonUtility.ToJson(colorList);

                    AddLog($"Client Select Room : {room.hostName}");
                    response.WriteLine(json);
                }
                break;

            case "Enter":
                if (FindRoom(room) == null) {
                    AddLog($"Client Enter Failure : {room.hostName}");
                    response.WriteLine("EnterFailure");
                }
                else {
                    List<PlayerColorType> availableColor = roomList[roomList.IndexOf(FindRoom(room))].availableColor;
                    if (availableColor.Contains(room.hostColor)) {
                        availableColor.Remove(room.hostColor);
                        AddLog($"Client Enter Room : {room.hostName}");
                        response.WriteLine("Success");
                    }
                    else {
                        AddLog($"Client Color Select Failure : {room.hostName}");
                        response.WriteLine("ColorFailrue");
                    }
                }
                break;
            case "Exit":
                if(FindRoom(room) == null) {
                    AddLog($"Client Exit Room Failure : {room.hostName}");
                    response.WriteLine("Status : Room Exit Failed.");
                }
                else {
                    roomList[roomList.IndexOf(FindRoom(room))].currentConnected--;
                    AddLog($"Client Exit Room : {room.hostName}");
                    response.WriteLine("Status : Room Exit Successfully.");
                }
                break;
            default:
                AddLog("ERROR : Unexpected Request Receieved");
                response.WriteLine("Status : Unexpected Request.");
                break;

        }
    }

    public RoomData FindRoom(RoomData room) {
        foreach (RoomData eachRoom in roomList) {
            if (room.hostName == eachRoom.hostName &&
                room.hostIP == eachRoom.hostIP &&
                room.hostPort == eachRoom.hostPort)
                return eachRoom;
        }
        return null;
    }

    public static JsonData GetServerConfig() {
        try {
            TextAsset jsonTextAsset = Resources.Load<TextAsset>("Network/serverConfig");
            string JsonString = jsonTextAsset.text;
            JsonData jsonData = JsonMapper.ToObject(JsonString);
            return jsonData;
        }
        catch (Exception e) {
            return null;
        }
    }

    public static string GetServerIP() {
        var config = GetServerConfig();
        return (string)config[0]["ServerIP"];
    }
    public static string GetServerPort() {
        var config = GetServerConfig();
        return (string)config[0]["PORT"];
    }

    private void OnApplicationQuit() {
        isRun = false;
        server.Stop();
    }
}
