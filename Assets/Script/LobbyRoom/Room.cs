using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using Unity.VisualScripting.FullSerializer;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class Room : MonoBehaviourPunCallbacks
{
	[Header("Room Info")]
	[SerializeField] private Text roomName;
	[SerializeField] private Button readyBtn;

	[Header("CharacterBox Info")]
	public Image[] img_star; // Crown in CharacterBox
	public Image[] img_playerImg; // player_img in CharacterBox
	public Sprite[] sprite_mario;  // img_player[i] = sprite_mario[i]
	public Text[] txt_NickName; // NickName in CharacterBox
	public Image[] img_ready; // img_Ready in CharacterBox

	[Header("Room_map Select 관련 변수")]
	public int maxMapNum;
	public int currentMapNum;
	private Coroutine set_map_img;

	Player[] playerlist;

	int localPlayerIdx;

	bool isReady = false;
	bool isPossibleToStart;
	int readyPersonNum = 0;


	private void Start()
	{
		// map Select info init
		// maxMapNum = 저장된 map의 갯수
		maxMapNum = 2;
		currentMapNum = 1;
		GameObject.Find("Scroll_MapSelect").GetComponent<Scrollbar>().value = 0;
	}

	private void Update()
	{

	}

	// 플레이어가 방에 들어왔을 때(=Lobby의 스크립트 OnJoinedRoom()이 실행되었을 때 이 함수가 실행되어야 함
	[PunRPC]
	public void RoomUISync()
	{
		// 방 이름
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

	#region Btn
	// Btn MapSelect LeftArrow
	public void Btn_MapSelect_left_RPC()
	{
		if (PhotonNetwork.IsMasterClient)
		{
			photonView.RPC("Btn_MapSelect_LeftArrow", RpcTarget.All);
		}
	}
	// Btn MapSelect RightArrow
	public void Btn_MapSelect_right_RPC()
	{
		if (PhotonNetwork.IsMasterClient)
		{
			photonView.RPC("Btn_MapSelect_RightArrow", RpcTarget.All);
		}
	}

	[PunRPC]
	void Btn_MapSelect_LeftArrow()
	{
		if (currentMapNum - 1 <= maxMapNum && currentMapNum - 1 > 0)
		{
			if (set_map_img != null)
				StopCoroutine(set_map_img);
			currentMapNum--;
			set_map_img = StartCoroutine(Cor_Scroll_MapSelect());

		}
	}

	[PunRPC]
	void Btn_MapSelect_RightArrow()
	{
		if (currentMapNum + 1 <= maxMapNum)
		{
			if (set_map_img != null)
				StopCoroutine(set_map_img);
			currentMapNum++;
			set_map_img = StartCoroutine(Cor_Scroll_MapSelect());
		}
	}
	IEnumerator Cor_Scroll_MapSelect()
	{
		var a = GameObject.Find("Scroll_MapSelect").GetComponent<Scrollbar>();
		float current_value = a.value;
		float des_value = 1.0f / (maxMapNum - 1) * (currentMapNum - 1);

		//Debug.Log(current_value + " " + des_value + " " + max_Player + "  " );
		while (true)
		{
			a.value = (a.value + des_value) / 2;
			if (Mathf.Abs(des_value - a.value) < 0.001f)
			{
				break;
			}
			yield return null;
		}
		yield break; ;
	}

	// Btn Ready
	public void OnClick_Ready()
	{
		// 모든 플레이어가 준비 완료상태면서 && 방장이 Start(=방장한테는 Ready가 Start)
		if (PhotonNetwork.IsMasterClient && isPossibleToStart)
		{
			// {"room_state", true} means GameStart
			PhotonNetwork.CurrentRoom.SetCustomProperties(new Hashtable() { { "room_state", true } });

			// 일단 zero로 해두고 나중에 roomoption에 map정보를 담을 수 있게 해서
			// 해당 map의 startposition을 여기에 넣어줄 수 있게 할 수 있음?
			GameManager.instance.GameStart(Vector2.zero);
			//PhotonNetwork.LoadLevel(gameScene);
			return;
			
		}

		var ready = !isReady;
		isReady = ready;
		object[] obj = new object[2] {(object)isReady, (object)localPlayerIdx };

		GetComponent<PhotonView>().RPC("SyncReadyStatus", RpcTarget.AllBuffered, obj);
	}

	// Btn Back
	public void onClick_Back()
	{
		// 방 나갈 거니까 ready false로 만들어주고 레디 상태 알리기
		isReady = false;
		object[] obj = new object[2] { (object)isReady, (object)localPlayerIdx };
		GetComponent<PhotonView>().RPC("SyncReadyStatus", RpcTarget.AllBuffered, obj);

		// Lobby 스크립트의 public override void OnLeftRoom()에서
		// 룸 패널 다시 오른쪽으로 슉 넘겨야 함

		// 방 나가게 처리 하고
		PhotonNetwork.LeaveRoom();

	}
	#endregion


}
