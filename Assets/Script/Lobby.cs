using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;


public class Lobby : MonoBehaviourPunCallbacks
{
    [Header("로비 INFO 관련 변수")]
    [SerializeField] private Text lobby_info;
    [SerializeField] private int lobby_player_count = -1;
    [SerializeField] private int lobby_player_count_tmp; //최적화 용 임시 변수

    [Header("방 목록 관련 변수")]
    public GameObject room_btn;
    private List<RoomInfo> myList = new List<RoomInfo>();


    [Header("방 만들기 관련 변수")]
    private  int max_Player;
    private Coroutine set_max_player;
    public int current_max_player;
    private Text make_room_title;

    public Text log_text;

    private void Awake()
    {
        lobby_info = GameObject.Find("Lobby_info_count").GetComponent<Text>();
        log_text = GameObject.Find("Log").GetComponent<Text>();
        Lobby_Player_Count();

        max_Player = 4;
        GameObject.Find("Room_Maker_Player_Scroll").GetComponent<Scrollbar>().value = 0;
        current_max_player = 1;
        make_room_title = GameObject.Find("Make_Room_Title").GetComponent<Text>();
    }
    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        //Debug.Log("onConnectToMaster");
        
    }

    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        base.OnCreateRoomFailed(returnCode, message);
    }

    
    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        base.OnJoinRandomFailed(returnCode, message);
        
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        base.OnJoinRoomFailed(returnCode, message);
    }

    



    private void Update()
    {
        Lobby_Player_Count();

    }

    private void Get_Room_List()
    {
        

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
            lobby_info.text = "로비 대기자 :  " + lobby_player_count + "명 ";

            //log_text.text += "\n 룸의 마스터클라이언트 id : " + PhotonNetwork.CurrentRoom.MasterClientId;
            //log_text.text += "\n 나 자신은 마스텀 클라이언트 ?  : " + PhotonNetwork.IsMasterClient;
            
            
            
            //Debug.Log(PhotonNetwork.CurrentRoom.)
        }
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        Debug.Log("OnRoomListUpdate 업데이트..");
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
        Debug.Log(myList.Count + " << 방의 갯수");
        Transform Room_List_Content = GameObject.Find("Room_List_Content").transform;
        for (int i = 0; i < myList.Count; i++)
        {
            var a = Instantiate(room_btn, Vector3.zero, Quaternion.identity);
            a.transform.parent = Room_List_Content;
            a.transform.localScale = new Vector3(1, 1, 1);
            a.GetComponent<Lobby_Room_Btn>().my_room_info = myList[i];
            a.GetComponent<Lobby_Room_Btn>().room_num = i + 1;
            a.GetComponent<Lobby_Room_Btn>().master_client_id = myList[i].masterClientId;
            string s= myList[i].CustomProperties["master_name"].ToString();
            Debug.Log(s);
            //myList[i].
            //a.GetComponent<Lobby_Room_Btn>().
        }
    }

    //방 만들기 클릭
    public void Room_Plus_Click()
    {
        //var a = GameObject.Find("Lobby_Layer");
        //a.transform.Translate(new Vector3(-100, 0, 0));
        //StartCoroutine(CorLerp(a, a.transform.position, new Vector3(-2000, a.transform.position.y, a.transform.position.z)));

        var a = GameObject.Find("Room_Make_Layer");
        //a.transform.localPosition = new Vector3(0, 1000, 0);
        StartCoroutine(CorLerp(a,new Vector3(0,1000,0), new Vector3(0,-100,0)));
        Debug.Log("room plus click");
    }

    //방 만들기 레이어 클로즈 버튼 클릭
    public void Room_Close_Click()
    {
        var a = GameObject.Find("Room_Make_Layer");
        StartCoroutine(CorLerp(a, a.GetComponent<RectTransform>().localPosition, new Vector3(0, 1300, 0)));

    }

    //UI러프로 움직이기
    IEnumerator CorLerp(GameObject gameObject, Vector3 start_pos, Vector3 des_pos)
    {
        gameObject.SetActive(true);
        RectTransform RT = gameObject.GetComponent<RectTransform>();
        RT.localPosition = start_pos;
        while (Vector3.Distance(RT.localPosition, des_pos) > 50f) 
        {
            Debug.Log(Vector3.Distance(RT.localPosition, des_pos));
            RT.localPosition = Vector3.Lerp(RT.localPosition, des_pos, 0.3f);
            //yield return new WaitForSeconds(0.5f);
            yield return new WaitForSeconds(0.02f);
        }
        yield break;
    }

    //최대 플레이어 늘리기
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

    //최대 플레이어 줄이기
    public void Room_Maker_Player_Select_Minus()
    {
        
        if (current_max_player - 1 <= max_Player && current_max_player-1 >0)
        {
            if (set_max_player != null)
                StopCoroutine(set_max_player);
            current_max_player--;
            set_max_player = StartCoroutine(Cor_Room_Maker_Player_Scroll());

        }
    }

    //최대 플레이어 설정 코루틴
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

            yield return new WaitForSeconds(0.1f);
                
        }

        yield break;;
    }

    //방 생성 클릭
    public void Try_Room_Make()
    {
        string _title = make_room_title.text;
        int _max_player = current_max_player;
        Debug.Log(_title + " " + _max_player);

        RoomOptions options = new RoomOptions();
        options.MaxPlayers = _max_player;

        options.CustomRoomProperties = new ExitGames.Client.Photon.Hashtable() { { "master_name", "abc" } };
        options.CustomRoomPropertiesForLobby = new string[] { "master_name", PhotonNetwork.NickName };
        


        bool make_success = PhotonNetwork.JoinOrCreateRoom(_title, options, null);
        if (make_success)
        {
            Debug.Log("방 생성 성공");
        }
        else
        {
            Debug.Log("방 생성 실패");

        }

    }

    public override void OnFriendListUpdate(List<FriendInfo> friendList)
    {
        base.OnFriendListUpdate(friendList);
        
    }
}
