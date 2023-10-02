using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System;
using System.Text;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


public class Skyway : SingletonMonoBehaviour<Skyway>
{
    [DllImport("__Internal")]
    private static extern void createHost(string token, string room_name);
    [DllImport("__Internal")]
    private static extern void createClient(string token, string room_name);
    [DllImport("__Internal")]
    private static extern void reCreateToken(string token);
    [DllImport("__Internal")]
    private static extern void writeStream(string message);
    [DllImport("__Internal")]
    private static extern void changeJoin(bool mode);
    [DllImport("__Internal")]
    private static extern void closeRoom();
    [DllImport("__Internal")]
    private static extern int getMembers();
    [DllImport("__Internal")]
    public static extern bool skyway();

    // TODO API_KEYを置き換える
    private static string api_key = "API_KEY";
    private bool is_host = false;
    private bool is_mult = false;
    private bool my_char = false;
    private Level level;
    private Stage stage;
    private Member me;
    private List<Member> members = new List<Member>();
    private string room;
    private string room_raw;
    private List<string> _members_id = new List<string>();
    private int member_count = 0;

    public UnityEvent<string> ConnectHandler;
    public UnityEvent<string> ErrorHandler;
    public UnityEvent CloseHandler;
    public UnityEvent<int> ChangeMembersHandler;
    public UnityEvent<string, string, string> DataEventHandler;
    public UnityEvent ReadyHandler;

    public string my_id;

    public List<string> members_id
    {
        get
        {
            return _members_id;
        }
    }

    public List<Member> Members
    {
        get
        {
            return members;
        }
    }
    public string room_id
    {
        get
        {
            return room_raw;
        }
    }

    public bool Host
    {
        get
        {
            return is_host;
        }
    }

    public bool Mult
    {
        get
        {
            return is_mult;
        }
    }

    public Stage Stage
    {
        get
        {
            return stage;
        }
    }

    public Level Level
    {
        get
        {
            return level;
        }
    }

    public Member my_ins
    {
        get
        {
            return me;
        }
    }

    public void writeStream(string header, string body, bool loop_back=false)
    {
        if (is_mult) writeStream(header + ";" + body);
        else onData("me;" + header + ";" + body);
        if (is_mult && loop_back) onData("me;" + header + ";" + body);
    }

    public void joinHost(Level level, Stage stage)
    {
        this.level = level;
        this.stage = stage;
        this.my_char = false;
#if UNITY_EDITOR
        return;
#endif
        this.room = makeRoomName();
        UnityEngine.Random.InitState(int.Parse(this.room_raw));
        createHost(makeHostToken(this.room), this.room);
    }

    public void joinClient(string room_name)
    {
#if UNITY_EDITOR
        return;
#endif
        this.room_raw = room_name;
        this.room = Base64Url(room_name);
        UnityEngine.Random.InitState(int.Parse(this.room_raw));
        createClient(makeClientToken(this.room), this.room);
    }

    public void joinSolo(Level level, Stage stage, Character character)
    {
        is_host = true;
        is_mult = false;

        this.level = level;
        this.stage = stage;

        me.id = "solo";
        me.character = character;
    }

    public void retryRoomName()
    {
        joinHost(level, stage);
    }

    public void requestToken()
    {
        if (is_host)
        {
            reCreateToken(makeHostToken(this.room));
        }
        else
        {
            reCreateToken(makeClientToken(this.room));
        }
    }

    public void onClose()
    {
        CloseHandler?.Invoke();
        members.Clear();
        _members_id.Clear();
        members = new List<Member>();
        _members_id = new List<string>();
    }

    public void CloseRoom()
    {
        if (Host && Mult)
        {
            closeRoom();
        }
        is_host = false;
        is_mult = false;
        my_char = false;
        members = new List<Member>();
        _members_id = new List<string>();
}

    public void onData(string send_data)
    {
        int pos = send_data.IndexOf(";");
        string fromid = send_data.Substring(0, pos);
        string data = send_data.Substring(pos + 1);
        pos = data.IndexOf(";");
        string header = data.Substring(0, pos);
        string body = data.Substring(pos + 1);
        Debug.Log($"header:{header} body: {body} id:{fromid}");
        DataEventHandler?.Invoke(header, body, fromid);
#if !UNITY_EDITOR
        if (is_mult) member_count = getMembers();
#endif
        switch(header)
        {
            case "level":
                level = (Level)int.Parse(body);
                Debug.Log("level" + level);
                break;
            case "stage":
                stage = (Stage)int.Parse(body);
                Debug.Log("stage" + stage);
                break;
            case "char":
                Member tmp_member;
                tmp_member.character = (Character)int.Parse(body);
                tmp_member.id = fromid;
                members.Add(tmp_member);
                if (members.Count == member_count -1 && is_host && my_char)
                {
                    writeStream("room;start");
                    ReadyHandler?.Invoke();
                }
                break;
        }
    }

    public void changeMember(string member)
    {
        string[] members = member.Split(";");
        for (int i = 0; i < members.Length; i++)
        {
            _members_id.Add(members[i]);
        }
#if !UNITY_EDITOR
        member_count = getMembers();
#endif
        //Debug.Log(_members_id.Count);
        //ChangeMembersHandler?.Invoke(_members_id.Count);
        ChangeMembersHandler?.Invoke(member_count);
    }

