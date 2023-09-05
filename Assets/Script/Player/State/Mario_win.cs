using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class Mario_win : Mario_state
{
	GameObject flag; // ±ê¹ß
	float flagYPos; // ±ê¹ßÀÇ 
	float offsetY;
	Vector2 moveVec;


	public Mario_win(Mario _mario, Mario_stateMachine _stateMachine, string _animBoolName) : base(_mario, _stateMachine, _animBoolName)
	{
	}

	public override void Enter()
	{
		base.Enter();
		flag = GameObject.Find("flag_finish");
		//flagYPos = flag.transform.position.y - flag.GetComponent<Flag>().pos2.y;
		offsetY = flag.transform.position.y - mario.transform.position.y;
		//Debug.Log(offsetY);
		//mario.rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
		mario.rb.constraints = RigidbodyConstraints2D.FreezeAll;
	}

	public override void Update()
	{

		base.Update();
		if (mario.IsGroundDetected())
		{
			mario.collider.enabled = true;
			return;
		}
		//Debug.Log(flag.transform.position.y - mario.transform.position.y);
		moveVec = new Vector2(mario.transform.position.x, flag.transform.position.y - offsetY);
		mario.transform.position = moveVec;
		//yield return new WaitForSeconds(0.05f);

	}

	public override void Exit()
	{
		base.Exit();
	}
}