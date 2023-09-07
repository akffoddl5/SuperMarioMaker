using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pipe_bottom : MonoBehaviour
{
    CapsuleCollider2D cap;

    private void Awake()
    {
        cap = GetComponent<CapsuleCollider2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            cap.isTrigger = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            cap.isTrigger = false;
        }
    }
}
