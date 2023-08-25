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
		if (!PV.IsMine)
		{
			return;
		}
		base.Enter();
		last_velocity_y = 0;
		stateTimer = 103 * Time.deltaTime;
		//Debug.Log("나 점프했어");
		jumpMoveSpeed = mario.rb.velocity.x;
		mario.rb.velocity = new Vector2(mario.rb.velocity.x, 0.001f);
        mario.rb.velocity = new Vector2(mario.rb.velocity.y, 0.001f);
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
		if (Input.GetKeyUp(KeyCode.Space) && stateTimer > 0  )
		{
			mario.rb.AddForce(new Vector2(0, -5f), ForceMode2D.Impulse);
			stateTimer = -1;
		}

		//if (Input.GetKey(KeyCode.Space) && stateTimer > 0 && stateTimer < 3 * Time.deltaTime)
		//{
		//	Debug.Log("높점!!!");
		//	mario.rb.AddForce(new Vector2(0, 5f), ForceMode2D.Impulse);
		//	stateTimer = -1;
		//}

		// 점프 중 이동 코드
		if (jumpMoveSpeed != 0)
		{
			mario.rb.velocity = new Vector2(xInput* Mathf.Abs(jumpMoveSpeed), mario.rb.velocity.y);
		}
        else
        {
			mario.rb.velocity = new Vector2(xInput* mario.moveSpeed, mario.rb.velocity.y);
        }

		// if 그라운드 밟으면 상태 전환하기 idle
		if (mario.rb.velocity.y <= 0 && mario.IsGroundDetected())
		{
			Debug.Log("A");
			stateMachine.ChangeState(mario.idleState);
		} else if (mario.rb.velocity.y <= 0 && mario.IsPlayerDetected() != null) {
			Debug.Log("B");
			//Debug.Log("jump 로 바꾸자" + mario.IsPlayerDetected().name);
			stateMachine.ChangeState(mario.jumpState);
			var kickedMario = mario.IsPlayerDetected();
			kickedMario.GetComponent<Mario>().stateMachine.currentState = mario.idleState;
			//Debug.Log(kickedMario.gameObject.name + " 얘가 킥드야" + kickedMario.GetComponent<Mario>().gameObject.name);
			//Debug.Log(kickedMario.GetComponent<Mario>().stateMachine.currentState.animBoolName);
			kickedMario.GetComponent<Mario>().stateMachine.ChangeState(kickedMario.GetComponent<Mario>().kickedState);
		} else if (mario.rb.velocity.y <= 0 && mario.IsEnemyDetected() != null)
		{
			Debug.Log("C");
			Debug.Log("적을 밟아서 점프");
			stateMachine.ChangeState(mario.jumpState);
		}
	}
}
