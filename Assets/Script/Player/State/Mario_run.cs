using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mario_run : Mario_state
{
	public Mario_run(Mario mario, Mario_stateMachine stateMachine, string animBool) : base(mario, stateMachine, animBool)
	{
	}

	public override void Enter()
	{
		base.Enter();
	}

	public override void Exit()
	{
		base.Exit();
	}

	public override void Update()
	{
		base.Update();
		Debug.Log("move");
		rb.velocity = new Vector2(xInput * mario.moveSpeed, rb.velocity.y);
		if (xInput == 0)
		{
			stateMachine.ChangeState(mario.idleState);
		}
	}
}
