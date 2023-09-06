using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mario_bigFire : Mario_state
{
    public Mario_bigFire(Mario _mario, Mario_stateMachine _stateMachine, string _animBoolName) : base(_mario, _stateMachine, _animBoolName)
    {
    }

    Vector3 pos;

    public override void Enter()
    {
        AudioManager.instance.PlayerOneShot(MARIO_SOUND.POWER_UP, false, 2);
        base.Enter(); 
        stateTimer = 50 * Time.deltaTime;
        mario.collider_big.enabled = false;
        mario.PV.RPC("Photon_RigidBody_Off", RpcTarget.AllBuffered, 0);
        pos = mario.transform.position;
    }

    public override void Exit()
    {
        
        base.Exit();
        mario.PV.RPC("Photon_RigidBody_On", RpcTarget.AllBuffered, 0);
        mario.collider_big.enabled = true;
    }

    public override void Update()
    {
        base.Update();
        mario.transform.position = pos;
        if (stateTimer <= 0) stateMachine.ChangeState(mario.idleState);
    }
}
