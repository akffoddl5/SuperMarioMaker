using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

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



    public Rigidbody2D rb;
    protected Animator anim;
    protected float moveflip = 1;

    protected float overShellSpeed = 1;//1기본 10탈락 ->2이상이면 탈락처리
    protected bool overShell = false;//껍질 죽음
    //protected static float posX;
    //protected static float posY;
    public static float spdX;
    public static float spdY;

   protected bool move = false;



    protected virtual void Start()
    {

        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    protected virtual void Update()
    {



        Move(move);
        Flat();




      // posX = this.transform.position.x;
      // posY = this.transform.position.y;
       
    }



    protected void Flat()
    {


        if (IsSkyDetected())
        {
            anim.SetBool("Flat", true);

       }
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
        

    }

    private void Flip()
    {

        if (IsGroundDetected() != true)
        {

            transform.Rotate(0, 180, 0);


            moveflip = moveflip * -1;

        }

        if (IswallDetected() == true)
        {

            transform.Rotate(0, 180, 0);


            moveflip = moveflip * -1;

        }
    }
    //그라운드 체크
    public bool IsGroundDetected() => Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);
    public bool IswallDetected() => Physics2D.Raycast(wallCheck.position, Vector2.left * moveflip, wallCheckDistance, whatIsGround);

   



    //플레이어 위감지
    public bool IsSkyDetected() => Physics2D.Raycast(skyCheck.position, Vector2.up, skyCheckDistance, whatIsPlayer);
    //플레이어 좌우 감지
    public bool IsPlayerLDetected() => Physics2D.Raycast(playerLCheck.position, Vector2.left, playerCheckDistance, whatIsPlayer);
    public bool IsPlayerRDetected() => Physics2D.Raycast(playerRCheck.position, Vector2.right, playerCheckDistance, whatIsPlayer);




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
