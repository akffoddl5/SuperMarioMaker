using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Enemy_Koopa : Enemy
{
    public  GameObject shell;
    public  GameObject stop;
    
    public Enemy_Koopa()
    {
    }

    protected override void Start()
    {
        base.Start();
        move = true;
        
    }

    protected override void Update()
    {
        base.Update();
        //Move(true);

    }

    private void FixedUpdate()
    {
      //  Debug.Log(rb.velocity.x);
        if (isDie) return;

        if (rb.velocity.x > 0)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else if (rb.velocity.x < 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }

    public override void Die()
    {
        //PV.RPC("RPC_Die", RpcTarget.AllBuffered);
        
         PhotonNetwork.Instantiate("Prefabs/Koopa_Shell", new Vector2(transform.position.x, transform.position.y), Quaternion.identity);
        

        PV.RPC("RPC_Die", RpcTarget.AllBuffered);
        //Instantiate(shell, new Vector2(transform.position.x, transform.position.y), Quaternion.identity);
        
        //Destroy(gameObject);

    }
	public override void FilpOverDie()
	{
		base.FilpOverDie();

        isDie = true;
	}

	[PunRPC]
    public void RPC_Die()
    {
        
        //PhotonNetwork.Instantiate("Prefabs/Koopa_Shell", new Vector2(transform.position.x, transform.position.y), Quaternion.identity);
        //Instantiate(shell, new Vector2(transform.position.x, transform.position.y), Quaternion.identity);
        
        Destroy(gameObject);
    }
    

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //  Debug.Log(Enemy_shell.fsecMove);
       
        // Shell Tuttle collision && Shell is not moving
        // tuttle have to move flip side
        if (collision.gameObject.GetComponent<Enemy_shell>() != null && !collision.gameObject.GetComponent<Enemy_shell>().fsecMove)// && collision.collider.GetComponent<Rigidbody2D>().velocity == new Vector2(0, 0))
        {
            transform.Rotate(0, 180, 0);
            moveflip = moveflip * -1;
        }
        // moving shell kill other tuttles
        else if (collision.gameObject.GetComponent<Enemy_shell>() != null)
        {

           if (!PhotonNetwork.IsMasterClient) return;
            var a= PhotonNetwork.Instantiate("Prefabs/Koopa_Stop", new Vector2(transform.position.x, transform.position.y), Quaternion.identity);
            //var a = Instantiate(stop, new Vector2(transform.position.x, transform.position.y), Quaternion.identity);

            a.GetComponent<Rigidbody2D>().velocity = new Vector2(collision.transform.position.x, 0);
            PV.RPC("RPC_Die", RpcTarget.AllBuffered);
            //Destroy(gameObject);

        }
        // tuttle collision with other enemy
        else if (collision.collider.gameObject.CompareTag("Enemy"))
        {
            transform.Rotate(0, 180, 0);
            moveflip = moveflip * -1;
        }
    }
}
