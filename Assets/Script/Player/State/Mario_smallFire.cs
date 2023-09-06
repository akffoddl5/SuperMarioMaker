using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mario_smallFire : Mario_state
{
    public Mario_smallFire(Mario _mario, Mario_stateMachine _stateMachine, string _animBoolName) : base(_mario, _stateMachine, _animBoolName)
    {
    }

    Vector3 pos;

    public override void Enter()
    {
        base.Enter();
        stateTimer = 50 * Time.deltaTime;
        mario.collider.enabled = false;
        mario.transform.position += new Vector3(0, 0.5f, 0);
        mario.check_body.localScale = new Vector3(1.4f, 2.1f, 1);
        pos = mario.transform.position;
        
        //mario.GetComponent<Rigidbody2D>().Sleep();
        Debug.Log(PhotonNetwork.NickName + "small fire enter");
    }

    public override void Exit()
    {
        base.Exit();
        Debug.Log(PhotonNetwork.NickName + "small fire exit");
        mario.collider_big.enabled = true;
        mario.PV.RPC("SetCollider", RpcTarget.AllBuffered, 1);
        //mario.GetComponent<Rigidbody2D>().WakeUp();
    }

    public override void Update()
    {
        base.Update();
        mario.transform.position = pos;
        if (stateTimer <= 0) stateMachine.ChangeState(mario.idleState);
    }
}
