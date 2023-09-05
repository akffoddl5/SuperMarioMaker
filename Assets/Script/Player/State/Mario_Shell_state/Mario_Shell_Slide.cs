using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Mario_Shell_Slide : Mario_Shell_State
{
	float enterSpeed;
	float slideSpeed = 0.90f;

    public Mario_Shell_Slide(Mario _mario, Mario_stateMachine _stateMachine, string _animBoolName) : base(_mario, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
	{
		base.Enter();
		stateTimer = 40f * Time.deltaTime;
		stateTimer2 = 0.2f;
		//enterSpeed = mario.rb.velocity.x;
	}

	public override void Exit()
	{
		base.Exit();
	}

	public override void Update()
	{
		base.Update();
		mario.rb.velocity = new Vector2(mario.lastXSpeed * slideSpeed, mario.rb.velocity.y);
		if (stateTimer2 < 0)
		{
			mario.lastXSpeed *= 0.8f;
			stateTimer2 = 0.2f;
		}
		if (stateTimer < 0)
		{

			stateMachine.ChangeState(mario.mario_Shell_Idle);
		}
	}
}
