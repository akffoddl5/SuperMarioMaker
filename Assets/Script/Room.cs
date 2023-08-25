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
	public Sprite[] sprite_mario;  // img_player[i] = sprite_mario[i]
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

	}

	// 플레이어가 방에 들어왔을 때(=Lobby의 스크립트 OnJoinedRoom()이 실행되었을 때 이 함수가 실행되어야 함
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

				// player가 방장이면 방장의 버튼만 Start로 변경해주도록
				if (PhotonNetwork.IsMasterClient)
				{
					isReady = true;
					readyBtn.GetComponentInChildren<Text>().text = "Start";
					readyBtn.GetComponent<Button>().interactable = false;
				}
			}
			// img_ready[i].gameObject.SetActive(isReady);
			// 이걸 넣어서 문제가 생긴 거 
		}
	}

	// 이거 완성하면 RoomUISync() 지우고 이걸로 바꿀 거
	[PunRPC]
	public void SyncRoomUI()
	{

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
	void SyncReadyStatus(object[] obj)
	{
		//Debug.Log((bool)obj[0] + " " + (int)obj[1] + " RPC댐!!!");
		isReady = (bool)obj[0];
		img_ready[(int)obj[1]].gameObject.SetActive(isReady);

		// isReady 정보를 받아서 켜주는 애가 만약에 마스터 클라이언트라면
		if (PhotonNetwork.IsMasterClient)
		{
			readyPersonNum = 0;
			// 모든 애들의 Ready 상태를 받아와야 한다. => UI 켜짐으로 확인하면 됨
			// isReady로 보는 게 깔끔하긴 한데 거의 일치할 것 같으니까 일단 이렇게 함
			for (int i=0; i < PhotonNetwork.CurrentRoom.PlayerCount; i++)
			{
				if (img_ready[i].gameObject.activeSelf == true)
				{
					readyPersonNum++;
				}
			}
			// 모든 사람이 레디 했는지 체크
			isPossibleToStart = PossibleToStart();
		}
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
		// 모든 플레이어가 준비 완료상태면서 && 방장이 Ready(=방장한테는 Start니까) 누르면
		// 방장이 버튼을 눌렀다는 사실을 다른 플레이어가 알아야 함 RPC
		if (PhotonNetwork.IsMasterClient && isPossibleToStart)
		{
			// 방장만 첫 번째 화면으로 넘어감
			// 당연하지 여기 들어오는 건 방장만 들어오니까
			// 그러면 방장이 시작버튼을 누름 => 다른 애들한테 RPC 해주는 방법밖에 없나?
			//PhotonNetwork.LoadLevel(0);
			GetComponent<PhotonView>().RPC("LoadScene", RpcTarget.All);
			return;
			
		}

		var ready = !isReady;
		isReady = ready;
		object[] obj = new object[2] {(object)isReady, (object)localPlayerIdx };

		GetComponent<PhotonView>().RPC("SyncReadyStatus", RpcTarget.AllBuffered, obj);
	}

	[PunRPC]
	private void LoadScene()
	{
		PhotonNetwork.LoadLevel(gameScene);
	}
}
