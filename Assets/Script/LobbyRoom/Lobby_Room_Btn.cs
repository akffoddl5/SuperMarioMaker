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
    //πÊ ¡§∫∏
    public RoomInfo my_room_info { get; set; }
    public int room_num { get; set; }
    public int master_client_id { get; set; }
    public string room_name;
    public string room_master_name;


    [Header("INFO ∆–≥ŒµÈ")]
    [SerializeField] TMP_Text room_number_text;              //   πÊ π¯»£ ≈ÿΩ∫∆Æ
    public GameObject room_master_flag;                            //  πÊ¿Â ±π∞° ¿ÃπÃ¡ˆ
    public Text room_name_text;                                       // πÊ ¡¶∏Ò
    [SerializeField] TMP_Text room_player_num_text;      //  πÊ ¿Œø¯ºˆ «ˆ»≤ ≈ÿΩ∫∆Æ
    public Text room_master_name_text;                            //  πÊ¿Â ¿Ã∏ß ≈ÿΩ∫∆Æ
    public Text room_start_state; // Playing Text

    public Sprite korFlag;
    public Sprite engFlag;
    public Sprite chFlag;
    public Sprite jpFlag;


    void Start()
    {
        room_number_text.text = room_num.ToString();                           //πÊπ¯»£  º≥¡§
        room_name_text.text = my_room_info.Name;                                                       //πÊ ¡¶∏Ò º≥¡§
        room_player_num_text.text = my_room_info.PlayerCount + " / " + my_room_info.MaxPlayers;   // πÊ ¿Œø¯ «ˆ»≤ º≥¡§
        room_master_name_text.text = room_master_name;                                            //πÊ¿Â ¿Ã∏ß º≥¡§
		//Debug.Log(room_master_name + " " + room_master_name.ToCharArray()[0]);
		if (room_master_name != "" )
        room_master_flag.GetComponent<Image>().sprite = CheckCountry(room_master_name.ToCharArray()[0]);  //πÊ¿Â ±π∞° º≥¡§
	}

    void Update()
    {

	}

	// Change the shape of the flag that appears in Room Btn
    // depending on the first letter of the room leader's nickname.
	Sprite CheckCountry(char _firstNickName)
	{
		// ¿Ø¥œƒ⁄µÂ
		// Korean
		if ((0xAC00 <= _firstNickName && _firstNickName <= 0xD7A3) // «—±€ ¿Ω¿˝(∞°~∆R)
			|| (0x3131 <= _firstNickName && _firstNickName <= 0x318E)) // »£»ØøÎ «—±€(¿⁄¿Ω, ∏¿Ω)
			return korFlag;

		// English
		else if ((0x61 <= _firstNickName && _firstNickName <= 0x7A)
				 || (0x41 <= _firstNickName && _firstNickName <= 0x5A))
			return engFlag;

		// Japanese
		else if ((0x3040 <= _firstNickName && _firstNickName <= 0x309F) // »˜∂Û∞°≥™
				|| (0x30A0 <= _firstNickName && _firstNickName <= 0x30FF) // ∞°≈∏ƒ´≥™
				|| (0x31F0 <= _firstNickName && _firstNickName <= 0x31FF)) // «—¡ﬂ¿œ ∫Œºˆ ∫∏√Ê
			return jpFlag;

		// Chinese
		else if ((0x2E80 <= _firstNickName && _firstNickName <= 0x2EFF) // «—¡ﬂ¿œ ∫Œºˆ ∫∏√Ê
				|| (0x3400 <= _firstNickName && _firstNickName <= 0x4DBF) // «—¡ﬂ¿œ ≈Î«’«—¿⁄ »Æ¿Â -A
				|| (0x4E00 <= _firstNickName && _firstNickName <= 0x9FBF) // «—¡ﬂ¿œ ≈Î«’ «—¿⁄
				|| (0xF900 <= _firstNickName && _firstNickName <= 0xFAFF) // «—¡ﬂ¿œ »£»ØøÎ «—¿⁄
				|| (0x20000 <= _firstNickName && _firstNickName <= 0x2A6DF) // «—¡ﬂ¿œ ≈Î«’«—¿⁄ »Æ¿Â
				|| (0x2F800 <= _firstNickName && _firstNickName <= 0x2FA1F)) // «—¡ﬂ¿œ »£»Ø «—¿⁄ ∫∏√Ê
			return chFlag;
		else
			return korFlag;
	}

    // Btn ∑Œ∫Òø° ª˝º∫µ» πÊ¿ª ≈¨∏Ø«ﬂ¿ª ∂ß Ω««‡µ«¥¬ «‘ºˆ
    public void OnClick()
    {
        PhotonNetwork.JoinRoom(room_name);
        Debug.Log("Onclick" + room_name);
    }
}