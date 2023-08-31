using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ExitGames.Client.Photon;
using TMPro;


public class PhotonTest : MonoBehaviourPunCallbacks
{
	[SerializeField] Text m_InputField;
	InputField m_inputField;
	//[SerializeField] TMP_Text m_textConnectLog;
	//[SerializeField] TMP_Text m_textPlayerList;

	Title title;

	void Start()
	{
		// StartScene�� Canvas�� �پ��ִ� Title ��ũ��Ʈ
		title = GameObject.Find("Canvas").GetComponent<Title>();

		Screen.SetResolution(960, 600, false);
        
            //m_InputField = GameObject.Find("InputField").GetComponent<TMP_InputField>();
            //m_textPlayerList = GameObject.Find("TextPlayerList").GetComponent<TMP_Text>();
            //m_textConnectLog = GameObject.Find("TextConnectLog").GetComponent<TMP_Text>();

        //m_textConnectLog.text = "���ӷα�\n";

    }

	public override void OnConnectedToMaster()
	{
			//Debug.Log("onConnectToMaster");
			//RoomOptions options = new RoomOptions();
			//options.MaxPlayers = 5;

		// inputField�� �Է��� �÷��̾��� �г����� ����
		PhotonNetwork.LocalPlayer.NickName = m_InputField.text;

			//PhotonNetwork.JoinOrCreateRoom("Room1", options, null);
			//Debug.Log("�����");
			//PhotonNetwork.JoinLobby();

	}
	// �ڽ��� ������ �� �ݹ�
	public override void OnJoinedRoom()
	{
		Debug.Log("joined");
		updatePlayer();
			//m_textConnectLog.text += m_InputField.text;
			//m_textConnectLog.text += " ���� �濡 �����Ͽ����ϴ�.\n";
	}

	// ������ ������ �� �ݹ�
	public override void OnPlayerEnteredRoom(Player newPlayer)
	{
		updatePlayer();
		Debug.Log("OnPlayerEnteredRoom");
		//m_textConnectLog.text += newPlayer.NickName;
		//m_textConnectLog.text += " ���� �����Ͽ����ϴ�.\n";
	}

	public override void OnPlayerLeftRoom(Player otherPlayer)
	{
		Debug.Log("OnPlayerLeftRoom");
		updatePlayer();
		//m_textConnectLog.text += otherPlayer.NickName;
		//m_textConnectLog.text += " ���� �����Ͽ����ϴ�.\n";
	}

	public void Connect()
	{
		title.LogIn();
		// �ش� ���ӹ������� photon Ŭ����� ����
		
		PhotonNetwork.ConnectUsingSettings();
		
	}

	void updatePlayer()
	{
		//m_textPlayerList.text = "������";
		//for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
		//{
		//	m_textPlayerList.text += "\n";
		//	m_textPlayerList.text += PhotonNetwork.PlayerList[i].NickName;
		//}
	}



	public override void OnJoinedLobby()
	{
		base.OnJoinedLobby();
		//RoomOptions options = new RoomOptions();
		//options.MaxPlayers = 20;

		//PhotonNetwork.JoinOrCreateRoom("Lobby", options, null);
		//Debug.Log("�κ� ����");
	}
}
