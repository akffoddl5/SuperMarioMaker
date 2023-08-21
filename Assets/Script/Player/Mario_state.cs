using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public enum DIRECTION { LEFT = 0, RIGHT}

public class Mario_state
{
	//public DIRECTION dir = DIRECTION.LEFT;

	public Mario mario;
	public Mario_stateMachine stateMachine;
	public string animBoolName;

	public float stateTimer;
	public float xInput;
	public float yInput;
	public bool isFlip;

	public Mario_state(Mario _mario, Mario_stateMachine _stateMachine, string _animBoolName) 
	{
		mario = _mario;
		stateMachine = _stateMachine;
		animBoolName = _animBoolName;
	}

	public virtual void Enter()
	{
		mario.anim.SetBool(animBoolName, true);
	}

	public virtual void Exit()
	{
		mario.anim.SetBool(animBoolName, false);
	}
	public virtual void Update()
	{
		stateTimer -= Time.deltaTime;

		// �Է� �޴� ��
		xInput = Input.GetAxisRaw("Horizontal");
		yInput = Input.GetAxisRaw("Vertical");
		//if (xInput < 0) isFlip = DIRECTION.LEFT;

		// ���� ������ ���� �����ϱ� ��Ŭ���� �� �ø�
		if (xInput > 0)
		{
			// ��Ŭ�� �ߴµ� �ø��Ǿ������� �ø��ؾ���
			if (isFlip)
			{
				mario.spriteRenderer.flipX = false;
			}
			isFlip = false;
		}
		else if (xInput < 0)
		{ 
			// ��Ŭ���ߴµ� ������ ���� ������ �ø����ֱ�
			if (!isFlip)
			{
				mario.spriteRenderer.flipX = true;
			}
			isFlip = true; 
		}
		
	}
}