    public void onError(string res)
    {
        ErrorHandler?.Invoke(res);
    }

    public void onConnect(string id)
    {
        my_id = id;
        me.id = id;
        ConnectHandler?.Invoke(id);
    }

    string makeRoomName()
    {
        var now_time = DateTime.UtcNow;
        this.room_raw = $"{now_time.Minute:00}{now_time.Second:00}{now_time.Millisecond:0000}";
        return Base64Url(this.room_raw);
    }

    string makeHostToken(string room_name)
    {
        this.is_host = true;
        this.is_mult = true;
#if !UNITY_EDITOR
        changeJoin(true);
#endif
        var unixIat = (int)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
        var unixExp = unixIat + 3600;
        Guid uuidJti = Guid.NewGuid();
        string uuidUser = "*";

        string header = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9";
        // TODO YOUR_APP_ID_HEREを置き換える
        string claims = $"{{\"jti\":\"{uuidJti}\",\"iat\":{unixIat},\"exp\":{unixExp},\"scope\":{{\"app\":{{\"id\":\"YOUR_APP_ID_HERE\",\"turn\":true,\"actions\":[\"read\"],\"channels\":[{{\"name\":\"{room_name}\",\"actions\":[\"write\"],\"members\":[{{\"name\":\"{uuidUser}\",\"actions\":[\"write\"],\"publication\":{{\"actions\":[\"write\"]}},\"subscription\":{{\"actions\":[\"write\"]}}}}]}}]}}}}}}";

        string baseToken = $"{header}.{Base64Url(claims)}";
        string sig = Sig(baseToken);
        return $"{baseToken}.{sig}";
    }

    string makeClientToken(string room_name)
    {
        this.is_host = false;
        this.is_mult = true;
        var unixIat = (int)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
        var unixExp = unixIat + 3600;
        Guid uuidJti = Guid.NewGuid();
        //Guid uuidUser = Guid.NewGuid();
        string uuidUser = "*";
        string strChannel = room_name;

        //this.user = uuidUser;

        string header = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9";
        // TODO YOUR_APP_ID_HEREを置き換える
        string claims = $"{{\"jti\":\"{uuidJti}\",\"iat\":{unixIat},\"exp\":{unixExp},\"scope\":{{\"app\":{{\"id\":\"YOUR_APP_ID_HERE\",\"turn\":true,\"actions\":[\"read\"],\"channels\":[{{\"name\":\"{strChannel}\",\"actions\":[\"read\"],\"members\":[{{\"name\":\"{uuidUser}\",\"actions\":[\"write\"],\"publication\":{{\"actions\":[\"write\"]}},\"subscription\":{{\"actions\":[\"write\"]}}}}]}}]}}}}}}";

        string baseToken = $"{header}.{Base64Url(claims)}";
        string sig = Sig(baseToken);
        return $"{baseToken}.{sig}";
    }

    static string Base64Url(byte[] byteData)
    {
        return Convert.ToBase64String(byteData).TrimEnd('=').Replace('+', '-').Replace('/', '_');
    }

    static string Base64Url(string stringData)
    {
        byte[] contentBArr = Encoding.UTF8.GetBytes(stringData);
        return Convert.ToBase64String(contentBArr).TrimEnd('=').Replace('+', '-').Replace('/', '_');
    }

    static string Sig(string stringData)
    {
        // TODO API_KEYを置き換える
        string api_key = "API_KEY";
        string result = null;
        byte[] secretKeyBArr = Encoding.UTF8.GetBytes(Skyway.api_key);
        byte[] contentBArr = Encoding.UTF8.GetBytes(stringData);
        HMACSHA256 HMACSHA256 = new HMACSHA256();
        HMACSHA256.Key = secretKeyBArr;
        byte[] final = HMACSHA256.ComputeHash(contentBArr);
        result = Base64Url(final);
        return result;
    }

    public void end_reception()
    {

#if !UNITY_EDITOR
        changeJoin(false);
        writeStream("room;" + "rend");
        writeStream("level;" + (int)level);
        writeStream("stage;" + (int)stage);
#endif
    }

    public void pickCharacter(Character character)
    {
#if !UNITY_EDITOR
        member_count = getMembers();
#endif
        my_char = true;
        me.character = character;
        writeStream("char;" + (int)character);
        if (members.Count == member_count -1 && is_host)
        {
            writeStream("room;start");
            ReadyHandler?.Invoke();
        }
    }

    public void RemoveAllEvents()
    {
        ConnectHandler.RemoveAllListeners();
        ErrorHandler.RemoveAllListeners();
        CloseHandler.RemoveAllListeners();
        ChangeMembersHandler.RemoveAllListeners();
        DataEventHandler.RemoveAllListeners();
        ReadyHandler.RemoveAllListeners();
    }

    public void SetStageLevel(Stage stage, Level level)
    {
        this.stage = stage;
        this.level = level;
    }

    public void ResetRoom()
    {
        members.Clear();
        my_char = false;
    }
}
public enum Level
{
    Easy,
    Normal,
    Hard
}

public enum Stage
{
    A,
    B,
    C,
    D,
    E,
    F,
    G
}

public enum Character
{
    Miku,
    Rin,
    Len,
    Luka,
    Meiko,
    Kaito
}

public struct Member
{
    public string id;
    public Character character;
}