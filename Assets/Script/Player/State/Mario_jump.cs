using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mario_jump : Mario_state
{
	public float jumpMoveSpeed;
	float last_velocity_y = 0;
	public Mario_jump(Mario _mario, Mario_stateMachine _stateMachine, string _animBoolName) : base(_mario, _stateMachine, _animBoolName)
	{
	}

	

	public override void Enter()
	{
		base.Enter();
		last_velocity_y = 0;
		stateTimer = 13 * Time.deltaTime;
		//Debug.Log("�� �����߾�");
		jumpMoveSpeed = mario.rb.velocity.x;
		mario.rb.velocity = new Vector2(mario.rb.velocity.x, 0.001f);
		mario.rb.AddForce(new Vector2(0, mario.jumpPower), ForceMode2D.Impulse);
		//last_velocity_y = mario.rb.velocity.y;
	}

	public override void Exit()
	{
		base.Exit();
	}
	public override void Update()
	{
		base.Update();
		//Debug.Log(mario.IsGroundDetected());
		if (Input.GetKeyUp(KeyCode.Space) && stateTimer > 0 )
		{
			mario.rb.AddForce(new Vector2(0, -5f), ForceMode2D.Impulse);
			stateTimer = -1;
		}

		//if (Input.GetKey(KeyCode.Space) && stateTimer > 0 && stateTimer < 3 * Time.deltaTime)
		//{
		//	Debug.Log("����!!!");
		//	mario.rb.AddForce(new Vector2(0, 5f), ForceMode2D.Impulse);
		//	stateTimer = -1;
		//}

		// ���� �� �̵� �ڵ�
		if (jumpMoveSpeed != 0)
		{
			mario.rb.velocity = new Vector2(xInput* Mathf.Abs(jumpMoveSpeed), mario.rb.velocity.y);
		}
        else
        {
			mario.rb.velocity = new Vector2(xInput* mario.moveSpeed, mario.rb.velocity.y);
        }

		// if �׶��� ������ ���� ��ȯ�ϱ� idle
		if (mario.rb.velocity.y <= 0 && mario.IsGroundDetected())
		{
			stateMachine.ChangeState(mario.idleState);
		}else if (mario.rb.velocity.y <= 0 && mario.IsPlayerDetected()) {
			Debug.Log("jump �� �ٲ���");
			stateMachine.ChangeState(mario.jumpState);
		}
	}
}
