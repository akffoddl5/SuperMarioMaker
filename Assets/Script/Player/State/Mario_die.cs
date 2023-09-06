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
        AudioManager.instance.PlayerOneShot(MARIO_SOUND.MARIO_DIE, false, 2);
        base.Enter();
		mario.rb.AddForce(new Vector2(0, dieJumpPower), ForceMode2D.Impulse);

		// collider 끄기 => 리스폰 돼야 하니까 리스폰 되면 다시 켜줘야 함
		mario.GetComponent<CapsuleCollider2D>().enabled = false;
	}

	public override void Exit()
	{
		base.Exit();
	}

	public override void Update()
	{
		base.Update();

	}

}
