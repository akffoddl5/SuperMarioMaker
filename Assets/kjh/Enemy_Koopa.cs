using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

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

        if (IsSkyDetected())
        {
            Instantiate(shell, new Vector2(transform.position.x, transform.position.y), Quaternion.identity);
            Destroy(gameObject);
        }


    }
    private void FixedUpdate()
    {
        


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

            Destroy(gameObject);

            var a = Instantiate(stop, new Vector2(transform.position.x, transform.position.y), Quaternion.identity);


            a.GetComponent<Rigidbody2D>().velocity = new Vector2(collision.transform.position.x, 0);

        }
        else if (collision.collider.gameObject.CompareTag("Enemy"))
        {


            transform.Rotate(0, 180, 0);


            moveflip = moveflip * -1;


        }
    }
}
