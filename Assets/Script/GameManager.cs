using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviourPunCallbacks
{

	private static GameManager m_instance; // 싱글톤이 할당될 static 변수
	public bool isGameclear { get; private set; }
	public string gameSceneName;

	// 외부에서 싱글톤 오브젝트를 가져올때 사용할 프로퍼티
	public static GameManager instance
	{
		get
		{
			// 만약 싱글톤 변수에 아직 오브젝트가 할당되지 않았다면
			if (m_instance == null)
			{
				// 씬에서 GameManager 오브젝트를 찾아 할당
				m_instance = FindObjectOfType<GameManager>();
			}

			// 싱글톤 오브젝트를 반환
			return m_instance;
		}
	}

	private void Awake()
	{
		// 씬에 싱글톤 오브젝트가 된 다른 GameManager 오브젝트가 있다면
		if (instance != this)
		{
			
			// 자신을 파괴
			Destroy(gameObject);
		}
	}

	public void GameStart(Vector2 _startPosition)
	{
		Vector2 startPos = _startPosition;
		if (PhotonNetwork.IsMasterClient)
		{
			Debug.Log(startPos);
            PhotonNetwork.LoadLevel(gameSceneName);

			//StartCoroutine(Delay(startPos));

            // 방장만 스테이지 전환을 호출
            //photonView.RPC("RPC_LoadGameStage", RpcTarget.All, startPos);
		}
	}

	//IEnumerator Delay(Vector2 startPos)
	//{
	//	yield return new WaitForSeconds(5);
 //       photonView.RPC("RPC_LoadGameStage", RpcTarget.All, startPos);
 //   }
	
	//[PunRPC]
	//private void RPC_LoadGameStage(Vector2 _startPosition)
	//{
 //       PhotonNetwork.Instantiate("Prefabs/Mario", _startPosition, Quaternion.identity);
	//	// 모든 플레이어가 준비되었다고 가정하고, 다음 스테이지로 전환
		
	//	//string prefabName;
	//	//// 다음 스테이지로 넘어갈 때, 캐릭터 생성
	//	//if(PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("characterName", out object characterPrefabNameObj))
	//	//{
	//	//	prefabName = characterPrefabNameObj.ToString();
	//	//	Debug.Log("prefabNameprefabNameprefabNameprefabNameprefabNameprefabName" + prefabName);
	//	//	PhotonNetwork.Instantiate(prefabName, _startPosition, Quaternion.identity);
	//	//}
	//}
}
