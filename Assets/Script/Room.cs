using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using Unity.VisualScripting.FullSerializer;

public class Room : MonoBehaviourPunCallbacks
{
	[Header("Room Info")]
	[SerializeField] private Text roomName;
	[SerializeField] private Button readyBtn;
	public string gameScene; // OnClickStart change thisScene to gameScene

	[Header("CharacterBox Info")]
	public Image[] img_star; // Crown in CharacterBox
	public Image[] img_playerImg; // player_img in CharacterBox
	public Sprite[] sprite_mario; // img_player[i] = sprite_mario[i]
	public Text[] txt_NickName; // NickName in CharacterBox
	public Image[] img_ready; // img_Ready in CharacterBox

	Player[] playerlist;

	int localPlayerIdx;

	bool isReady = false;
	bool isPossibleToStart;
	int readyPersonNum = 0;


	private void Start()
	{
		
	}

	private void Update()
	{
		// if Room에 들어왔으면
		isPossibleToStart = PossibleToStart();

	}

	// 플레이어가 방에 들어왔을 때(=Lobby의 스크립트 OnJoinedRoom()이 실행되었을 때) 이 함수가 실행되어야 함
	[PunRPC]
	public void RoomUISync()
	{
		// 방 이름 변경
		roomName.text = PhotonNetwork.CurrentRoom.Name;

		ActiveFalse_allCharacterBox();
		
		// 현재 방에 있는 플레이어들의 정보를 배열에 저장함
		playerlist = PhotonNetwork.PlayerList;
		//Debug.Log("playerlistLength????? " + playerlist.Length);

		for (int i = 0; i < playerlist.Length; i++)
		{
			//Debug.Log("for loop Check i: " + i);
			Player player = playerlist[i];

			if (playerlist[i].IsLocal)
			{
				localPlayerIdx = i;
			}

			// CharacterBox Img, NickName Setting
			img_playerImg[i].gameObject.SetActive(true);
			img_playerImg[i].GetComponent<Image>().sprite = sprite_mario[i];
			txt_NickName[i].gameObject.SetActive(true);
			txt_NickName[i].text = player.NickName;

			// 그냥 if (PhotonNetwork.IsMasterClient)라고 바꾸면
			// 방장한테는 모든 플레이어가 다 방장표시+img_ready 켜진 상태가 됨
			if (player.IsMasterClient)
			{
				img_star[i].gameObject.SetActive(true);
				img_ready[i].gameObject.SetActive(true);

				/*
				// 이것도 방장 거만 바뀌어야 하는데 방장일 때 readyBtn의 
					// 만준: 이거 이렇게 하지말고 int readyPersonNum 이런 거 선언해서
					// readyBtn을 누를 때마다 readyPersonNum++
					// if(readyPersonNum == PhotonNetwork.CurrentRoom.PlayerCount)
					// 이러면 버튼의 글씨가 Start로 바뀌도록
				// 흠 근데 이러면 아무나 다 Start 시킬 수 있는 거 아닌가 어차피 방장의 readyBtn을 구분해줘야 하는데 
				 */
				// player가 방장이면 방장의 버튼만 Start로 변경해주도록
				if (PhotonNetwork.IsMasterClient)
				{
					readyBtn.GetComponentInChildren<Text>().text = "Start";
					//readyBtn.GetComponent<Button>().interactable = false;
					isReady = true;
				}
			}

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

	[PunRPC]
	void SyncReadyStatus(bool readyStatus)
	{
		isReady = readyStatus;
		img_ready[localPlayerIdx].gameObject.SetActive(isReady);
	}

	[PunRPC]
	void LoadGameScene()
	{
		SceneManager.LoadScene(gameScene);
	}

	// Ready Btn OnClick Function
	// IsMasterClient && AllPlayerIsReady
	public bool PossibleToStart()
	{
		// 모든 플레이어가 준비 완료상태면서 && 방장이 Ready(=방장한테는 Start니까)라면
		if (PhotonNetwork.IsMasterClient)
		{
			if (readyPersonNum == PhotonNetwork.CurrentRoom.PlayerCount)
			{
				readyBtn.GetComponent<Button>().interactable = true;
				return true;
			}
			else
			{
				readyBtn.GetComponent<Button>().interactable = false;
				return false;
			}
		}
		return false;
	}
	public void OnClick_Ready()
	{
		var ready = !isReady;
		isReady = ready;
		Object[] obj = new Object[2];

		GetComponent<PhotonView>().RPC("SyncReadyStatus", RpcTarget.AllBuffered, isReady);

		Debug.Log("OnClick_Ready 함수 들어옴!!!!!!!!!!!!!!!!!!!!" + isReady);
		
		//int count = 0;
		//for (int i = 0; i < PhotonNetwork.CurrentRoom.Players.Count; i++)
		//{
		//	if (img_ready[i].gameObject.activeSelf == true)
		//		count++;
		//}

		//readyPersonNum = count;

		//// 모든 플레이어가 준비 완료상태면서 && 방장이 Ready(=방장한테는 Start니까) 누르면
		//if (PhotonNetwork.IsMasterClient && isPossibleToStart)
		//{
		//	LoadGameScene();
		//}

		//// 버튼을 클릭하면 자신의 CharacterBox 아래에 있는 Img_Ready를 SetActive(true);
		//if (!isReady)
		//{
		//	//isReady = true;
		//	readyPersonNum++;
		//}
		//else
		//{
		//	//isReady = false;
		//	readyPersonNum--;
		//}
		//// RPC로 동기화
		
	}
}
