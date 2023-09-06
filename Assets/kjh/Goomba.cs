using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goomba : Enemy
{
    public GameObject stop;
    public bool isFlat = false;
    
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
        if (!isFilpOverDie)TouchDown();
    }

    private void TouchDown()
    {
        
        //Debug.Log("TouchDown()"// + isFilpOverDie);
        if (IsSkyDetected())
        {
            //Debug.Log("IsSkyDetected(): " + isFilpOverDie);

            move = false;
            anim.SetBool("Flat", true);
            isFlat = true;
			// gameObject.GetComponent<Collider2D>().enabled = false;
			gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;

			Destroy(gameObject, 1f);
        }
    }
	public override void FilpOverDie()
	{
		base.FilpOverDie();
        
	}


	private void OnCollisionEnter2D(Collision2D collision)
    {
        // not moving enemyshell
        if (collision.gameObject.GetComponent<Enemy_shell>() != null && collision.gameObject.GetComponent<Enemy_shell>().fsecMove == false)//&& collision.collider.GetComponent<Rigidbody2D>().velocity == new Vector2(0, 0))
        {
           
            transform.Rotate(0, 180, 0);
            moveflip = moveflip * -1;
        }
        // moving enemyshell
        else if (collision.gameObject.GetComponent<Enemy_shell>() != null)
        {
            //Destroy(gameObject);
            //var a = Instantiate(stop, new Vector2(transform.position.x, transform.position.y), Quaternion.identity);
            //a.GetComponent<Rigidbody2D>().velocity = new Vector2(collision.transform.position.x, 0);
            FilpOverDie();

		}
        // enemy
		else if (collision.collider.gameObject.CompareTag("Enemy"))
        {
            transform.Rotate(0, 180, 0);
            moveflip = moveflip * -1;
        }
    }
}
