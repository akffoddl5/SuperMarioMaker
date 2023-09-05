using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mario_Shell_State : Mario_state
{
    public Mario_Shell_State(Mario _mario, Mario_stateMachine _stateMachine, string _animBoolName) : base(_mario, _stateMachine, _animBoolName)
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

        mario.pickedShell.transform.position = new Vector2(mario.check_body.position.x + 0.5f,
            mario.check_body.position.y + 0.45f);

        //던져버리기
        if (Input.GetKeyUp(KeyCode.C))
        {
            mario.pickedShell.GetComponent<Rigidbody2D>().AddForce(new Vector2(2, 0), ForceMode2D.Impulse);
            mario.pickedShell.GetComponent<Enemy_shell>().pickedState = false;

            mario.pickedShell = null;

            stateMachine.ChangeState(mario.idleState);
        }

    }
}
