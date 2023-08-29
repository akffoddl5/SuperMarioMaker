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
        anim.SetBool("Move", false);


        if (IsPlayerCheck() || IsPlayer2Check())
        {
            anim.SetBool("Move", false);
           
            Invoke("start", 1);
            Invoke("back", 3f);

        }


    }

    private void start()
    {
        anim.SetBool("Move", true);

        transform.position = Vector3.MoveTowards(pos, new Vector2(transform.position.x, IsGroundDetected().collider.gameObject.transform.position.y ), Time.deltaTime * speed);
    }

    private void back()
    {
        transform.position = Vector3.MoveTowards(new Vector2(transform.position.x, IsGroundDetected().collider.gameObject.transform.position.y ), pos, Time.deltaTime * speed);
    }

    public bool IsPlayerCheck() => Physics2D.Raycast(playerCheck.position, Vector2.down, playerCheckDistance, whatIsPlayer);
    public bool IsPlayer2Check() => Physics2D.Raycast(playerCheck2.position, Vector2.down, playerCheckDistance, whatIsPlayer);
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
