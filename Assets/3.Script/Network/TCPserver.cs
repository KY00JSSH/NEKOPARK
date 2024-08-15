using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Collections.Generic;
using LitJson;
using UnityEngine;
using System.IO;
using System.Text;

[System.Serializable]
public class RoomList {
    public List<RoomData> roomList;
}

[System.Serializable]
public class RoomData {
    public string hostName;
    public string hostIP;
    public int hostPort;
}

[System.Serializable]
public class TCPrequest {
    public string type;
    public string data;
}

public enum RequestType {
    Create, Remove, Request
}

public class TCPserver : MonoBehaviour {
    private List<RoomData> roomList = new List<RoomData>();
    private TcpListener server;
    private Thread serverThread;
    private bool isRun;

    private Queue<string> Log;
    private int maxLogCount = 100;

    public void AddLog(string log) {
        Debug.Log(log);
        Log.Enqueue(log);
        if (Log.Count > maxLogCount) Log.Dequeue();
    }
    private void Start() {
        Log = new Queue<string>();
        try {
            StartServer(GetServerIP(), GetServerPort());
            if(server == null) 
                AddLog("ERROR : SERVER CONFIG LOADING FALIURE.");
            
        }
        catch(Exception e) {
            AddLog("ERROR : SERVER CONFIG FALIURE.\n" + e.Message);
        }
    }

    public void StartServer (string serverIP, string PORT) {
        try {
            server = new TcpListener(IPAddress.Parse(serverIP), int.Parse(PORT));
            server.Start();  isRun = true;
            AddLog("Server Starting...");

            serverThread = new Thread(Run);
            serverThread.IsBackground = true;
            serverThread.Start();
        }
        catch(Exception e) {
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
        TCPrequest request = JsonUtility.FromJson<TCPrequest>(req);
        RoomData room;
        switch (request.type) {
            case "Create":
                room = JsonUtility.FromJson<RoomData>(request.data);
                roomList.Add(room);

                AddLog($"Room Created : {room.hostName}");
                response.WriteLine("Status : Room Created Successfully.");
                break;

            case "Remove":
                room = JsonUtility.FromJson<RoomData>(request.data);
                roomList.Remove(roomList.Find(r =>
                r.hostName == room.hostName && r.hostIP == room.hostIP && r.hostPort == room.hostPort));

                AddLog($"Room Removed : {room.hostName}");
                response.WriteLine("Status : Room Removed Successfully.");

                break;

            case "Request":
                string json = JsonUtility.ToJson(new RoomList { roomList = roomList });
                response.WriteLine(json);
                break;
            default: break;

        }
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
        server.Stop();
    }
}
