using Cinemachine;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Stone : MonoBehaviour
{
    public float speed = 10f;
    Vector2 speedV;
    Vector3 vel = Vector3.zero;
    Sprite spr;
    float ground;
    Vector2 pos;
    float timer = 2f;
    bool move = false;
    Animator anim;
    Rigidbody2D rbody;
    [Header("Collision Info")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private float groundCheckDistance_short;
    // [SerializeField] private Transform playerCheck;
    // [SerializeField] private Transform playerCheck2;
    // [SerializeField] private float playerCheckDistance;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private LayerMask whatIsPlayer;

    public CinemachineVirtualCamera cam;


    GameObject groundObject;
    Coroutine dropCoroutine;          
    // Start is called before the first frame update
    void Start()
    {
        float ground = whatIsGround.value;
        pos = transform.position;
        anim = GetComponent<Animator>();
        rbody = GetComponent<Rigidbody2D>();
        speedV = new Vector2(0, 10000);
        spr= GetComponent<Sprite>();
        
    }
    // Update is called once per frame
    void Update()
    {
        //�浹�ϰ� �������� �̰� �۵�
        // transform.position = Vector3.MoveTowards(pos, new Vector3(transform.position.x, 1000, 0), Time.deltaTime * speed);


        // cols = �� �ڽ��ȿ� �浹�� ��� �ö��̴� ���� ������ 

        var cols = Physics2D.OverlapAreaAll(new Vector2(transform.position.x - 3.5f, transform.position.y), new Vector2(transform.position.x + 3.5f, IsGroundDetected().collider.transform.position.y), LayerMask.GetMask("Player"));

        // �浹�� ������, �÷��̾ �װ����ȿ� ������ �ð��� �ٿ��� 


        if(cols.Length > 0)
        {
            
        
            if (dropCoroutine == null)
            { move = true;
                anim.SetBool("Move", move);
              
                dropCoroutine = StartCoroutine(Drop());

                
            }
        }

          

        //if (cols.Length == 0)
        //{
        //    anim.SetBool("Move", false);
        //    dropCoroutine = null;
        //}
        timer -= Time.deltaTime;

        //�����ϰ� �ð����� ó�������� �帤���ϱ�


        //Debug.Log("cols.Length" + cols.Length);
        //Debug.Log("timer" + timer);
        //Debug.Log("�÷��̾� ����" + cols.Length);


        //if (timer < 0)
        //{

        //    rbody.gravityScale = 3;



        //    //new Vector2(transform.position.x, IsGroundDetected().collider.gameObject.transform.position.y)
        //    timer = 3f;

        //}






        //if (cols.Length == 0)
        //{
        //    timer = 3;
        //    rbody.gravityScale = 0;
        //    if (IsGroundDetected())
        //        transform.position = Vector2.SmoothDamp(new Vector2(transform.position.x, IsGroundDetected().collider.gameObject.transform.position.y), pos, ref speedV, 10f, 50);
        //}



        //if(IsPlayer2Check().collider.transform.position.y> IsGroundDetected().collider.transform.position.y)
        //  { move = true;
        //      timer = 2;
        //      anim.SetBool("Move", move);
        //      start();
        //  }


        //  if (IsPlayer2Check().collider.transform.position.y <=IsGroundDetected().collider.transform.position.y)
        //  {

        //  }




        //{
        //    anim.SetBool("Move", true);
        //    Invoke("start", 1);
        //}


    }

        IEnumerator Drop()
    {
        // Ÿ�̸�[2��] ��ٸ�
        yield return new WaitForSeconds(2.5f);
        move = false;
        anim.SetBool("Move", move);

        var groundDist = 100f;
        // �ݺ���
       
         while(!IsGroundDetected_Short())
         { // ��ӳ����� [���� ����������]
            transform.Translate(Vector2.down * speed * Time.deltaTime);

            
            groundDist = Vector2.Distance(transform.position, groundObject.transform.position);
            // Debug.Log(" �Ÿ� ::" +  groundDist);
            yield return new WaitForEndOfFrame();
        }
        AudioManager.instance.PlayerOneShot(MARIO_SOUND.STONE , false, 2);

         // ������� ���Ŀ� 1�� ��ٸ�
        yield return new WaitForSeconds(1f);

        // distance �Ÿ� 
        float dist = 100f;
        while( dist >= 0.1f)
        {
            dist = Vector2.Distance(transform.position, pos);

            transform.Translate(Vector2.up * 1.5f * Time.deltaTime);
// gameObject.GetComponent<SpriteRenderer>().sprite = spr;
            yield return new WaitForEndOfFrame();
        }
       
        yield return null;

        dropCoroutine = null;

    }


    private void start()
    {


        //if (timer<=0)
        //{

        //    move = false;
        //    anim.SetBool("Move", move);
        //    Debug.Log(1);
        //}
        //   Debug.Log(1);
        //transform.position = Vector3.MoveTowards(pos, new Vector2(transform.position.x, IsGroundDetected().collider.gameObject.transform.position.y ), Time.deltaTime * speed);
        // }
    }

    private void back()
    {
        //  transform.position = Vector3.MoveTowards(new Vector2(transform.position.x, IsGroundDetected().collider.gameObject.transform.position.y ), pos, Time.deltaTime * speed);
    }

    //public RaycastHit2D IsPlayerCheck() => Physics2D.Raycast(playerCheck.position, Vector2.down, playerCheckDistance, whatIsPlayer);
    //public RaycastHit2D IsPlayer2Check() => Physics2D.Raycast(playerCheck2.position, Vector2.down, playerCheckDistance, whatIsPlayer);
    // public bool IsGroundDetected() => Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);
    public RaycastHit2D IsGroundDetected()
    {
        RaycastHit2D hit = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);

        if (hit)
        {
            groundObject = hit.collider.gameObject;
           
        }

            return hit;
    }

    public bool IsGroundDetected_Short()
    {
        RaycastHit2D hit = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance_short, whatIsGround);

        if (hit)
        {
            return true;

        }

        return false;
    }


    private void OnDrawGizmos()
    { //
      // Gizmos.DrawLine(playerCheck.position, new Vector3(playerCheck.position.x,
      //     playerCheck.position.y - playerCheckDistance));
      // Gizmos.DrawLine(playerCheck2.position, new Vector3(playerCheck2.position.x,
      //     playerCheck2.position.y - playerCheckDistance));

        //Gizmos.DrawLine(groundCheck.position, new Vector3(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));

        Gizmos.DrawLine(groundCheck.position, new Vector3(groundCheck.position.x,
          groundCheck.position.y - groundCheckDistance_short));

        //Gizmos.DrawCube((new Vector2(transform.position.x, transform.position.y - (transform.position.y - IsGroundDetected().collider.transform.position.y) / 2)), new Vector2(7, (transform.position.y - IsGroundDetected().collider.transform.position.y)));
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (!collision.gameObject.GetComponent<PhotonView>().IsMine) return;
            var mario = collision.gameObject.GetComponent<Mario>();
            // star Mode��� ���� �ʵ���
            if (mario.isStarMario) return;


            //mario.stateMachine.ChangeState(mario.dieState);

            if (mario.marioMode > 0)
            {
                mario.stateMachine.ChangeState(mario.bigSmall); // �����̴� �ź��� ������� ������ ����
            }
            else
            {
                mario.stateMachine.ChangeState(mario.dieState); // �����̴� �ź��� ������� ������ ����
            }
        }
    }




}
