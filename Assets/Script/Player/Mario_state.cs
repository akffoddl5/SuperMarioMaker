using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

//public enum DIRECTION { LEFT = 0, RIGHT}

public class Mario_state
{
	//public DIRECTION dir = DIRECTION.LEFT;

	public Mario mario;
	public Mario_stateMachine stateMachine;
	public string animBoolName;

	public float stateTimer;
	public float stateTimer2;

	public float xInput;
	public float yInput;
	public bool isFlip;

	public PhotonView PV;

    public Mario_state(Mario _mario, Mario_stateMachine _stateMachine, string _animBoolName) 
	{
		mario = _mario;
		stateMachine = _stateMachine;
		animBoolName = _animBoolName;
		PV = mario.GetComponent<PhotonView>();
	}

	public virtual void Enter()
	{
		//Debug.Log(mario.GetComponent<PhotonView>().IsMine + " << ENter");
		if (!mario.GetComponent<PhotonView>().IsMine)
		{
			return;
		}
		mario.anim.SetBool(animBoolName, true);
		mario.anim.SetInteger("Mario_Mode", mario.marioMode);
	}

	public virtual void Exit()
	{
		if (!mario.GetComponent<PhotonView>().IsMine)
		{
			return;
		}
		//Debug.Log("animBoolName Exit()" + animBoolName);
		mario.anim.SetBool(animBoolName, false);
	}
	public virtual void Update()
	{
		if (!mario.GetComponent<PhotonView>().IsMine)
		{
			return;
		}
		stateTimer -= Time.deltaTime;
		stateTimer2 -= Time.deltaTime;

		if (mario.IsWallDetected())
		{
			mario.PM.friction = 0;
			mario.collider.sharedMaterial = mario.PM;
			mario.collider_big.sharedMaterial = mario.PM;
		}
		else
		{
			mario.PM.friction = 0.4f;
			mario.collider.sharedMaterial = mario.PM;
			mario.collider_big.sharedMaterial = mario.PM;
		}

		// 입력 받는 곳
		xInput = Input.GetAxisRaw("Horizontal");
		yInput = Input.GetAxisRaw("Vertical");
		//if (xInput < 0) isFlip = DIRECTION.LEFT;

		// 원래 오른쪽 보고 있으니까 우클릭은 안 플립
		FlipCheck();
			//PV.RPC("Flip", RpcTarget.All, FlipCheck() == 1);

	}

	private void FlipCheck()
	{
		if (xInput > 0)
		{
			// 우클릭 했는데 플립되어있으면 플립해야함
			if (isFlip)
			{
				isFlip = false;
				PV.RPC("Flip", RpcTarget.All, false);
            }
		}
		else if (xInput < 0)
		{
			// 좌클릭했는데 오른쪽 보고 있으면 플립해주기
			if (!isFlip)
			{
				isFlip = true;
				PV.RPC("Flip", RpcTarget.All, true);
            }
		}
	}
}
