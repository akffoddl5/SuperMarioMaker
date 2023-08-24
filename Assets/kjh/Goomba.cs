using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goomba : Enemy
{
    public GameObject stop;
    
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

            Debug.Log("flag0");

        
        if (collision.gameObject.GetComponent<Enemy_shell>() != null && false ==  collision.gameObject.GetComponent<Enemy_shell>().fsecMove)//&& collision.collider.GetComponent<Rigidbody2D>().velocity == new Vector2(0, 0))
        {

            Debug.Log("flag1");
            transform.Rotate(0, 180, 0);


            moveflip = moveflip * -1;
        }
        else if (collision.gameObject.GetComponent<Enemy_shell>() != null)
        {
            Debug.Log("flag2");
            Destroy(gameObject);

            var a = Instantiate(stop, new Vector2(transform.position.x, transform.position.y), Quaternion.identity);


            a.GetComponent<Rigidbody2D>().velocity = new Vector2(collision.transform.position.x, 0);

        }else if (collision.collider.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("flag3");


            transform.Rotate(0, 180, 0);


            moveflip = moveflip * -1;


        }


    }
}
