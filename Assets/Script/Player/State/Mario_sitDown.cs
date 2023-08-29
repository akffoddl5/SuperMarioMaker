using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Mario_sitDown : Mario_state
{
	public Mario_sitDown(Mario _mario, Mario_stateMachine _stateMachine, string _animBoolName) : base(_mario, _stateMachine, _animBoolName)
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
		if (Input.GetKeyUp(KeyCode.DownArrow))
		{
			stateMachine.ChangeState(mario.idleState);
		}

        //FIre
        if (Input.GetKeyDown(KeyCode.X) && mario.marioMode == 2)
        {
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
    }
}
