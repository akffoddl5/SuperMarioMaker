using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpBoard : MonoBehaviour
{
    Animator anim;
    bool jumpBoard=false;
    void Start()
    {
        anim= GetComponent<Animator>();
    }


    void Update()
    {

    }
    private void OnCollisionEnter(Collision collision)
    {if(collision.gameObject.tag=="Player")
            anim.SetBool("Jump", jumpBoard);
       // Invoke("jumpBoard", 0.5f);
    }




    //private void OnCollisionExit(Collision collision)
    //{
    //    if (collision.gameObject.tag == "Player")
    //        anim.SetBool("Jump", false);

    //}
}
