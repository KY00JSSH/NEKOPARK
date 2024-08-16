using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MySql.Data.MySqlClient;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;


public class UserInfo {
    public int userNo { get; private set; }
    public string userId { get; private set; }
    public string userNick { get; private set; }
    public string userPassword { get; private set; }

    public UserInfo(int no, string id, string nick, string password) {
        userNo = no;
        userId = id;
        userNick = nick;
        userPassword = password;
    }

    public void infoDelete() {
        userNo = 0;
        userId = string.Empty;
        userNick = string.Empty;
        userPassword = string.Empty;
    }
}

public enum DBState {
    EXIST_ID,
    LGN_SUCCESS,
    LGN_ERROR,
    REG_SUCCESS,
    REG_ERROR,
    UPD_SUCCESS,
    UPD_ERROR,
    DIS_CONNECT
}

[System.Serializable]
public class DBConfig {
    public string IP;
    public string TableName;
    public string ID;
    public string PW;
    public string PORT;
}

public class SQLManager : MonoBehaviour
{
    public UserInfo info;
    public MySqlConnection connection;
    public MySqlDataReader reader;

    public static SQLManager instance = null;

    private void Awake() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else {
            Destroy(gameObject);
            return;
        }

        try {
            string serverInfo = serverSet();
            connection = new MySqlConnection(serverInfo);
            connection.Open();
            Debug.Log("DB Open Connection compelete");
        }
        catch (Exception e) {
            Debug.Log(e.Message);
        }
    }
    private string serverSet() {
        TextAsset serverPath = Resources.Load<TextAsset>(Path.Combine("Network/db.config"));
        DBConfig dbConfig = JsonUtility.FromJson<DBConfig>(serverPath.text);

        string serverInfo = $"Server={dbConfig.IP};" + $"Database={dbConfig.TableName};" +
            $"Uid={dbConfig.ID};" + $"Pwd={dbConfig.PW};"
            + $"Port={dbConfig.PORT};" + "CharSet=utf8;";

        return serverInfo;
    }

    private bool connectCheck(MySqlConnection connection) {
        if (connection.State != System.Data.ConnectionState.Open) {
            connection.Open();
            if (connection.State != System.Data.ConnectionState.Open) {
                return false;
            }
        }
        return true;
    }

    public DBState Login(string id, string password) {
        try {
            if (!connectCheck(connection)) {
                return DBState.DIS_CONNECT;
            }
            string sqlCommand = string.Format(@"SELECT USR_NO, USR_ID, USR_NCNM, USR_PWD FROM tb_user WHERE " +
                $"USR_ID = '{id}' AND DEL_YN = 'N';");
            string hashedPwd = SHA256Hash(password);    //秦教
            MySqlCommand cmd = new MySqlCommand(sqlCommand, connection);
            reader = cmd.ExecuteReader();
            if (reader.HasRows) {
                while (reader.Read()) {
                    int no = (reader.IsDBNull(0)) ? -1 : (int)reader["USR_NO"];
                    string usrId = (reader.IsDBNull(1)) ? string.Empty : (string)reader["USR_ID"];
                    string nick = (reader.IsDBNull(2)) ? string.Empty : (string)reader["USR_NCNM"];
                    string pwd = (reader.IsDBNull(3)) ? string.Empty : (string)reader["USR_PWD"];
                    if (!usrId.Equals(string.Empty) || !pwd.Equals(string.Empty)) {
                        if (pwd.Equals(hashedPwd)) {    //秦教 password 眉农
                            info = new UserInfo(no, usrId, nick, pwd);
                            if (!reader.IsClosed) reader.Close();
                            return DBState.LGN_SUCCESS;
                        }
                        else {
                            break;
                        }
                    }
                    else {
                        break;
                    }
                }
            }
            if (!reader.IsClosed) reader.Close();
            return DBState.LGN_ERROR;
        }
        catch (Exception e) {
            Debug.Log(e.Message);
            if (!reader.IsClosed) reader.Close();
            return DBState.LGN_ERROR;
        }
    }

    public DBState SignUp(string id, string password, string nick) {
        try {
            if (!connectCheck(connection)) {
                return DBState.DIS_CONNECT;
            }

            string sqlCommand = string.Format(@$"SELECT USR_NO FROM tb_user WHERE USR_ID = '{id}';");

            string hashedPwd = SHA256Hash(password);    //秦教
            MySqlCommand cmd = new MySqlCommand(sqlCommand, connection);
            reader = cmd.ExecuteReader();

            if (reader.HasRows) {
                return DBState.EXIST_ID;
            }
            else {
                if (!reader.IsClosed) reader.Close();

                string insertSqlCommand = string.Format(@$"INSERT INTO tb_usr(USR_ID, USR_NCNM, USR_PWD, CREATE_USR, CREATE_DATE) VALUES('{id}' , '{nick}', '{hashedPwd}', '{id}', NOW()) ;");

                MySqlCommand cmd2 = new MySqlCommand(insertSqlCommand, connection);
                int signNum = cmd2.ExecuteNonQuery();

                if (signNum > 0) {
                    return DBState.REG_SUCCESS;
                }
            }
            if (!reader.IsClosed) reader.Close();
            return DBState.REG_ERROR;
        }
        catch (Exception e) {
            Debug.Log(e.Message);
            if (!reader.IsClosed) reader.Close();
            return DBState.REG_ERROR;
        }
    }

    public DBState UpdateUsrInfo(string id, string password, string nick) {
        try {
            if (!connectCheck(connection)) {
                return DBState.DIS_CONNECT;
            }

            string hashedPwd = SHA256Hash(password);

            string updSqlCommand = string.Format(@$"UPDATE tb_usr SET USR_ID = '{id}', USR_NCNM='{nick}', USR_PWD ='{hashedPwd}', UPDATE_USR='{id}' ,UPDATE_DATE = NOW()  WHERE USR_ID='{info.userId}';");
            MySqlCommand cmd2 = new MySqlCommand(updSqlCommand, connection);
            int updNum = cmd2.ExecuteNonQuery();
            if (updNum > 0) {
                info = new UserInfo(info.userNo, id, nick, password);
                return DBState.UPD_SUCCESS;
            }

            return DBState.UPD_ERROR;
        }
        catch (Exception e) {
            Debug.Log(e.Message);
            if (!reader.IsClosed) reader.Close();
            return DBState.UPD_ERROR;
        }
    }

    public static string SHA256Hash(string data) {
        SHA256 sha = new SHA256Managed();
        byte[] hash = sha.ComputeHash(Encoding.ASCII.GetBytes(data));
        StringBuilder stringBuilder = new StringBuilder();
        foreach (byte b in hash) {
            stringBuilder.AppendFormat("{0:x2}", b);
        }
        return stringBuilder.ToString();
    }

    public string MessageByState(DBState thisState) {
        switch (thisState) {
            case DBState.EXIST_ID:
                return "This ID already exists\n\nPlease sign up with a different ID";
            case DBState.LGN_SUCCESS:
                return "Login successful";
            case DBState.LGN_ERROR:
                return "Login failed\n\nPlease check your information again";
            case DBState.REG_ERROR:
                return "Registration faile\n\nPlease check your information again";
            case DBState.REG_SUCCESS:
                return "You've signed up\n\nWelcome :)";
            case DBState.UPD_SUCCESS:
                return "Your information has been modified";
            case DBState.UPD_ERROR:
                return "Modification failed.\n\nPlease check the information you entered.";
            case DBState.DIS_CONNECT:
                return "The connection to the DB has been lost\n\nPlease check the server connection.";
            default:
                return "";
        }
    }
}
