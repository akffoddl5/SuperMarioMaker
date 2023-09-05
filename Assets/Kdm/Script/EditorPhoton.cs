using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using UnityEngine.SceneManagement;

public class EditorPhoton : MonoBehaviourPunCallbacks
{

    private void Start()
    {

        //연결 안되어 있을때
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings();
            return;
        } else if (!PhotonNetwork.InLobby)
        {
            PhotonNetwork.JoinLobby();
            return;
        }
        //
        JoinRoom();

        
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        if (UI_Editor.instance.isQuit)
        {
            SceneManager.LoadScene("LobbyScene");
        }
        else
        {
            PhotonNetwork.JoinLobby();
        }
    }

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        JoinRoom();
    }

    private static void JoinRoom()
    {
        Guid guid = Guid.NewGuid();
        string _title = guid.ToString();
        int _max_player = 1;

        RoomOptions options = new RoomOptions();
        options.MaxPlayers = _max_player;

        //options.CustomRoomProperties = new ExitGames.Client.Photon.Hashtable(){{"master_name", PhotonNetwork.NickName},{"room_name", _title}};
        options.CustomRoomProperties = new Hashtable() { { "master_name", PhotonNetwork.NickName }, { "room_name", _title }, { "room_state", true } };

        //options.CustomRoomProperties = new ExitGames.Client.Photon.Hashtable() {  };
        options.CustomRoomPropertiesForLobby = new string[] { "master_name", "room_name", "room_state" };


        bool make_success = PhotonNetwork.JoinOrCreateRoom(_title, options, null);
    }

    public static void Room_Exit()
    {
        if (PhotonNetwork.InRoom)
        {
            PhotonNetwork.LeaveRoom();
        }
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        base.OnDisconnected(cause);
        Debug.Log("Disconnected");
        
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        Debug.Log("Room Join" + PhotonNetwork.CurrentRoom.Name);
        PhotonNetwork.LocalPlayer.NickName = "test";
    }

    public override void OnLeftLobby()
    {
        base.OnLeftLobby();
        Debug.Log("left lobby");
        SceneManager.LoadScene("LobbyScene");
    }
}
