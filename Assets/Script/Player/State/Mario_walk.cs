using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mario_walk : Mario_state
{
	public Mario_walk(Mario _mario, Mario_stateMachine _stateMachine, string _animBoolName) : base(_mario, _stateMachine, _animBoolName)
	{
	}

	public override void Enter()
	{
		base.Enter();
		stateTimer2 = 0.5f; // fireTimer
	}

	public override void Exit()
	{
		base.Exit();
	}

	public override void Update()
	{
		base.Update();


		mario.rb.velocity = new Vector2(xInput * mario.moveSpeed, mario.rb.velocity.y);

		//Idle
		if (xInput == 0)
		{
			stateMachine.ChangeState(mario.idleState);
		}

		// Run
		if(Input.GetKey(KeyCode.Z))
		{
			stateMachine.ChangeState(mario.runState);
		}

		// Jump
		if (Input.GetKeyDown(KeyCode.Space))
		{
			stateMachine.ChangeState(mario.jumpState);
		}

		//FIre
		if (Input.GetKeyDown(KeyCode.X) && PV.IsMine && (mario.marioMode == 2) && stateTimer2 <= 0f)
		{
			stateTimer2 = 0.5f; // fireTimer
			if (isFlip) {
                var a = PhotonNetwork.Instantiate("Prefabs/Fire_Bullet", mario.obj_bulletGeneratorA.position, Quaternion.identity);
                a.GetComponent<Fire_Bullet>().faceDir = -1;
			}
			else {
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
