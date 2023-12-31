using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class Lobby_Room_Btn : MonoBehaviourPunCallbacks
{
    //방 정보
    public RoomInfo my_room_info { get; set; }
    public int room_num { get; set; }
    public int master_client_id { get; set; }
    public string room_name;
    public string room_master_name;


    [Header("INFO 패널들")]
    [SerializeField] TMP_Text room_number_text;              //   방 번호 텍스트
    public GameObject room_master_flag;                            //  방장 국가 이미지
    public Text room_name_text;                                       // 방 제목
    [SerializeField] TMP_Text room_player_num_text;      //  방 인원수 현황 텍스트
    public Text room_master_name_text;                            //  방장 이름 텍스트
    public Text room_start_state; // Playing Text

    public Sprite korFlag;
    public Sprite engFlag;
    public Sprite chFlag;
    public Sprite jpFlag;


    void Start()
    {
        room_number_text.text = room_num.ToString();                           //방번호  설정
        room_name_text.text = my_room_info.Name;                                                       //방 제목 설정
        room_player_num_text.text = my_room_info.PlayerCount + " / " + my_room_info.MaxPlayers;   // 방 인원 현황 설정
        room_master_name_text.text = room_master_name;                                            //방장 이름 설정
		//Debug.Log(room_master_name + " " + room_master_name.ToCharArray()[0]);
		if (room_master_name != "" )
        room_master_flag.GetComponent<Image>().sprite = CheckCountry(room_master_name.ToCharArray()[0]);  //방장 국가 설정
	}

    void Update()
    {

	}

	// Change the shape of the flag that appears in Room Btn
    // depending on the first letter of the room leader's nickname.
	Sprite CheckCountry(char _firstNickName)
	{
		// 유니코드
		// Korean
		if ((0xAC00 <= _firstNickName && _firstNickName <= 0xD7A3) // 한글 음절(가~힣)
			|| (0x3131 <= _firstNickName && _firstNickName <= 0x318E)) // 호환용 한글(자음, 모음)
			return korFlag;

		// English
		else if ((0x61 <= _firstNickName && _firstNickName <= 0x7A)
				 || (0x41 <= _firstNickName && _firstNickName <= 0x5A))
			return engFlag;

		// Japanese
		else if ((0x3040 <= _firstNickName && _firstNickName <= 0x309F) // 히라가나
				|| (0x30A0 <= _firstNickName && _firstNickName <= 0x30FF) // 가타카나
				|| (0x31F0 <= _firstNickName && _firstNickName <= 0x31FF)) // 한중일 부수 보충
			return jpFlag;

		// Chinese
		else if ((0x2E80 <= _firstNickName && _firstNickName <= 0x2EFF) // 한중일 부수 보충
				|| (0x3400 <= _firstNickName && _firstNickName <= 0x4DBF) // 한중일 통합한자 확장 -A
				|| (0x4E00 <= _firstNickName && _firstNickName <= 0x9FBF) // 한중일 통합 한자
				|| (0xF900 <= _firstNickName && _firstNickName <= 0xFAFF) // 한중일 호환용 한자
				|| (0x20000 <= _firstNickName && _firstNickName <= 0x2A6DF) // 한중일 통합한자 확장
				|| (0x2F800 <= _firstNickName && _firstNickName <= 0x2FA1F)) // 한중일 호환 한자 보충
			return chFlag;
		else
			return korFlag;
	}

    // Btn 로비에 생성된 방을 클릭했을 때 실행되는 함수
    public void OnClick()
    {
        PhotonNetwork.JoinRoom(room_name);
        Debug.Log("Onclick" + room_name);
    }
}