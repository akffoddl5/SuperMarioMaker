using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mario_BigSmall : Mario_state
{
    public Mario_BigSmall(Mario _mario, Mario_stateMachine _stateMachine, string _animBoolName) : base(_mario, _stateMachine, _animBoolName)
    {
    }

    Vector3 pos;

    public override void Enter()
    {
        Debug.Log("big small");
        base.Enter();
        stateTimer = 60 * Time.deltaTime;
        mario.marioMode = 0;
        pos = mario.gameObject.transform.position;
        mario.collider.enabled = false;
        mario.collider_big.enabled = false;
    }

    public override void Exit()
    {
        base.Exit();
        
        mario.collider.enabled = true;
    }

    public override void Update()
    {
        base.Update();
        mario.transform.position = pos;
        if (stateTimer <= 0) stateMachine.ChangeState(mario.idleState);
    }
}
