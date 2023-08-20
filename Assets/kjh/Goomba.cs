using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Goomba : MonoBehaviour
{


    [Header("Collision Info")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private Transform skyCheck;
    [SerializeField] private float SkyCheckDistance;

    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private LayerMask whatIsPlayer;

    public Rigidbody2D rb;
    Animator anim;
    float moveflip = 1;

    bool move = true;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Flat();

    }

    private void Flat()
    {
        
        if (IsSkyDetected())
        {
            anim.SetBool("Flat", true);
            move = false;
            Destroy(gameObject, 1);
        }
    }

    private void Move()
    {
        if (move)
        {

            rb.velocity = new Vector2(1 * moveflip, 0);

            if (!IsGroundDetected())
            {

                transform.Rotate(0, 180, 0);


                moveflip = moveflip * -1;

            }
        }
    }

    public bool IsGroundDetected() => Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);
    public bool IsSkyDetected() => Physics2D.Raycast(skyCheck.position, Vector2.up, groundCheckDistance, whatIsPlayer);

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position, new Vector3(groundCheck.position.x,
            groundCheck.position.y - groundCheckDistance));

        Gizmos.DrawLine(skyCheck.position, new Vector3(skyCheck.position.x,
            skyCheck.position.y + SkyCheckDistance));

    }
}
