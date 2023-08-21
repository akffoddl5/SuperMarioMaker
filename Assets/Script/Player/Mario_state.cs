using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mario_state
{
	public Mario mario;
	public Mario_stateMachine stateMachine;
	public string animBoolName;


	public Rigidbody2D rb;
	public Animator anim;

	public float stateTimer;
	public float xInput;
	public float yInput;

	public Mario_state(Mario _mario, Mario_stateMachine _stateMachine, string _animBoolName) 
	{
		mario = _mario;
		stateMachine = _stateMachine;
		animBoolName = _animBoolName;
	}

	public virtual void Enter()
	{
		anim.SetBool(animBoolName, true);
	}

	public virtual void Exit()
	{
		anim.SetBool(animBoolName, false);
	}
	public virtual void Update()
	{
		xInput = Input.GetAxisRaw("Horizontal");
		yInput = Input.GetAxisRaw("Vertical");

		stateTimer -= Time.deltaTime;
	}
}
