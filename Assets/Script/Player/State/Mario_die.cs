using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mario_die : Mario_state
{
	private float dieJumpPower = 10;
	public Mario_die(Mario _mario, Mario_stateMachine _stateMachine, string _animBoolName) : base(_mario, _stateMachine, _animBoolName)
	{
	}

	public override void Enter()
	{
		base.Enter();
		Debug.Log(PV.name + " DieState 들어옴");
		mario.rb.AddForce(new Vector2(0, dieJumpPower), ForceMode2D.Impulse);
		Debug.Log(PV.name + " DieState AddForce 끝");

		// collider 끄기 => 리스폰 돼야 하니까 리스폰 되면 다시 켜줘야 함
		mario.GetComponent<CapsuleCollider2D>().enabled = false;
		Debug.Log(PV.name + " DieState Collider2D 끝");
	}

	public override void Exit()
	{
		base.Exit();
		Debug.Log(PV.name + " DieState 나감");
	}

	public override void Update()
	{
		base.Update();

	}

	//void MarioJumpDie()
	//{

	//}

}
