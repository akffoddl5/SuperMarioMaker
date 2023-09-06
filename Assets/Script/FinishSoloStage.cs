using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class FinishSoloStage : MonoBehaviour
{
	//public bool isFinishGame = false;
	bool isCoroutineStart = false;

	Vector2 minusVector = new Vector2(0.5f, 0.5f);
	Vector2 finishScale = new Vector2(3, 3);
	//Vector2 finishPosition; // 1���� position

	public GameObject obj_winTextCanvas; // ������
	[HideInInspector] public string winnerName;
	//Text winTxt;

	// Start is called before the first frame update
	void Start()
	{
		//transform.localPosition = Vector3.zero; // �ӽ÷� �־��� �ڵ�, ������ ��ġ���� �����Ǿ�� �ϴϱ� �ּ�ó��
		transform.localScale = new Vector2(40, 40);
	}

	// Update is called once per frame
	void Update()
	{
		if (!isCoroutineStart)
		{
			isCoroutineStart = true;
			StartCoroutine(FinishGame_EF());
		}
	}
	IEnumerator FinishGame_EF()
	{
		while (transform.localScale.x >= finishScale.x)
		{
			Vector2 tmp = new Vector2(transform.localScale.x - minusVector.x, transform.localScale.y - minusVector.y);
			transform.localScale = tmp;
			//Debug.Log("IEnumerator FinishGame_EF() ����!!!" + transform.localScale);
			yield return new WaitForSeconds(0.01f);
		}
		// PlayerName win
		var a = Instantiate(obj_winTextCanvas, transform.position, Quaternion.identity);

		// �̰� RPC �ؾ� ��
		a.GetComponentInChildren<Text>().text = WIndowManager.instance.winnerNickName + " WIN ";


		yield return new WaitForSeconds(4f);

		// ������ ���� ����
		PhotonNetwork.CurrentRoom.SetCustomProperties(new Hashtable() { { "room_state", false } });
		SceneManager.LoadScene("LobbyScene");
	}
}
