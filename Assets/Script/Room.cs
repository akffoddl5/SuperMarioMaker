using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class Room : MonoBehaviourPunCallbacks
{
	[Header("Room INFO ���� ����")]
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

	// �÷��̾ �濡 ������ ��(=Lobby�� ��ũ��Ʈ OnJoinedRoom()�� ����Ǿ��� ��) �� �Լ��� ����Ǿ�� ��
	[PunRPC]
	public void RoomUISync()
	{
		// ���� �濡 �ִ� �÷��̾���� ������ �迭�� ������
		Player[] playerlist = PhotonNetwork.PlayerList;
		Debug.Log("playerlistLength��????? " + playerlist.Length);

		for (int i = 0; i < playerlist.Length; i++)
		{
			Debug.Log("for�� �������� i üũ�ϱ�!!!!!!!!!!!!!!!!!!!!!! " + i);
			Player player = playerlist[i];
			// �� �÷��̾� ���� �ɷ� UI SetActive(true)
			img_playerImg[i].gameObject.SetActive(true);
			txt_NickName[i].gameObject.SetActive(true);
			// img_star�� ���常 SetActive(true);
			if (player.IsMasterClient)
			{
				img_star[i].gameObject.SetActive(true);
				img_ready[i].gameObject.SetActive(true);
			}

			// �̹��� ����
			img_playerImg[i].GetComponent<Image>().sprite = sprite_mario[i];
			// ���� ĳ������ �г����� text�� �־���� ��
			txt_NickName[i].text = player.NickName;
			
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

	// Ready Btn OnClick Function
	public void OnClick_Ready()
	{
		// ��ư�� Ŭ���ϸ� �ڽ��� CharacterBox �Ʒ��� �ִ� Img_Ready�� SetActive(true);
		//img_ready[i].gameObject.SetActive(true);

	}


}
