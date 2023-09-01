using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mario_BigSmall : Mario_state
{
    public Mario_BigSmall(Mario _mario, Mario_stateMachine _stateMachine, string _animBoolName) : base(_mario, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        stateTimer = 15 * Time.deltaTime;
        mario.collider.enabled = false;
    }

    public override void Exit()
    {
        base.Exit();
        mario.collider.enabled = true;
    }

    public override void Update()
    {
        base.Update();
        if (stateTimer <= 0) stateMachine.ChangeState(mario.idleState);
    }
}
