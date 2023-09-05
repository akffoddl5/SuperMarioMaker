using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviourPunCallbacks
{

	private static GameManager m_instance; // �̱����� �Ҵ�� static ����
	public bool isGameclear { get; private set; }
	public string gameSceneName;

	// �ܺο��� �̱��� ������Ʈ�� �����ö� ����� ������Ƽ
	public static GameManager instance
	{
		get
		{
			// ���� �̱��� ������ ���� ������Ʈ�� �Ҵ���� �ʾҴٸ�
			if (m_instance == null)
			{
				// ������ GameManager ������Ʈ�� ã�� �Ҵ�
				m_instance = FindObjectOfType<GameManager>();
			}

			// �̱��� ������Ʈ�� ��ȯ
			return m_instance;
		}
	}

	private void Awake()
	{
		// ���� �̱��� ������Ʈ�� �� �ٸ� GameManager ������Ʈ�� �ִٸ�
		if (instance != this)
		{
			
			// �ڽ��� �ı�
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

            // ���常 �������� ��ȯ�� ȣ��
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
	//	// ��� �÷��̾ �غ�Ǿ��ٰ� �����ϰ�, ���� ���������� ��ȯ
		
	//	//string prefabName;
	//	//// ���� ���������� �Ѿ ��, ĳ���� ����
	//	//if(PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("characterName", out object characterPrefabNameObj))
	//	//{
	//	//	prefabName = characterPrefabNameObj.ToString();
	//	//	Debug.Log("prefabNameprefabNameprefabNameprefabNameprefabNameprefabName" + prefabName);
	//	//	PhotonNetwork.Instantiate(prefabName, _startPosition, Quaternion.identity);
	//	//}
	//}
}
