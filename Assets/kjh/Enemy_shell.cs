using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Enemy_shell : MonoBehaviour
{
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private Transform skyCheck;
    [SerializeField] private float skyCheckDistance;
    [SerializeField] private Transform playerLCheck;
    [SerializeField] private Transform playerRCheck;
    [SerializeField] private float playerCheckDistance;
    [SerializeField] private Transform wallLCheck;
    [SerializeField] private Transform wallRCheck;
    [SerializeField] private float wallCheckDistance;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private LayerMask whatIsPlayer;

    public float spdX;
    public float spdY;

    public float moveflip = 1;
    // float flyTimer = 0;

    bool fly = false;
    public Rigidbody2D rb;
    public GameObject koopa;
    public bool fsecMove = false;

    public bool pickedState = false;
    bool pastPickedState = false;

    float timer;

    public PhotonView PV;

    public GameObject pickedPlayer = null;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(0, rb.velocity.y);
        timer = 60 * Time.deltaTime;
        PV = GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
    }

    private void FixedUpdate()
    {
        if (pickedState)
        {
            pastPickedState = true;
            return;
        }
        else
        {
            if (pastPickedState)
            {
                fsecMove = true;
                pastPickedState = false;
            }
        }
        

        if (timer > 0) return;
        if (fsecMove == true) Move(fsecMove);

        // wall flip
        if (IsWallRDetected() || IsWallLDetected())
        {
            Flip2();
            //Move(true);
        }

        if (!IsGroundDetected())
        {
            Invoke("Fly", 0.1f);
        }
        //spdX = rb.position.x;
        //spdY = rb.position.y;

        //if (IsGroundDetected() && !fly && !IsSkyDetected())// 돌아가기
        //{
        //    StartCoroutine("Rekoopa");
        //}
        //Debug.Log(fly);
        //if (fly == true)// 수직낙하 거북스
        //{
        //   /rb.velocity = new Vector2(0, rb.velocity.y);
        //}
        //else if (fly == false)
        //{
        //    rb.gravityScale = 1;
        //}
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //플레이어에게 잡혀있다가 던져지면 콜라이더의 isTrigger를 false로 바꾼다
        if (collision.gameObject == pickedPlayer)
        {
            GetComponent<Collider2D>().isTrigger = false;
            rb.gravityScale = 1;
            pickedPlayer = null;
        }

    }

    void Move(bool move)
    {
        //object[] tmp = new object[] { moveflip, move, rb.velocity.y };
        //PV.RPC("RPC_Move", RpcTarget.AllBuffered, tmp);
        if (move)
        {
            // right
            if (rb.velocity.x > 0) moveflip = 1;
            // left
            else if (rb.velocity.x < 0) moveflip = -1;

            spdX = 8 * moveflip;
            spdY = rb.velocity.y;

            rb.velocity = new Vector2(spdX, spdY);
        }
        else return;
    }

    [PunRPC]
    public void RPC_Move(object[] tmp)
    {
        moveflip = (float)tmp[0];
        bool move = (bool)tmp[1];

        if (move)
        {
            spdX = -8 * moveflip;
            spdY = rb.velocity.y;

            rb.velocity = new Vector2(spdX, spdY);
        }
        else
        {
            return;
        }
    }

    //private void secMove()
    //{
    //    if (IsGroundDetected())
    //    {
    //        Move(true);

    //        if (!fly && !IsSkyDetected())// 돌아가기
    //        {
    //            StartCoroutine("Rekoopa");
    //        }
    //    }
    //}

    //private void playerRL()
    //{
    //    if (IsPlayerLDetected())//왼플체크
    //    {
    //        Flip2();
    //        Move(true);
    //        fsecMove = true;
    //    }
    //    else if (IsPlayerRDetected())//오플체크
    //    {
    //        Move(true);
    //        fsecMove = true;
    //    }

    //}

    private void Fly()//0.1초 동안 하늘일떄
    {
        if (!IsGroundDetected())
        {
            fly = true;
        }
        else if (IsGroundDetected())
        {
            fly = false;
        }
    }

    public virtual void FilpOverDie()
    {
        Debug.Log("FilpOverDie()FilpOverDie()FilpOverDie()FilpOverDie()FilpOverDie()FilpOverDie()FilpOverDie()");
        gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 4f), ForceMode2D.Impulse);
        gameObject.transform.Rotate(180, 0, 0);
        var col = gameObject.GetComponent<Collider2D>();
        col.enabled = false;

        Destroy(gameObject, 1f);
    }

    void Flip2()//스프라이트 회전
    {
        transform.Rotate(0, 180, 0);
        moveflip = -moveflip;
    }

    IEnumerator Rekoopa()
    {
        yield return new WaitForSeconds(10f);
        Destroy(gameObject);
        Instantiate(koopa, new Vector2(transform.position.x, transform.position.y), Quaternion.identity);
    }

    //그라운드 체크
    public bool IsGroundDetected() => Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);
    public bool IsWallLDetected() => Physics2D.Raycast(wallLCheck.position, Vector2.left * moveflip, wallCheckDistance, whatIsGround);
    public bool IsWallRDetected() => Physics2D.Raycast(wallRCheck.position, Vector2.right * moveflip, wallCheckDistance, whatIsGround);


    //플레이어 위감지
    public GameObject IsSkyDetected()
    {

        Collider2D[] cols = Physics2D.OverlapAreaAll(new Vector2(skyCheck.position.x - skyCheckDistance, skyCheck.position.y), new Vector2(skyCheck.position.x + skyCheckDistance, skyCheck.position.y));

        for (int i = 0; i < cols.Length; i++)
        {
            if (cols[i].gameObject != this.gameObject && cols[i].gameObject.CompareTag("Player"))
            {
                if (!cols[i].gameObject.GetComponent<Mario>().isStarMario)
                    return cols[i].gameObject;
            }
        }

        //Debug.Log("player detected");
        return null;

    }

    //플레이어 좌우 감지
    public bool IsPlayerLDetected() => Physics2D.Raycast(playerLCheck.position, Vector2.left, playerCheckDistance, whatIsPlayer);
    public bool IsPlayerRDetected() => Physics2D.Raycast(playerRCheck.position, Vector2.right, playerCheckDistance, whatIsPlayer);

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position, new Vector3(groundCheck.position.x,
            groundCheck.position.y - groundCheckDistance));

        Gizmos.DrawLine(wallLCheck.position, new Vector3(wallLCheck.position.x - wallCheckDistance,
            wallLCheck.position.y));
        Gizmos.DrawLine(wallLCheck.position, new Vector3(wallRCheck.position.x + wallCheckDistance,
            wallRCheck.position.y));

        //플레이어 좌우 감지

        Gizmos.DrawLine(playerLCheck.position, new Vector3(playerLCheck.position.x - playerCheckDistance,
            playerLCheck.position.y));

        Gizmos.DrawLine(playerRCheck.position, new Vector3(playerRCheck.position.x + playerCheckDistance,
            playerRCheck.position.y));

    }
}

