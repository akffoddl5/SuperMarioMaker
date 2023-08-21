using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mario_idle : Mario_state
{
	public Mario_idle(Mario mario, Mario_stateMachine stateMachine, string animBool) : base(mario, stateMachine, animBool)
	{
	}

	public override void Enter()
	{
		base.Enter();
		rb.velocity = Vector2.zero;
	}

	public override void Exit()
	{
		base.Exit();
	}

	public override void Update()
	{
		base.Update();
		Debug.Log("idle");

		if (xInput != 0)
		{
			stateMachine.ChangeState(mario.runState);
		}

	}
}
