using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class Room : MonoBehaviourPunCallbacks
{
	[Header("Room INFO 관련 변수")]
	[SerializeField] private Text roomName;
	[SerializeField] private Button readyBtn;
	public Image[] img_star; // Crown in CharacterBox
	public Image[] img_playerImg; // player_img in CharacterBox
	public Sprite[] sprite_mario; // img_player[i] = sprite_mario[i]
	public Text[] txt_NickName; // NickName in CharacterBox
	public Image[] img_ready; // img_Ready in CharacterBox

	private void Start()
	{
		ActiveFalse_allCharacterBox();
	}

	// 플레이어가 방에 들어왔을 때(=Lobby의 스크립트 OnJoinedRoom()이 실행되었을 때) 이 함수가 실행되어야 함
	[PunRPC]
	public void RoomUISync()
	{
		// 현재 방에 있는 플레이어들의 정보를 배열에 저장함
		Player[] playerlist = PhotonNetwork.PlayerList;
		Debug.Log("playerlistLength는????? " + playerlist.Length);

		for (int i = 0; i < playerlist.Length; i++)
		{
			Debug.Log("for문 들어오는지 i 체크하기!!!!!!!!!!!!!!!!!!!!!! " + i);
			Player player = playerlist[i];
			// 이 플레이어 만든 걸로 UI SetActive(true)
			img_playerImg[i].gameObject.SetActive(true);
			txt_NickName[i].gameObject.SetActive(true);
			// img_star는 방장만 SetActive(true);
			if (player.IsMasterClient)
			{
				img_star[i].gameObject.SetActive(true);
				img_ready[i].gameObject.SetActive(true);
			}

			// 이미지 띄우고
			img_playerImg[i].GetComponent<Image>().sprite = sprite_mario[i];
			// 들어온 캐릭터의 닉네임을 text에 넣어줘야 함
			txt_NickName[i].text = player.NickName;
			
		}
	}

	// 전체 CharacterBox속 img, text 전부 SetActive(false)해주기
	public void ActiveFalse_allCharacterBox()
	{
		for (int i = 0; i < img_star.Length; i++)
		{
			img_star[i].gameObject.SetActive(false);
			img_playerImg[i].gameObject.SetActive(false);
			txt_NickName[i].gameObject.SetActive(false);
			img_ready[i].gameObject.SetActive(false);
		}
	}

	// Ready Btn OnClick Function
	public void OnClick_Ready()
	{
		// 버튼을 클릭하면 자신의 CharacterBox 아래에 있는 Img_Ready를 SetActive(true);
		//img_ready[i].gameObject.SetActive(true);

	}


}
