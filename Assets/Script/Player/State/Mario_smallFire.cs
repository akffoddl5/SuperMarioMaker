using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mario_smallFire : Mario_state
{
    public Mario_smallFire(Mario _mario, Mario_stateMachine _stateMachine, string _animBoolName) : base(_mario, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        stateTimer = 50 * Time.deltaTime;
        mario.collider.enabled = false;
        mario.transform.position += new Vector3(0, 0.5f, 0);
        mario.check_body.localScale = new Vector3(1.4f, 2.1f, 1);
        mario.GetComponent<Rigidbody2D>().Sleep();
    }

    public override void Exit()
    {
        base.Exit();
        mario.collider_big.enabled = true;
        mario.GetComponent<Rigidbody2D>().WakeUp();
    }

    public override void Update()
    {
        base.Update();
        mario.collider_big.enabled = true;
        mario.GetComponent<Rigidbody2D>().WakeUp();
    }
}
