using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Stone : MonoBehaviour
{
    public float speed = 10;
    float ground;
    Vector2 pos;
    float timer;
    Animator anim;
    [Header("Collision Info")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private Transform playerCheck;
    [SerializeField] private Transform playerCheck2;
    [SerializeField] private float playerCheckDistance;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private LayerMask whatIsPlayer;

    // Start is called before the first frame update
    void Start()
    {
        float ground = whatIsGround.value;
        pos = transform.position;
        anim = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
     
     //   timer-=Time.deltaTime;
        

        if (IsPlayerCheck() || IsPlayer2Check())//통과해서 판독이 어려움
        {
            anim.SetBool("Move", true);
            Invoke("start", 1);
        }


    }

    private void start()
    {
        //timer = 3;
       // {


            anim.SetBool("Move", false);
         //   Debug.Log(1);
      //transform.position = Vector3.MoveTowards(pos, new Vector2(transform.position.x, IsGroundDetected().collider.gameObject.transform.position.y ), Time.deltaTime * speed);
       // }
    }

    private void back()
    {
      //  transform.position = Vector3.MoveTowards(new Vector2(transform.position.x, IsGroundDetected().collider.gameObject.transform.position.y ), pos, Time.deltaTime * speed);
    }

    public bool IsPlayerCheck() => Physics2D.Raycast(playerCheck.position, Vector2.down, playerCheckDistance, whatIsPlayer);
    public bool IsPlayer2Check() => Physics2D.Raycast(playerCheck2.position, Vector2.down, playerCheckDistance, whatIsPlayer);
   // public bool IsGroundDetected() => Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);
    public RaycastHit2D IsGroundDetected() => Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);


    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(playerCheck.position, new Vector3(playerCheck.position.x,
            playerCheck.position.y - playerCheckDistance));
        Gizmos.DrawLine(playerCheck2.position, new Vector3(playerCheck2.position.x,
            playerCheck2.position.y - playerCheckDistance));

        Gizmos.DrawLine(groundCheck.position, new Vector3(groundCheck.position.x,
          groundCheck.position.y - groundCheckDistance));
    }




}
