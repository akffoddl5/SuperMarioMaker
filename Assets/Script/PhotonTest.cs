using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ExitGames.Client.Photon;


public class PhotonTest : MonoBehaviourPunCallbacks
{
	[SerializeField] InputField m_InputField;
	[SerializeField] Text m_textConnectLog;
	[SerializeField] Text m_textPlayerList;

	

	void Start()
	{
		Screen.SetResolution(960, 600, false);

		m_InputField = GameObject.Find("InputField").GetComponent<InputField>();
		m_textPlayerList = GameObject.Find("TextPlayerList").GetComponent<Text>();
		m_textConnectLog = GameObject.Find("TextConnectLog").GetComponent<Text>();

		m_textConnectLog.text = "���ӷα�\n";
	}

	public override void OnConnectedToMaster()
	{
		RoomOptions options = new RoomOptions();
		options.MaxPlayers = 5;

		PhotonNetwork.LocalPlayer.NickName = m_InputField.text;
		PhotonNetwork.JoinOrCreateRoom("Room1", options, null);
		Debug.Log("�����");

	}
	public override void OnJoinedRoom()
	{
		Debug.Log("joined");
		updatePlayer();
		m_textConnectLog.text += m_InputField.text;
		m_textConnectLog.text += " ���� �濡 �����Ͽ����ϴ�.\n";
	}

	public override void OnPlayerEnteredRoom(Player newPlayer)
	{
		updatePlayer();
		Debug.Log("OnPlayerEnteredRoom");
		m_textConnectLog.text += newPlayer.NickName;
		m_textConnectLog.text += " ���� �����Ͽ����ϴ�.\n";
	}

	public override void OnPlayerLeftRoom(Player otherPlayer)
	{
		Debug.Log("OnPlayerLeftRoom");
		updatePlayer();
		m_textConnectLog.text += otherPlayer.NickName;
		m_textConnectLog.text += " ���� �����Ͽ����ϴ�.\n";
	}

	public void Connect()
	{
		PhotonNetwork.ConnectUsingSettings();
		
	}

	void updatePlayer()
	{
		m_textPlayerList.text = "������";
		for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
		{
			m_textPlayerList.text += "\n";
			m_textPlayerList.text += PhotonNetwork.PlayerList[i].NickName;
		}
	}

	public override void OnRoomListUpdate(List<RoomInfo> roomList)
	{
		base.OnRoomListUpdate(roomList);

		var g = GetComponent<PhotonView>();
		if (g.IsMine)
		{

		}
	}
}
