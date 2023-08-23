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
        Debug.Log("kicked ¡¯¿‘" + mario.gameObject.name );
        stateTimer = 50f * Time.deltaTime;
        mario.transform.localScale = new Vector3(1, 0.5f, 1);
    }

    public override void Exit()
    {
        //base.Exit();
        Debug.Log("kicked exit" + mario.gameObject.name);
        mario.transform.localScale = new Vector3(1, 1, 1);
    }

    public override void Update()
    {
        //base.Update();
        Debug.Log(stateTimer);

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
