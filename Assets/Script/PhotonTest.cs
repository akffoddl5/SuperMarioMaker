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
		// StartScene의 Canvas에 붙어있는 Title 스크립트
		title = GameObject.Find("Canvas").GetComponent<Title>();

		Screen.SetResolution(960, 600, false);
        
            //m_InputField = GameObject.Find("InputField").GetComponent<TMP_InputField>();
            //m_textPlayerList = GameObject.Find("TextPlayerList").GetComponent<TMP_Text>();
            //m_textConnectLog = GameObject.Find("TextConnectLog").GetComponent<TMP_Text>();

        //m_textConnectLog.text = "접속로그\n";

    }

	public override void OnConnectedToMaster()
	{
			//Debug.Log("onConnectToMaster");
			//RoomOptions options = new RoomOptions();
			//options.MaxPlayers = 5;

		// inputField에 입력한 플레이어의 닉네임을 저장
		PhotonNetwork.LocalPlayer.NickName = m_InputField.text;

			//PhotonNetwork.JoinOrCreateRoom("Room1", options, null);
			//Debug.Log("룸생성");
			//PhotonNetwork.JoinLobby();

	}
	// 자신이 들어왔을 때 콜백
	public override void OnJoinedRoom()
	{
		Debug.Log("joined");
		updatePlayer();
			//m_textConnectLog.text += m_InputField.text;
			//m_textConnectLog.text += " 님이 방에 참가하였습니다.\n";
	}

	// 누군가 들어왔을 때 콜백
	public override void OnPlayerEnteredRoom(Player newPlayer)
	{
		updatePlayer();
		Debug.Log("OnPlayerEnteredRoom");
		//m_textConnectLog.text += newPlayer.NickName;
		//m_textConnectLog.text += " 님이 입장하였습니다.\n";
	}

	public override void OnPlayerLeftRoom(Player otherPlayer)
	{
		Debug.Log("OnPlayerLeftRoom");
		updatePlayer();
		//m_textConnectLog.text += otherPlayer.NickName;
		//m_textConnectLog.text += " 님이 퇴장하였습니다.\n";
	}

	public void Connect()
	{
		title.LogIn();
		// 해당 게임버전으로 photon 클라우드로 연결
		
		PhotonNetwork.ConnectUsingSettings();
		
	}

	void updatePlayer()
	{
		//m_textPlayerList.text = "접속자";
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
		//Debug.Log("로비 입장");
	}
}
