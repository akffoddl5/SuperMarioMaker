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
		// if Room�� ��������
		isPossibleToStart = PossibleToStart();

	}

	// �÷��̾ �濡 ������ ��(=Lobby�� ��ũ��Ʈ OnJoinedRoom()�� ����Ǿ��� ��) �� �Լ��� ����Ǿ�� ��
	[PunRPC]
	public void RoomUISync()
	{
		// �� �̸� ����
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

				/*
				// �̰͵� ���� �Ÿ� �ٲ��� �ϴµ� ������ �� readyBtn�� 
					// ����: �̰� �̷��� �������� int readyPersonNum �̷� �� �����ؼ�
					// readyBtn�� ���� ������ readyPersonNum++
					// if(readyPersonNum == PhotonNetwork.CurrentRoom.PlayerCount)
					// �̷��� ��ư�� �۾��� Start�� �ٲ��
				// �� �ٵ� �̷��� �ƹ��� �� Start ��ų �� �ִ� �� �ƴѰ� ������ ������ readyBtn�� ��������� �ϴµ� 
				 */
				// player�� �����̸� ������ ��ư�� Start�� �������ֵ���
				if (PhotonNetwork.IsMasterClient)
				{
					readyBtn.GetComponentInChildren<Text>().text = "Start";
					//readyBtn.GetComponent<Button>().interactable = false;
					isReady = true;
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
	public void OnClick_Ready()
	{
		var ready = !isReady;
		isReady = ready;
		Object[] obj = new Object[2];

		GetComponent<PhotonView>().RPC("SyncReadyStatus", RpcTarget.AllBuffered, isReady);

		Debug.Log("OnClick_Ready �Լ� ����!!!!!!!!!!!!!!!!!!!!" + isReady);
		
		//int count = 0;
		//for (int i = 0; i < PhotonNetwork.CurrentRoom.Players.Count; i++)
		//{
		//	if (img_ready[i].gameObject.activeSelf == true)
		//		count++;
		//}

		//readyPersonNum = count;

		//// ��� �÷��̾ �غ� �Ϸ���¸鼭 && ������ Ready(=�������״� Start�ϱ�) ������
		//if (PhotonNetwork.IsMasterClient && isPossibleToStart)
		//{
		//	LoadGameScene();
		//}

		//// ��ư�� Ŭ���ϸ� �ڽ��� CharacterBox �Ʒ��� �ִ� Img_Ready�� SetActive(true);
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
		//// RPC�� ����ȭ
		
	}
}
