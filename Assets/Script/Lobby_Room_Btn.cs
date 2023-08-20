using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;

public class Lobby_Room_Btn : MonoBehaviourPunCallbacks
{
    //방 정보
    public RoomInfo my_room_info { get; set; }
    public int room_num { get; set; }
    public int master_client_id { get; set; }


    [Header("INFO 패널들")]
    [SerializeField] TMP_Text room_number_text;              //   방 번호 텍스트
    public GameObject room_master_flag;                            //  방장 국가 이미지
    public Text room_name_text;                                       // 방 제목
    [SerializeField] TMP_Text room_player_num_text;      //  방 인원수 현황 텍스트
    public Text room_master_name_text;                            //  방장 이름 텍스트




    void Start()
    {
        room_number_text.text = room_num.ToString();                           //방번호  설정
        room_name_text.text = my_room_info.Name;                                                       //방 제목 설정
        room_player_num_text.text = my_room_info.PlayerCount + " / " + my_room_info.MaxPlayers;   // 방 인원 현황 설정
        room_master_name_text.text = master_client_id.ToString();                                            //방장 이름 설정
                                                                                                       //방장 국가 설정
        


    }

    void Update()
    {
        
    }
}
