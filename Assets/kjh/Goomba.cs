using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goomba : Enemy
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        
            move = true;




    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        TouchDown();



    }

    private void TouchDown()
    {

       if (IsSkyDetected())
        {
            
            move = false;

            Destroy(gameObject, 1);
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {


        if (collision.collider.gameObject.CompareTag("Enemy"))
        {


            transform.Rotate(0, 180, 0);


            moveflip = moveflip * -1;


        }
        else if (collision.collider.gameObject.CompareTag("Enemy_Shell"))
        {

            this.GetComponent<CapsuleCollider2D>().isTrigger = true;
            this.rb.position = new Vector2(spdX, spdY + 1);
            anim.SetBool("Stop", true);





        }
    }
}
