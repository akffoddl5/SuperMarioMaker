using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class thornBlock : MonoBehaviour
{
	private void OnCollisionStay2D(Collision2D collision)
	{
		if (collision.gameObject.CompareTag("Player"))
		{
			if (!collision.gameObject.GetComponent<PhotonView>().IsMine) return;
			var mario = collision.gameObject.GetComponent<Mario>();
			// star Mode라면 죽지 않도록
			if (mario.isStarMario) return;
			mario.stateMachine.ChangeState(mario.dieState);
		}
	}
}
