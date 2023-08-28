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
        Move(true);

    }


    private void FixedUpdate()
    {
    }

    

    public override void Die()
    {
        Debug.Log("Die");

        //PV.RPC("RPC_Die", RpcTarget.AllBuffered);
       
         PhotonNetwork.Instantiate("Prefabs/Koopa_Shell", new Vector2(transform.position.x, transform.position.y), Quaternion.identity);
        

        PV.RPC("RPC_Die", RpcTarget.AllBuffered);
        //Instantiate(shell, new Vector2(transform.position.x, transform.position.y), Quaternion.identity);
        
        //Destroy(gameObject);

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

        if (collision.gameObject.GetComponent<Enemy_shell>() != null && !collision.gameObject.GetComponent<Enemy_shell>().fsecMove)// && collision.collider.GetComponent<Rigidbody2D>().velocity == new Vector2(0, 0))
        {
            transform.Rotate(0, 180, 0);


            moveflip = moveflip * -1;
        }
        else if (collision.gameObject.GetComponent<Enemy_shell>() != null)
        {

           if (!PhotonNetwork.IsMasterClient) return;
            var a= PhotonNetwork.Instantiate("Prefabs/Koopa_Stop", new Vector2(transform.position.x, transform.position.y), Quaternion.identity);
            //var a = Instantiate(stop, new Vector2(transform.position.x, transform.position.y), Quaternion.identity);


            a.GetComponent<Rigidbody2D>().velocity = new Vector2(collision.transform.position.x, 0);
            PV.RPC("RPC_Die", RpcTarget.AllBuffered);
            //Destroy(gameObject);

        }
        else if (collision.collider.gameObject.CompareTag("Enemy"))
        {


            transform.Rotate(0, 180, 0);


            moveflip = moveflip * -1;


        }

        
    }
}
