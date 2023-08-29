using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class JumpBoard : MonoBehaviour
{
   public Animator anim;
  //bool jumpBoard = false;
    Mario mario;
   // public GameObject player;
    // Rigidbody rb;
    void Start()
    {


        
        // rb= GetComponent<Rigidbody>();
    }


    void Update()
    {

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
           // jumpBoard = true;
            anim.SetBool("Jump", true);

            StartCoroutine(IJump((collision.gameObject)));
           // Invoke(jumpBoard)
          
        }

    }

    IEnumerator IJump(GameObject obj)
    {
        yield return new WaitForSeconds(0.1f);
        obj.GetComponent<Mario>().rb.AddForce(Vector3.up * 1000);
        anim.SetBool("Jump", false);
    }


}
