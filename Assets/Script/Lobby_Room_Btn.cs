using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;

public class Lobby_Room_Btn : MonoBehaviourPunCallbacks
{
    //�� ����
    public RoomInfo my_room_info { get; set; }
    public int room_num { get; set; }
    public int master_client_id { get; set; }


    [Header("INFO �гε�")]
    [SerializeField] TMP_Text room_number_text;              //   �� ��ȣ �ؽ�Ʈ
    public GameObject room_master_flag;                            //  ���� ���� �̹���
    public Text room_name_text;                                       // �� ����
    [SerializeField] TMP_Text room_player_num_text;      //  �� �ο��� ��Ȳ �ؽ�Ʈ
    public Text room_master_name_text;                            //  ���� �̸� �ؽ�Ʈ




    void Start()
    {
        room_number_text.text = room_num.ToString();                           //���ȣ  ����
        room_name_text.text = my_room_info.Name;                                                       //�� ���� ����
        room_player_num_text.text = my_room_info.PlayerCount + " / " + my_room_info.MaxPlayers;   // �� �ο� ��Ȳ ����
        room_master_name_text.text = master_client_id.ToString();                                            //���� �̸� ����
                                                                                                       //���� ���� ����
        


    }

    void Update()
    {
        
    }
}
