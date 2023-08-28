using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class TestServer : MonoBehaviourPunCallbacks
{
    public override void OnConnected()
    {
        base.OnConnected();
    }


    public override void OnConnectedToMaster()
    {
        //Debug.Log("onConnectToMaster");
        RoomOptions options = new RoomOptions();
        options.MaxPlayers = 20;



		options.CustomRoomProperties = new ExitGames.Client.Photon.Hashtable() { { "master_name", "test" }, { "room_name", "test" }, { "room_state", "test" } };
		//options.CustomRoomProperties = new ExitGames.Client.Photon.Hashtable() {  };
		options.CustomRoomPropertiesForLobby = new string[] { "master_name", "room_name", "room_state" };




		PhotonNetwork.JoinOrCreateRoom("TESTSERVER1", options, null);
        //Debug.Log("룸생성");
        //PhotonNetwork.JoinLobby();

    }

    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
    }

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        Debug.Log("테스트 서버에 들어옴");
        //var a =PhotonNetwork.InstantiateRoomObject("Prefabs/Mario", new Vector3(0, 0, 0), Quaternion.identity);
        var a = PhotonNetwork.Instantiate("Prefabs/Mario", new Vector3(0, 0, 0), Quaternion.identity);
        
    }

    private void Awake()
    {
        Screen.SetResolution(960, 540, false);
        //PhotonNetwork.SendRate = 60;
        //PhotonNetwork.SerializationRate = 30;
    }
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    void Update()
    {
        
    }
}
