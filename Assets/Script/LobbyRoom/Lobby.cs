using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using UnityEngine.Events;
using Photon.Pun.Demo.Cockpit;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using UnityEngine.SceneManagement;
using UnityEditor;

public class Lobby : MonoBehaviourPunCallbacks
{
    [Header("�κ� INFO ���� ����")]
    [SerializeField] private Text lobby_info;
    [SerializeField] private int lobby_player_count = -1;
    [SerializeField] private int lobby_player_count_tmp; //����ȭ �� �ӽ� ����

    [Header("�� ��� ���� ����")]
    public GameObject room_btn;
    private List<RoomInfo> myList = new List<RoomInfo>();
    public RectTransform Room_List_Content;


    [Header("�� ����� ���� ����")]
    private  int max_Player;
    private Coroutine set_max_player;
    public int current_max_player;
    private Text make_room_title;


    public Text log_text;

	string characterPrefab;
    float instMarioX = 0;
	// Room�� �ִ� �Լ��� �����ϱ� ���� �̺�Ʈ �Լ�
	//public UnityEvent RoomUISync;
	public PhotonView PV;

	private void Awake()
    {
        PhotonNetwork.JoinLobby();

		lobby_info = GameObject.Find("Lobby_info_count").GetComponent<Text>();
        log_text = GameObject.Find("Log").GetComponent<Text>();
        Lobby_Player_Count();

        max_Player = 4;
        GameObject.Find("Room_Maker_Player_Scroll").GetComponent<Scrollbar>().value = 0;
        current_max_player = 1;
        make_room_title = GameObject.Find("Make_Room_Title").GetComponent<Text>();

        StartCoroutine(ILobby_Refresh());
    }

    private void Start()
    {
        AudioManager.instance.PlayerOneShot(MARIO_SOUND.LOBBY_BGM, true, 0);
    }

	private void Update()
	{
		Lobby_Player_Count();
	}


    private void Lobby_Player_Count()
	{
		lobby_player_count_tmp = PhotonNetwork.CountOfPlayers - PhotonNetwork.CountOfPlayersInRooms;
		//lobby_player_count_tmp = PhotonNetwork.CurrentRoom.PlayerCount;

		//Debug.Log(PhotonNetwork.CurrentRoom.PlayerCount);

		if (lobby_player_count_tmp != lobby_player_count)
		{
			//Debug.Log(PhotonNetwork.CurrentRoom.Name);
			lobby_player_count = lobby_player_count_tmp;
			lobby_info.text = "�κ� ����� :  " + lobby_player_count + "�� ";

			//log_text.text += "\n ���� ������Ŭ���̾�Ʈ id : " + PhotonNetwork.CurrentRoom.MasterClientId;
			//log_text.text += "\n �� �ڽ��� ������ Ŭ���̾�Ʈ ?  : " + PhotonNetwork.IsMasterClient;

			//Debug.Log(PhotonNetwork.CurrentRoom.)
		}

        if (!PhotonNetwork.IsConnected)
        {
            SceneManager.LoadScene(0);
        }
	}

