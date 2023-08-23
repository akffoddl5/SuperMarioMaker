using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Koopa : Enemy
{
   public  GameObject shell;
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
        if (IsSkyDetected())
        {
            Destroy(gameObject);
            Instantiate(shell, new Vector2(transform.position.x,transform.position.y), Quaternion.identity);
        }


    }

    private void OnCollisionEnter2D(Collision2D collision)
    {


       if (collision.collider.gameObject.CompareTag("Enemy"))
        {
            

                transform.Rotate(0, 180, 0);


                moveflip = moveflip * -1;


        }else if (collision.collider.gameObject.CompareTag("Enemy_Shell"))
        {

            this.GetComponent<CapsuleCollider2D>().isTrigger = true;
            this.rb.position = new Vector2(spdX, spdY + 1);

            anim.SetBool("Stop", true);



        }

    }
}
