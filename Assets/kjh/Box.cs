using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{

    float posy;
    Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim= GetComponentInChildren<Animator>();
        posy = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.tag == "Player")
        {

             anim.SetBool("Move", true);
            gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.up*0.5f);
            StartCoroutine(IJump((collision.gameObject)));
            //  Invoke("defort",0.3f);
        }
    }
    IEnumerator IJump(GameObject obj)
    {
        yield return new WaitForSeconds(0.2f);
        gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero; 
        transform.position=new Vector2(transform.position.x,posy);
       //anim.SetBool("Move", false);
    }

}


    
