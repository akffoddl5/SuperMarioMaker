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
        //base.Enter();
        stateTimer = 7f * Time.deltaTime;
        mario.anim.SetBool("Idle", false);
        mario.anim.SetBool("Kicked", true);
        //mario.transform.localScale = new Vector3(1, 0.5f, 1);
    }

    public override void Exit()
    {
        //base.Exit();
        mario.anim.SetBool("Kicked", false);
        mario.anim.SetBool("Idle", true);
        //mario.transform.localScale = new Vector3(1, 1, 1);
    }

    public override void Update()
    {
        //base.Update();
        //if(stateTimer > 0)
        //Debug.Log(stateTimer);
        stateTimer -= Time.deltaTime;

        //if (stateTimer2 < 0)
        //{
        //    mario.transform.localScale = new Vector3(mario.transform.localScale.x, mario.transform.localScale.y - 0.1f, mario.transform.localScale.z);
        //    stateTimer2 = 0.2f;
        //}

        if (stateTimer < 0)
        {
            stateMachine.ChangeState(mario.idleState);
        }
    }
}
