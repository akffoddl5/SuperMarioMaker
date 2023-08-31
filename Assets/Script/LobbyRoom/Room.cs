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

	// �÷��̾ �濡 ������ ��(=Lobby�� ��ũ��Ʈ OnJoinedRoom()�� ����Ǿ��� �� �� �Լ��� ����Ǿ�� ��
	[PunRPC]
	public void RoomUISync()
	{
		// �� �̸�
		roomName.text = PhotonNetwork.CurrentRoom.Name;
		ActiveFalse_allCharacterBox();
		
		// ���� �濡 �ִ� �÷��̾���� ������ �迭�� ������
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

			// �׳� if (PhotonNetwork.IsMasterClient)��� �ٲٸ�
			// �������״� ��� �÷��̾ �� ����ǥ��+img_ready ���� ���°� ��
			if (player.IsMasterClient)
			{
				img_star[i].gameObject.SetActive(true);
				img_ready[i].gameObject.SetActive(true);

				// player�� �����̸� ������ ��ư�� Start�� �������ֵ���
				if (PhotonNetwork.IsMasterClient)
				{
					isReady = true;
					readyBtn.GetComponentInChildren<Text>().text = "Start";
					readyBtn.GetComponent<Button>().interactable = false;
				}
			}
		}
	}

	// ��ü CharacterBox�� img, text ���� SetActive(false)���ֱ�
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
		//Debug.Log((bool)obj[0] + " " + (int)obj[1] + " RPC��!!!");
		isReady = (bool)obj[0];
		img_ready[(int)obj[1]].gameObject.SetActive(isReady);

		// isReady ������ �޾Ƽ� ���ִ� �ְ� ���࿡ ������ Ŭ���̾�Ʈ���
		if (PhotonNetwork.IsMasterClient)
		{
			readyPersonNum = 0;
			// ��� �ֵ��� Ready ���¸� �޾ƿ;� �Ѵ�. => UI �������� Ȯ���ϸ� ��
			// isReady�� ���� �� ����ϱ� �ѵ� ���� ��ġ�� �� �����ϱ� �ϴ� �̷��� ��
			for (int i=0; i < PhotonNetwork.CurrentRoom.PlayerCount; i++)
			{
				if (img_ready[i].gameObject.activeSelf == true)
				{
					readyPersonNum++;
				}
			}
			// ��� ����� ���� �ߴ��� üũ
			isPossibleToStart = PossibleToStart();
		}
	}

	// Ready Btn OnClick Function
	// IsMasterClient && AllPlayerIsReady        
	public bool PossibleToStart()
	{
		// ��� �÷��̾ �غ� �Ϸ���¸鼭 && ������ Ready(=�������״� Start�ϱ�)���
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

	// Btn Ready
	public void OnClick_Ready()
	{
		// ��� �÷��̾ �غ� �Ϸ���¸鼭 && ������ Start(=�������״� Ready�� Start)
		if (PhotonNetwork.IsMasterClient && isPossibleToStart)
		{
			// {"room_state", true} means GameStart
			PhotonNetwork.CurrentRoom.SetCustomProperties(new Hashtable() { { "room_state", true } });

			// �ϴ� zero�� �صΰ� ���߿� roomoption�� map������ ���� �� �ְ� �ؼ�
			// �ش� map�� startposition�� ���⿡ �־��� �� �ְ� �� �� ����?
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
		// �� ���� �Ŵϱ� ready false�� ������ְ� ���� ���� �˸���
		isReady = false;
		object[] obj = new object[2] { (object)isReady, (object)localPlayerIdx };
		GetComponent<PhotonView>().RPC("SyncReadyStatus", RpcTarget.AllBuffered, obj);

		// Lobby ��ũ��Ʈ�� public override void OnLeftRoom()����
		// �� �г� �ٽ� ���������� �� �Ѱܾ� ��

		// �� ������ ó�� �ϰ�
		PhotonNetwork.LeaveRoom();

	}
	#endregion


}