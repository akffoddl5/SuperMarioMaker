using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mario_jump : Mario_state
{
	public float jumpMoveSpeed;
	public Mario_jump(Mario _mario, Mario_stateMachine _stateMachine, string _animBoolName) : base(_mario, _stateMachine, _animBoolName)
	{
	}

	public override void Enter()
	{
		base.Enter();
		//Debug.Log("나 점프했어");
		jumpMoveSpeed = mario.rb.velocity.x;
		mario.rb.AddForce(new Vector2(0, mario.jumpPower), ForceMode2D.Impulse);
	}

	public override void Exit()
	{
		base.Exit();
	}

	public override void Update()
	{
		base.Update();
		Debug.Log(mario.IsGroundDetected());
		// 점프 중 이동 코드
		if (jumpMoveSpeed > 0)
		{
			mario.rb.velocity = new Vector2(xInput* jumpMoveSpeed, mario.rb.velocity.y);
		}
        else
        {
			mario.rb.velocity = new Vector2(xInput* mario.moveSpeed, mario.rb.velocity.y);
        }

        // if 그라운드 밟으면 상태 전환하기 idle
        if (mario.rb.velocity.y <= 0 && mario.IsGroundDetected())
		{
			stateMachine.ChangeState(mario.idleState);
		}
	}
}
