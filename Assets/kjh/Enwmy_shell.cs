using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;

public class Enwmy_shell : MonoBehaviour
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



    static float moveflip = 1;
    // float flyTimer = 0;

    bool fly = false;
    public Rigidbody2D rb;
    public GameObject koopa;
    bool fsecMove = false;



    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

    }

    // Update is called once per frame
    void Update()
    {
        

    }
    private void FixedUpdate()
    {





        playerRL();
        if (fsecMove == true)
        {

            secMove();
        }
        if (IswallRDetected() || IswallLDetected())
        {



            Flip2();
            Move(true);

        }







        if (!IsGroundDetected())
        {
            Invoke("Fly", 0.1f);
        }
        Enemy.spdX = rb.position.x;
        Enemy.spdY = rb.position.y;


        if (IsGroundDetected() && !fly && !IsSkyDetected())// ���ư���
        {


            StartCoroutine("Rekoopa");


        }

        if (fly == true)
        {
            rb.gravityScale = 100;
        }
        else if (fly == false)
        {
            rb.gravityScale = 1;
        }
    }
    private void secMove()
    {
        if (IsGroundDetected())
        {


            Move(true);





            if (!fly && !IsSkyDetected())// ���ư���
            {


                StartCoroutine("Rekoopa");


            }
        }


    }

    private void playerRL()
    {
        if (IsPlayerLDetected())//����üũ
        {


            Flip2();
            Move(true);


            fsecMove = true;


        }
        else if (IsPlayerRDetected())//����üũ
        {

            Move(true);

            fsecMove = true;


        }

    }

    private void Fly()//0.1�� ���� �ϴ��ϋ�
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



     void Flip2()//��������Ʈ ȸ��
    {

        transform.Rotate(0, 180, 0);


        moveflip = -moveflip;
    }



    void Move(bool move)//������
    {

        if (move)
        {



            Enemy.spdX = -8 * moveflip;
            Enemy.spdY = rb.velocity.y;

            rb.velocity = new Vector2(Enemy.spdX, Enemy.spdY);


        }
        else
        {
            return;
        }


    }






    IEnumerator Rekoopa()
    {
        yield return new WaitForSeconds(10f);


        Destroy(gameObject);
        Instantiate(koopa, new Vector2(Enemy.spdX, Enemy.spdY), Quaternion.identity);
    }







    //�׶��� üũ
    public bool IsGroundDetected() => Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);
    public bool IswallLDetected() => Physics2D.Raycast(wallLCheck.position, Vector2.left * moveflip, wallCheckDistance, whatIsGround);
    public bool IswallRDetected() => Physics2D.Raycast(wallRCheck.position, Vector2.right * moveflip, wallCheckDistance, whatIsGround);





    //�÷��̾� ������
    public bool IsSkyDetected() => Physics2D.Raycast(skyCheck.position, Vector2.up, skyCheckDistance, whatIsPlayer);
    //�÷��̾� �¿� ����
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
        //�÷��̾� �� ����
        Gizmos.DrawLine(skyCheck.position, new Vector3(skyCheck.position.x,
            skyCheck.position.y + skyCheckDistance));

        //�÷��̾� �¿� ����

        Gizmos.DrawLine(playerLCheck.position, new Vector3(playerLCheck.position.x - playerCheckDistance,
            playerLCheck.position.y));

        Gizmos.DrawLine(playerRCheck.position, new Vector3(playerRCheck.position.x + playerCheckDistance,
            playerRCheck.position.y));

    }
}

