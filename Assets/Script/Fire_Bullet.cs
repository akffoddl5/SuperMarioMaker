using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire_Bullet : MonoBehaviour
{
    Rigidbody2D rb;
    public int faceDir;
    public float moveSpeed;
    public float jumpPower;
    

    private void Awake()
    {
        
        rb = GetComponent<Rigidbody2D>();
        
    }

    void Start()
    {
        
    }

    void Update()
    {
        rb.velocity = new Vector2(faceDir * moveSpeed, rb.velocity.y);

    }

    public bool GroundDetected()
    {
        //Debug.Log("ground detect" + rb.velocity.magnitude);
        if (rb.velocity.magnitude < 1) Destroy(this.gameObject);
        return Physics2D.Raycast(transform.position, Vector2.down, 0.5f, LayerMask.GetMask("Ground"));
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // ���� ������ Ƣ�� ���� 
        if (collision.gameObject.tag == "Ground" && GroundDetected())
        {
            rb.AddForce(new Vector2(0, jumpPower));
        }
        // �� ���̰� �����
        else if (collision.gameObject.CompareTag("Enemy"))
        {
            // �ź��� �������� �̰� ����
            if(collision.gameObject.GetComponent<Enemy_shell>() != null)
            {
				collision.gameObject.GetComponent<Enemy_shell>().FilpOverDie();
			}
            // �ƴ϶�� �̰� ����
            else
            {
                collision.gameObject.GetComponent<Enemy>().FilpOverDie();
            }

            if (GetComponent<PhotonView>().IsMine)
            {
                GetComponent<PhotonView>().RPC("DestroySelf_RPC", RpcTarget.All);
			}
		}
        else
        {
			if (GetComponent<PhotonView>().IsMine)
			{
                GetComponent<PhotonView>().RPC("DestroySelf_RPC", RpcTarget.All);
			}
			//Debug.Log(collision.gameObject.tag + " " + GroundDetected());
        }
    }

    [PunRPC]
    void DestroySelf_RPC()
    {
        // �Ѿ� �ڱ��ڽ� ����
		Destroy(gameObject);
	}

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * 1f) ;
    }
}