	public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        Debug.Log("onConnectToMaster");
        PhotonNetwork.JoinLobby();
        
    }

    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
        
	}

   


    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();

    }

	public override void OnCreateRoomFailed(short returnCode, string message)
    {
        base.OnCreateRoomFailed(returnCode, message);
    }
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        base.OnJoinRoomFailed(returnCode, message);
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        base.OnJoinRandomFailed(returnCode, message);
        
    }


    
    // �κ� �ִ� ����� ���� �� ����
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        
        //Debug.Log("OnRoomListUpdate ������Ʈ..");
        base.OnRoomListUpdate(roomList);
        int updated_count = roomList.Count;
        for(int i = 0; i < updated_count; i++)
        {
            if (roomList[i].RemovedFromList)
            {
                if (myList.Contains(roomList[i])){
                    myList.Remove(roomList[i]);
                }
            } else
            {
                if (myList.Contains(roomList[i]))
                {
                    myList[myList.IndexOf(roomList[i])] = roomList[i];
                }
                else
                {
                    myList.Add(roomList[i]);
                }
            }
        }
        
        Room_List_Init();
    }
    
    private void Room_List_Init()
    {
		RectTransform[] tmp = Room_List_Content.GetComponentsInChildren<RectTransform>();
        
        if (tmp != null)
        {
            for (int i = 0; i < tmp.Length; i++)
            {
                if (tmp[i].gameObject != Room_List_Content.gameObject)
                    Destroy(tmp[i].gameObject);
                    //tmp[i].gameObject.SetActive(false);
            }
        }

        for (int i = 0; i < myList.Count; i++)
        {
            var a = Instantiate(room_btn, Vector3.zero, Quaternion.identity);
            a.transform.parent = Room_List_Content;
            a.transform.localScale = new Vector3(1, 1, 1);
            
            a.GetComponent<Lobby_Room_Btn>().my_room_info = myList[i];
            a.GetComponent<Lobby_Room_Btn>().room_num = i + 1;
            a.GetComponent<Lobby_Room_Btn>().master_client_id = myList[i].masterClientId;

			// At Room_List_Init(), Turned on/off Playing Text according to "roomstate"
			// +interactive(roomState)
            a.GetComponent<Button>().interactable = !((bool)myList[i].CustomProperties["room_state"]);
			a.GetComponent<Lobby_Room_Btn>().room_start_state.gameObject.SetActive((bool)myList[i].CustomProperties["room_state"]);
            // false�� interactable true���� ��
            
            a.GetComponent<Lobby_Room_Btn>().room_master_name = myList[i].CustomProperties["master_name"].ToString();
            a.GetComponent<Lobby_Room_Btn>().room_name = myList[i].CustomProperties["room_name"].ToString();
            
        }
    }

	// Btn�� ����� Ŭ��
	public void Room_Plus_Click()
    {
        AudioManager.instance.PlayerOneShot(MARIO_SOUND.SELECT, false, 2);
        //var a = GameObject.Find("Lobby_Layer");
        //a.transform.Translate(new Vector3(-100, 0, 0));
        //StartCoroutine(CorLerp(a, a.transform.position, new Vector3(-2000, a.transform.position.y, a.transform.position.z)));

        var a = GameObject.Find("Room_Make_Layer");

        // If you leave the room and click Create, the title of the room you created before is written
        // So we need to initialize the room title inputfield
        a.GetComponentInChildren<InputField>().text = "";

		//a.transform.localPosition = new Vector3(0, 1000, 0);
		StartCoroutine(CorLerp(a,new Vector3(0,1000,0), new Vector3(0,-100,0)));
        Debug.Log("room plus click");
    }

	// Btn �� ����� ���̾� ���� �� â�ݱ� ��ư(x)
	public void Room_Close_Click()
    {
        AudioManager.instance.PlayerOneShot(MARIO_SOUND.SELECT, false, 2);
        var a = GameObject.Find("Room_Make_Layer");
        StartCoroutine(CorLerp(a, a.GetComponent<RectTransform>().localPosition, new Vector3(0, 1300, 0)));

    }

	// Btn �ִ� �÷��̾� �ø���
	public void Room_Maker_Player_Select_Plus()
	{

		if (current_max_player + 1 <= max_Player)
		{
			if (set_max_player != null)
				StopCoroutine(set_max_player);
			current_max_player++;
			set_max_player = StartCoroutine(Cor_Room_Maker_Player_Scroll());
		}
	}

	// Btn �ִ� �÷��̾� ���̱�
	public void Room_Maker_Player_Select_Minus()
	{

		if (current_max_player - 1 <= max_Player && current_max_player - 1 > 0)
		{
			if (set_max_player != null)
				StopCoroutine(set_max_player);
			current_max_player--;
			set_max_player = StartCoroutine(Cor_Room_Maker_Player_Scroll());

		}
	}
    
    // Btn �� ���� Ŭ��
    public void Try_Room_Make()
    {
        string _title = make_room_title.text;
        int _max_player = current_max_player;
        bool _isRoomStart = false;
        Debug.Log(_title + " " + _max_player);

        RoomOptions options = new RoomOptions();
        options.MaxPlayers = _max_player;

        //options.CustomRoomProperties = new ExitGames.Client.Photon.Hashtable(){{"master_name", PhotonNetwork.NickName},{"room_name", _title}};
        options.CustomRoomProperties = new Hashtable(){{"master_name", PhotonNetwork.NickName},{"room_name", _title}, { "room_state", _isRoomStart } };

        //options.CustomRoomProperties = new ExitGames.Client.Photon.Hashtable() {  };
        options.CustomRoomPropertiesForLobby = new string[]{"master_name", "room_name", "room_state"};
        

        bool make_success = PhotonNetwork.JoinOrCreateRoom(_title, options, null);
        if (make_success)
        {
            Debug.Log("�� ���� ����");
        }
        else
        {
            Debug.Log("�� ���� ����");
            // �� ���� ���� UI ����
        }

    }

	// UI������ �����̱�
	IEnumerator CorLerp(GameObject gameObject, Vector3 start_pos, Vector3 des_pos)
    {
        gameObject.SetActive(true);
        RectTransform RT = gameObject.GetComponent<RectTransform>();
        RT.localPosition = start_pos;
        while (Vector3.Distance(RT.localPosition, des_pos) > 50f) 
        {
            //Debug.Log(Vector3.Distance(RT.localPosition, des_pos));
            RT.localPosition = Vector3.Lerp(RT.localPosition, des_pos, 0.3f);
            //yield return new WaitForSeconds(0.5f);
            yield return new WaitForSeconds(0.02f);
        }
        yield break;
    }

    // �ִ� �÷��̾� ������ �� �߾��� ���ڰ� ������ �� �Ѿ�� �ϴ� ��
    IEnumerator Cor_Room_Maker_Player_Scroll()
    {
        var a = GameObject.Find("Room_Maker_Player_Scroll").GetComponent<Scrollbar>();
        float current_value = a.value;
        float des_value =  1.0f/(max_Player-1) * (current_max_player-1);

        //Debug.Log(current_value + " " + des_value + " " + max_Player + "  " );
        while (true)
        {
            a.value = (a.value + des_value) / 2;
            if (Mathf.Abs(des_value - a.value) < 0.001f)
            {
                break;
            }
            yield return null;
        }
        yield break;;
    }


    public override void OnFriendListUpdate(List<FriendInfo> friendList)
    {
        base.OnFriendListUpdate(friendList);
    }

	public void Room_Init()
	{
		// lobby left move
		GameObject lobby = GameObject.Find("Lobby_Layer");
		StartCoroutine(CorLerp(lobby, lobby.GetComponent<RectTransform>().localPosition,
			lobby.GetComponent<RectTransform>().localPosition + new Vector3(-2000, 0, 0)));
		// room move leftside
		GameObject room_layer = GameObject.Find("Room_Layer");
		StartCoroutine(CorLerp(room_layer, new Vector3(0, 1100, 0), new Vector3(0, 0, 0)));
        // room maker move upside
		var a = GameObject.Find("Room_Make_Layer");
		Vector3 localPositionA = a.GetComponent<RectTransform>().localPosition;
		StartCoroutine(CorLerp(a, localPositionA, new Vector3(localPositionA.x, 1000, 0)));
	}

	public override void OnJoinedRoom()
	{
		base.OnJoinedRoom();

		Room_Init();
        
		// The number of players in the room determines which prefab is created.
		int playerNum = PhotonNetwork.CurrentRoom.PlayerCount;
		Vector3 spawnPosition = new Vector3(instMarioX, 2, 0);
		switch (playerNum)
        {
            case 1:
		        PhotonNetwork.Instantiate("Prefabs/Mario", spawnPosition, Quaternion.identity);
				characterPrefab = "Prefabs/Mario";
				break;
            case 2:
		        PhotonNetwork.Instantiate("Prefabs/Mario2", spawnPosition, Quaternion.identity);
				characterPrefab = "Prefabs/Mario2";
                break;
            case 3:
		        PhotonNetwork.Instantiate("Prefabs/Mario3", spawnPosition, Quaternion.identity);
				characterPrefab = "Prefabs/Mario3";
                break;
            case 4:
		        PhotonNetwork.Instantiate("Prefabs/Mario4", spawnPosition, Quaternion.identity);
				characterPrefab = "Prefabs/Mario4";
                break;
        }
        instMarioX++;
		if (instMarioX > 4) instMarioX = 0;

		ExitGames.Client.Photon.Hashtable customProperties = new ExitGames.Client.Photon.Hashtable();
		customProperties.Add("characterName", characterPrefab);
		PhotonNetwork.LocalPlayer.SetCustomProperties(customProperties);

		//Debug.Log("marioX ��ġ �� �� �ٲ�� �ž�: " + new Vector3(instMarioX, 2, 0));
		// Mario spawns from x values ??0, 1, 2, 3

		PV.RPC("RoomUISync", RpcTarget.AllBuffered);
		//RoomUISync.Invoke();
	}


	public override void OnLeftRoom()
	{
		base.OnLeftRoom();
        // �� ���������� ����
		GameObject room = GameObject.Find("Room_Layer");
		StartCoroutine(CorLerp(room, room.GetComponent<RectTransform>().localPosition,
			room.GetComponent<RectTransform>().localPosition + new Vector3(2000, 0, 0)));

		// �κ� �����;� ��
		GameObject lobby = GameObject.Find("Lobby_Layer");

		StartCoroutine(CorLerp(lobby, lobby.GetComponent<RectTransform>().localPosition, Vector3.zero));

		// ���� ������ �����ͷ� ���ϱ� �κ�� �ٽ� ������ �ؾ� ��
		PhotonNetwork.JoinLobby();

	}

	public override void OnPlayerLeftRoom(Player otherPlayer)
	{
		base.OnPlayerLeftRoom(otherPlayer);
        // ������ ������ UI ����ȭ
		PV.RPC("RoomUISync", RpcTarget.AllBuffered);
	}

    // �κ� ���ΰ�ħ 1�ʸ��� �ڷ�ƾ
    public IEnumerator ILobby_Refresh()
    {
        while (true)
        {
            yield return new WaitForSeconds(3);
            //Debug.Log("refresh..." + PhotonNetwork.IsConnected + " " + PhotonNetwork.IsConnectedAndReady + " " + PhotonNetwork.InLobby + " " + PhotonNetwork.InRoom);
            Lobby_Refresh();

        }
    }

    // �κ� ������
    public void Lobby_Refresh()
    {
        //PhotonNetwork.JoinLobby();

        //RoomOptions RO = new RoomOptions();
        //RO.IsVisible = false;
        //RO.MaxPlayers = 30;
        if(PhotonNetwork.InLobby)
            PhotonNetwork.LeaveLobby();
        

    }

    // �κ� ������ �� �ٷ� �ٽ� ������ �����
    public override void OnLeftLobby()
    {
        base.OnLeftLobby();
        //Debug.Log("lobby left");
        PhotonNetwork.JoinLobby();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        base.OnDisconnected(cause);
        Debug.Log(cause + " " + cause.ToString());
        PhotonNetwork.ConnectUsingSettings();
    }
}
