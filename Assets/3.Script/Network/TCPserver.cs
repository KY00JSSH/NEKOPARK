using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Collections.Generic;
using LitJson;
using UnityEngine;

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


public class TCPserver : MonoBehaviour {
    private List<RoomData> roomList = new List<RoomData>();
    private Queue<string> Log = new Queue<string>();
    private int maxLogCount = 100;

    public void AddLog(string log) {
        Log.Enqueue(log);
        if (Log.Count > maxLogCount) Log.Dequeue();
    }

    public TCPserver (string serverIP, string PORT) {
        TcpListener server = new TcpListener(IPAddress.Parse(serverIP), int.Parse(PORT));
        server.Start();
    }

    private void Start() {
        new TCPserver(GetServerIP(), GetServerPort());
    }


    public JsonData GetServerConfig() {
        try {
            TextAsset jsonTextAsset = Resources.Load<TextAsset>("Network/serverConfig");
            string JsonString = jsonTextAsset.text;
            JsonData jsonData = JsonMapper.ToObject(JsonString);
            return jsonData;
        }
        catch (Exception e) {
            Debug.LogError("ERROR : SERVER INITIALIZE FALIURE.\n" + e.Message);
            return null;
        }
    }
    public string GetServerIP() {
        var config = GetServerConfig();
        return (string)config[0]["ServerIP"];
    }
    public string GetServerPort() {
        var config = GetServerConfig();
        return (string)config[0]["PORT"];
    }
}
