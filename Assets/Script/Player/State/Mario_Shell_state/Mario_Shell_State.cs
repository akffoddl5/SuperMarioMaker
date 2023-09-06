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

        //플레이어 방향에 따라 등딱지 위치 변경
        float shellX = 0.4f;
        float shellY = 0.45f;
        if (mario.marioMode != 0)
        {
            shellY = 0.3f;
        }
        if (mario.spriteRenderer.flipX)
            mario.pickedShell.transform.position = new Vector2(mario.check_body.position.x - shellX,
                        mario.check_body.position.y + shellY);
        else
            mario.pickedShell.transform.position = new Vector2(mario.check_body.position.x + shellX,
                mario.check_body.position.y + shellY);

        //던져버리기
        if (Input.GetKeyUp(KeyCode.C))
        {
            if (mario.spriteRenderer.flipX)
                mario.pickedShell.GetComponent<Rigidbody2D>().AddForce(new Vector2(-2, 0), ForceMode2D.Impulse);
            else
                mario.pickedShell.GetComponent<Rigidbody2D>().AddForce(new Vector2(2, 0), ForceMode2D.Impulse);

            mario.pickedShell.GetComponent<Enemy_shell>().pickedState = false;

            mario.pickedShell = null;

            stateMachine.ChangeState(mario.idleState);
        }

    }
}
