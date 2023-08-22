using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Mario : MonoBehaviour
{

	
    [Header("Move Info")]
    public float moveSpeed = 5;
	public float runSpeed = 6;
	public float jumpPower;
	public float lastXSpeed;

	[Header("Ground Check")]
	public Transform obj_isGround;
	public Transform obj_isPlayerA;
	public Transform obj_isPlayerB;
	public float groundCheckDist;
	public float playerCheckDist;
	public LayerMask whatIsGround;
	public LayerMask whatIsPlayer;


	[HideInInspector] public Rigidbody2D rb;
	[HideInInspector] public Animator anim;
	[HideInInspector] public SpriteRenderer spriteRenderer;

	Mario_stateMachine stateMachine;
 
    public Mario_idle idleState;
    public Mario_run runState;
	public Mario_jump jumpState;
	public Mario_slide slideState;
	public Mario_walk walkState;


	private void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
		anim = GetComponent<Animator>();
		spriteRenderer = GetComponent<SpriteRenderer>();

        stateMachine = new Mario_stateMachine();

        idleState = new Mario_idle(this, stateMachine, "Idle");
		walkState = new Mario_walk(this, stateMachine, "Walk");
		runState = new Mario_run(this, stateMachine, "Run");
		jumpState = new Mario_jump(this, stateMachine, "Jump");
		slideState = new Mario_slide(this, stateMachine, "Slide");
	}
	[PunRPC]
	public void Flip(bool a)
	{
		spriteRenderer.flipX = a;
	}
	private void Start()
	{
		if(!GetComponent<PhotonView>().IsMine) return ;
        stateMachine.InitState(idleState);
	}
	// Update is called once per frame
	void Update()
    {
		if (!GetComponent<PhotonView>().IsMine) return;
		//Debug.Log(GetComponent<PhotonView>().IsMine);
		stateMachine.currentState.Update();
	}

	public bool IsGroundDetected() => Physics2D.Raycast(obj_isGround.position, Vector2.down, groundCheckDist, whatIsGround);
	public bool IsPlayerDetected()
	{
		//for (int i = 0; i < obj_isPlayer.Length; i++)
		//{
		//	if (Physics2D.Raycast(obj_isPlayer[i].position, Vector2.down, playerCheckDist, whatIsPlayer))
		//	{
		//		return true;
		//	}
		//}
		Collider2D[] cols = Physics2D.OverlapAreaAll(obj_isPlayerA.position, obj_isPlayerB.position);

		for (int i = 0; i < cols.Length; i++)
		{
			if (cols[i].gameObject != this.gameObject && cols[i].gameObject.CompareTag("Player"))
			{
				Debug.Log(cols[i].gameObject.name);
				
				return true;
			}
		}

		//Debug.Log("player detected");
		return false;
		
	}


	protected virtual void OnDrawGizmos()
	{
		Gizmos.DrawLine(obj_isGround.position,
			new Vector3(obj_isGround.position.x, obj_isGround.position.y - groundCheckDist));
	}
}
