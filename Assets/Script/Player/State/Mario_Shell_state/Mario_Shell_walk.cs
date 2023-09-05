using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mario_Shell_walk : Mario_Shell_State
{
	public Mario_Shell_walk(Mario _mario, Mario_stateMachine _stateMachine, string _animBoolName) : base(_mario, _stateMachine, _animBoolName)
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


		mario.rb.velocity = new Vector2(xInput * mario.moveSpeed, mario.rb.velocity.y);

		//Idle
		if (xInput == 0)
		{
			stateMachine.ChangeState(mario.mario_Shell_Idle);
		}

		// Run
		if(Input.GetKey(KeyCode.Z))
		{
			stateMachine.ChangeState(mario.mario_Shell_Run);
		}

		// Jump
		if (Input.GetKeyDown(KeyCode.Space))
		{
			stateMachine.ChangeState(mario.mario_Shell_Jump);
		}

		////FIre
  //      if (Input.GetKeyDown(KeyCode.X) && mario.marioMode == 2)
  //      {
            
		//	if (isFlip) {
  //              var a = PhotonNetwork.Instantiate("Prefabs/Fire_Bullet", mario.obj_bulletGeneratorA.position, Quaternion.identity);
  //              a.GetComponent<Fire_Bullet>().faceDir = -1;
		//	}
		//	else {
  //              var a = PhotonNetwork.Instantiate("Prefabs/Fire_Bullet", mario.obj_bulletGeneratorB.position, Quaternion.identity);
  //              a.GetComponent<Fire_Bullet>().faceDir = 1;
  //          } 


		//}

        if (mario.rb.velocity.y <= 0 && mario.IsPlayerDetected())
		{
			stateMachine.ChangeState(mario.mario_Shell_Stamp);
		}

	}
}
