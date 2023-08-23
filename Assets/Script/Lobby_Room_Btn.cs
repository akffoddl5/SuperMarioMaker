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
    //¹æ Á¤º¸
    public RoomInfo my_room_info { get; set; }
    public int room_num { get; set; }
    public int master_client_id { get; set; }
    public string room_name;
    public string room_master_name;


    [Header("INFO ÆÐ³Îµé")]
    [SerializeField] TMP_Text room_number_text;              //   ¹æ ¹øÈ£ ÅØ½ºÆ®
    public GameObject room_master_flag;                            //  ¹æÀå ±¹°¡ ÀÌ¹ÌÁö
    public Text room_name_text;                                       // ¹æ Á¦¸ñ
    [SerializeField] TMP_Text room_player_num_text;      //  ¹æ ÀÎ¿ø¼ö ÇöÈ² ÅØ½ºÆ®
    public Text room_master_name_text;                            //  ¹æÀå ÀÌ¸§ ÅØ½ºÆ®
    
    public Sprite korFlag;
    public Sprite engFlag;
    public Sprite chFlag;
    public Sprite jpFlag;



    void Start()
    {
        room_number_text.text = room_num.ToString();                           //¹æ¹øÈ£  ¼³Á¤
        room_name_text.text = my_room_info.Name;                                                       //¹æ Á¦¸ñ ¼³Á¤
        room_player_num_text.text = my_room_info.PlayerCount + " / " + my_room_info.MaxPlayers;   // ¹æ ÀÎ¿ø ÇöÈ² ¼³Á¤
        room_master_name_text.text = room_master_name;                                            //¹æÀå ÀÌ¸§ ¼³Á¤
        //Debug.Log(room_master_name + " " + room_master_name.ToCharArray()[0]);
        room_master_flag.GetComponent<Image>().sprite = CheckCountry(room_master_name.ToCharArray()[0]);  //¹æÀå ±¹°¡ ¼³Á¤
	}

    void Update()
    {

	}

    // ¹æÀå ´Ð³×ÀÓÀÇ Ã¹±ÛÀÚ°¡ ¹¹³Ä¿¡ µû¶ó Room Btn¿¡ ¶ß´Â ±¹±â ¸ð¾ç º¯°æ
    Sprite CheckCountry(char _firstNickName)
    {
        // À¯´ÏÄÚµå
        // ÇÑ±¹¾î
        if ((0xAC00 <= _firstNickName && _firstNickName <= 0xD7A3) // ÇÑ±Û À½Àý(°¡~ÆR)
            || (0x3131 <= _firstNickName && _firstNickName <= 0x318E)) // È£È¯¿ë ÇÑ±Û(ÀÚÀ½, ¸ðÀ½)
            return korFlag;


        // ¿µ¾î
        else if ((0x61 <= _firstNickName && _firstNickName <= 0x7A)
                 || (0x41 <= _firstNickName && _firstNickName <= 0x5A))
            return engFlag;

        // ÀÏº»¾î
        else if ((0x3040 <= _firstNickName && _firstNickName <= 0x309F) // È÷¶ó°¡³ª
                || (0x30A0 <= _firstNickName && _firstNickName <= 0x30FF) // °¡Å¸Ä«³ª
                || (0x31F0 <= _firstNickName && _firstNickName <= 0x31FF)) // °¡Å¸Ä«³ª À½¼º È®Àå
            return jpFlag;

        // ÇÑÀÚ
        else if ((0x2E80 <= _firstNickName && _firstNickName <= 0x2EFF) // ÇÑÁßÀÏ ºÎ¼ö º¸Ãæ
                || (0x3400 <= _firstNickName && _firstNickName <= 0x4DBF) // ÇÑÁßÀÏ ÅëÇÕÇÑÀÚ È®Àå -A
                || (0x4E00 <= _firstNickName && _firstNickName <= 0x9FBF) // ÇÑÁßÀÏ ÅëÇÕ ÇÑÀÚ
                || (0xF900 <= _firstNickName && _firstNickName <= 0xFAFF) // ÇÑÁßÀÏ È£È¯¿ë ÇÑÀÚ
                || (0x20000 <= _firstNickName && _firstNickName <= 0x2A6DF) // ÇÑÁßÀÏ ÅëÇÕÇÑÀÚ È®Àå
                || (0x2F800 <= _firstNickName && _firstNickName <= 0x2FA1F)) // ÇÑÁßÀÏ È£È¯ ÇÑÀÚ º¸Ãæ
            return chFlag;

        return null;
	}
    
    // Btn ·Îºñ¿¡ »ý¼ºµÈ ¹æÀ» Å¬¸¯ÇßÀ» ¶§ ½ÇÇàµÇ´Â ÇÔ¼ö
    public void OnClick()
    {
        PhotonNetwork.JoinRoom(room_name);
        Debug.Log("Onclick" + room_name);
    }
}
