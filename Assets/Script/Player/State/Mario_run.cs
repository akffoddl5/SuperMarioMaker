using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mario_run : Mario_state
{
	float lastXSpeed;
	public Mario_run(Mario mario, Mario_stateMachine stateMachine, string animBool) : base(mario, stateMachine, animBool)
	{
	}

	public override void Enter()
	{
		base.Enter();
		// 원래 마리오가 점프 하다가 달릴 때 속도가 유지됐던 거 같다
		//mario.rb.velocity = new Vector2(mario.rb.velocity.x, mario.rb.velocity.y);
	}

	public override void Exit()
	{
		base.Exit();
	}

	public override void Update()
	{
		base.Update();

		mario.rb.velocity = new Vector2(xInput * mario.runSpeed, mario.rb.velocity.y);

		if (xInput == 0)
		{
			stateMachine.ChangeState(mario.slideState);
		}

		lastXSpeed = mario.rb.velocity.x;

		//// Slide
		//if (mario.rb.velocity.x >= mario.runSpeed && xInput < 0)
		//{
		//	stateMachine.ChangeState(mario.slideState);
		//}
		//else if (mario.rb.velocity.x <= -mario.runSpeed && xInput > 0)
		//{
		//	stateMachine.ChangeState(mario.slideState);
		//}

		// Jump
		if (Input.GetKeyDown(KeyCode.Space)) 
		{
			stateMachine.ChangeState(mario.jumpState);
		}

		// Change State Run to Walk
		if (Input.GetKeyUp(KeyCode.Z))
		{
			stateMachine.ChangeState(mario.walkState);
		}
	}
}
