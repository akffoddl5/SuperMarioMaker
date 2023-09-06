using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mario_die : Mario_state
{
    private float dieJumpPower = 10;
    public Mario_die(Mario _mario, Mario_stateMachine _stateMachine, string _animBoolName) : base(_mario, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        if (AudioManager.instance != null)
            AudioManager.instance.PlayerOneShot(MARIO_SOUND.MARIO_DIE, false, 2);
        base.Enter();
        mario.rb.AddForce(new Vector2(0, dieJumpPower), ForceMode2D.Impulse);
        Debug.Log(PV.name + " DieState AddForce ��");

        // collider ���� => ������ �ž� �ϴϱ� ������ �Ǹ� �ٽ� ����� ��
        mario.GetComponent<CapsuleCollider2D>().enabled = false;
        Debug.Log(PV.name + " DieState Collider2D ��");
    }

    public override void Exit()
    {
        base.Exit();
        Debug.Log(PV.name + " DieState ����");
    }

    public override void Update()
    {
        base.Update();

    }

    //void MarioJumpDie()
    //{

    //}

}
