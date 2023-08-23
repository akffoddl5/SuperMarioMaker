using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mario_kicked : Mario_state
{
    public Mario_kicked(Mario _mario, Mario_stateMachine _stateMachine, string _animBoolName) : base(_mario, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        stateTimer = 4f * Time.deltaTime;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        if (stateTimer < 0)
        {

        }
    }
}
