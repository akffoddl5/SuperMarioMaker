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
    //�� ����
    public RoomInfo my_room_info { get; set; }
    public int room_num { get; set; }
    public int master_client_id { get; set; }
    public string room_name;
    public string room_master_name;


    [Header("INFO �гε�")]
    [SerializeField] TMP_Text room_number_text;              //   �� ��ȣ �ؽ�Ʈ
    public GameObject room_master_flag;                            //  ���� ���� �̹���
    public Text room_name_text;                                       // �� ����
    [SerializeField] TMP_Text room_player_num_text;      //  �� �ο��� ��Ȳ �ؽ�Ʈ
    public Text room_master_name_text;                            //  ���� �̸� �ؽ�Ʈ
    public Text room_start_state; // Playing Text

    public Sprite korFlag;
    public Sprite engFlag;
    public Sprite chFlag;
    public Sprite jpFlag;


    void Start()
    {
        room_number_text.text = room_num.ToString();                           //���ȣ  ����
        room_name_text.text = my_room_info.Name;                                                       //�� ���� ����
        room_player_num_text.text = my_room_info.PlayerCount + " / " + my_room_info.MaxPlayers;   // �� �ο� ��Ȳ ����
        room_master_name_text.text = room_master_name;                                            //���� �̸� ����
		//Debug.Log(room_master_name + " " + room_master_name.ToCharArray()[0]);
		if (room_master_name != "" )
        room_master_flag.GetComponent<Image>().sprite = CheckCountry(room_master_name.ToCharArray()[0]);  //���� ���� ����
	}

    void Update()
    {

	}

	// Change the shape of the flag that appears in Room Btn
    // depending on the first letter of the room leader's nickname.
	Sprite CheckCountry(char _firstNickName)
	{
		// �����ڵ�
		// Korean
		if ((0xAC00 <= _firstNickName && _firstNickName <= 0xD7A3) // �ѱ� ����(��~�R)
			|| (0x3131 <= _firstNickName && _firstNickName <= 0x318E)) // ȣȯ�� �ѱ�(����, ����)
			return korFlag;

		// English
		else if ((0x61 <= _firstNickName && _firstNickName <= 0x7A)
				 || (0x41 <= _firstNickName && _firstNickName <= 0x5A))
			return engFlag;

		// Japanese
		else if ((0x3040 <= _firstNickName && _firstNickName <= 0x309F) // ���󰡳�
				|| (0x30A0 <= _firstNickName && _firstNickName <= 0x30FF) // ��Ÿī��
				|| (0x31F0 <= _firstNickName && _firstNickName <= 0x31FF)) // ������ �μ� ����
			return jpFlag;

		// Chinese
		else if ((0x2E80 <= _firstNickName && _firstNickName <= 0x2EFF) // ������ �μ� ����
				|| (0x3400 <= _firstNickName && _firstNickName <= 0x4DBF) // ������ �������� Ȯ�� -A
				|| (0x4E00 <= _firstNickName && _firstNickName <= 0x9FBF) // ������ ���� ����
				|| (0xF900 <= _firstNickName && _firstNickName <= 0xFAFF) // ������ ȣȯ�� ����
				|| (0x20000 <= _firstNickName && _firstNickName <= 0x2A6DF) // ������ �������� Ȯ��
				|| (0x2F800 <= _firstNickName && _firstNickName <= 0x2FA1F)) // ������ ȣȯ ���� ����
			return chFlag;
		else
			return korFlag;
	}

    // Btn �κ� ������ ���� Ŭ������ �� ����Ǵ� �Լ�
    public void OnClick()
    {
        PhotonNetwork.JoinRoom(room_name);
        Debug.Log("Onclick" + room_name);
    }
}