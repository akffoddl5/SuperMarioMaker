using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mario_SmallBig : Mario_state
{
    public Mario_SmallBig(Mario _mario, Mario_stateMachine _stateMachine, string _animBoolName) : base(_mario, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        stateTimer = 50 * Time.deltaTime;
        mario.collider.enabled = false;
        mario.GetComponent<Rigidbody2D>().Sleep();
        
    }

    public override void Exit()
    {
        base.Exit();
        mario.collider.enabled = true;
        mario.GetComponent<Rigidbody2D>().WakeUp();
    }

    public override void Update()
    {
        base.Update();
        if (stateTimer <= 0) stateMachine.ChangeState(mario.idleState);
    }
}