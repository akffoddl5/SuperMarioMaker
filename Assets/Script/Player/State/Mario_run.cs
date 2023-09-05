using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mario_run : Mario_state
{
	
	float run_rate;
	float run_speed;
	float run_max_speed;
	
	public Mario_run(Mario mario, Mario_stateMachine stateMachine, string animBool) : base(mario, stateMachine, animBool)
	{
	}

	public override void Enter()
	{
		base.Enter();
		run_max_speed = mario.runSpeed * 2.5f;
		run_speed = mario.runSpeed;
		run_rate = 1;

		// 원래 마리오가 점프 하다가 달릴 때 속도가 유지됐던 거 같다
		//mario.rb.velocity = new Vector2(mario.rb.velocity.x, mario.rb.velocity.y);
		stateTimer2 = 0.5f; // fireTimer
	}

	public void Run_Speed_Increase()
	{
		if (run_speed * 1.5f >= run_max_speed) return;
		run_speed *= 1.1f;
		run_rate *= 1.1f;
		mario.anim.speed = run_rate;
		mario.lastXSpeed = mario.rb.velocity.x;
		//Debug.Log("run 빨라지는중.." + run_speed + " " + run_rate);
	}

	public override void Exit()
	{
		base.Exit();
		mario.anim.speed = 1;
	}

	public override void Update()
	{
		base.Update();

		if (stateTimer < 0)
		{
			Run_Speed_Increase();
			stateTimer = 0.1f;
		}

		mario.rb.velocity = new Vector2(xInput * run_speed, mario.rb.velocity.y);

		if (xInput == 0)
		{
			if (run_rate > 1.4f)
				stateMachine.ChangeState(mario.slideState);
			else
				stateMachine.ChangeState(mario.idleState);

			
			
		}

		//FIre
		if (Input.GetKeyDown(KeyCode.X) && PV.IsMine && (mario.marioMode == 2) && stateTimer2 <= 0f)
		{
			stateTimer2 = 0.5f; // fireTimer
			if (isFlip)
            {
                var a = PhotonNetwork.Instantiate("Prefabs/Fire_Bullet", mario.obj_bulletGeneratorA.position, Quaternion.identity);
                a.GetComponent<Fire_Bullet>().faceDir = -1;
            }
            else
            {
                var a = PhotonNetwork.Instantiate("Prefabs/Fire_Bullet", mario.obj_bulletGeneratorB.position, Quaternion.identity);
                a.GetComponent<Fire_Bullet>().faceDir = 1;
            }
        }

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

		if (mario.rb.velocity.y <= 0 && mario.IsPlayerDetected())
		{
			stateMachine.ChangeState(mario.jumpState);
		}


	}
}
