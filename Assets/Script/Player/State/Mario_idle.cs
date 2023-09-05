using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Mario_idle : Mario_state
{
	public Mario_idle(Mario mario, Mario_stateMachine stateMachine, string animBool) : base(mario, stateMachine, animBool)
	{
	}

	public override void Enter()
	{
		base.Enter();
		mario.anim.SetBool("Jump", false);
		mario.rb.velocity = Vector2.zero;
		stateTimer2 = 0.5f; // fireTimer
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
		if (Input.GetKey(KeyCode.DownArrow) && mario.marioMode > 0 && 1==2)
		{
			stateMachine.ChangeState(mario.sitDown);
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

        if (mario.rb.velocity.y <= 0 && mario.IsPlayerDetected())
		{
			stateMachine.ChangeState(mario.stampState);
		}
	}
}
