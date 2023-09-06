using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using Photon.Pun;
using Photon.Realtime;

public class Enemy : MonoBehaviour
{

    [Header("Collision Info")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private Transform skyCheck;
    [SerializeField] private float skyCheckDistance;
    [SerializeField] private Transform playerLCheck;
    [SerializeField] private Transform playerRCheck;
    [SerializeField] private float playerCheckDistance;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private float wallCheckDistance;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private LayerMask whatIsPlayer;
    [SerializeField] private Transform skyCheckA;
    [SerializeField] private Transform skyCheckB;

    public Rigidbody2D rb;
    protected Animator anim;
    GameObject enemy;
    protected bool anim2=false;
    protected float moveflip = 1;

	public bool isDie = false;
	public bool isFilpOverDie = false;

	protected float overShellSpeed = 1;//1기본 10탈락 ->2이상이면 탈락처리
    protected bool overShell = false;//껍질 죽음

    //protected static float posX;
    //protected static float posY;
    public  float spdX;
    public  float spdY;

   protected bool move = false;

    public PhotonView PV;
    
    protected virtual void Start()
    {

        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        PV = GetComponent<PhotonView>();
       
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (!PhotonNetwork.IsMasterClient) return;
        if (isDie || isFilpOverDie) return;
		Move(move);
    }

    protected void Move(bool move)
    {
		if (move)
        {
            spdX = -1 *moveflip;
            spdY = rb.velocity.y;
            rb.velocity = new Vector2(spdX, spdY);

            spdX = rb.position.x;
            spdY = rb.position.y;

            Flip();
        }
        else if (!move)
        {
            return;
        }

    }

    protected void Flip()
    {
		if (moveflip > 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        if (IsGroundDetected() != true)
        {
            //transform.Rotate(0, 180, 0);
            moveflip = moveflip * -1;
        }

        if (IswallDetected() == true)
        {
            moveflip = moveflip * -1;
        }
    }


    //GroundCheck
    public bool IsGroundDetected() => Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);
    public bool IswallDetected() => Physics2D.Raycast(wallCheck.position, Vector2.left * moveflip, wallCheckDistance, whatIsGround);

    //플레이어 Up / Left / Right Detect
    //public bool IsSkyDetected() => Physics2D.Raycast(skyCheck.position, Vector2.up, skyCheckDistance, whatIsPlayer);
    public bool IsSkyDetected()
    {
        // if superMario, dont detect sky
        //if (isFilpOverDie) return false;
        Collider2D[] colliders = Physics2D.OverlapAreaAll(skyCheckA.position, skyCheckB.position);
        for (int i = 0; i < colliders.Length; i++){
            if (colliders[i] != this.gameObject && colliders[i].CompareTag("Player"))
            {
                if(!colliders[i].gameObject.GetComponent<Mario>().isStarMario)
                return true;
            }
        }
        return false;
    }
    public bool IsPlayerLDetected() => Physics2D.Raycast(playerLCheck.position, Vector2.left, playerCheckDistance, whatIsPlayer);
    public bool IsPlayerRDetected() => Physics2D.Raycast(playerRCheck.position, Vector2.right, playerCheckDistance, whatIsPlayer);

	public virtual void Die()
    {
		
	}

    public virtual void FilpOverDie()
    {
        Debug.Log("FilpOverDie()FilpOverDie()FilpOverDie()FilpOverDie()FilpOverDie()FilpOverDie()FilpOverDie()");
		var col = gameObject.GetComponent<Collider2D>();
		col.enabled = false;
		isDie = true;
        isFilpOverDie = true;
		gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 4f), ForceMode2D.Impulse);
		gameObject.transform.Rotate(180, 0, 0);

        
        Destroy(gameObject, 1f);
	}

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position, new Vector3(groundCheck.position.x,
            groundCheck.position.y - groundCheckDistance));

        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x - wallCheckDistance * moveflip,
            wallCheck.position.y));
        //플레이어 위 감지
        Gizmos.DrawLine(skyCheck.position, new Vector3(skyCheck.position.x,
            skyCheck.position.y + skyCheckDistance));

        //플레이어 좌우 감지

        Gizmos.DrawLine(playerLCheck.position, new Vector3(playerLCheck.position.x - playerCheckDistance,
            playerLCheck.position.y));

        Gizmos.DrawLine(playerRCheck.position, new Vector3(playerRCheck.position.x + playerCheckDistance,
            playerRCheck.position.y));

    }
}
