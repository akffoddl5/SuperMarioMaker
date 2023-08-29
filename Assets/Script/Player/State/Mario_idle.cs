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
		mario.rb.velocity = Vector2.zero;
	}

	public override void Exit()
	{
		base.Exit();
	}

	public override void Update()
	{
		base.Update();

		if (xInput != 0)
		{
			stateMachine.ChangeState(mario.walkState);
		}

		// а║га
		if (Input.GetKeyDown(KeyCode.Space))
		{
			stateMachine.ChangeState(mario.jumpState);
		}
		// ╬и╠Б
		if (Input.GetKey(KeyCode.DownArrow) && marioMode > 0)
		{
			stateMachine.ChangeState(mario.sitDown);
		}

		if (mario.rb.velocity.y <= 0 && mario.IsPlayerDetected())
		{
			stateMachine.ChangeState(mario.stampState);
		}
	}
}
