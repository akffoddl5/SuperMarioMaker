 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flag : MonoBehaviour
{
    GameObject groundObject;
//    Coroutine dropCoroutine;
    Vector2 pos;
    Vector2 pos2;
   static public bool move=false;
    // Start is called before the first frame update
    void Start()
    {
        pos = transform.position;
        pos2 = new Vector2(pos.x, pos.y - 8.66f);
    }

    // Update is called once per frame
    void Update()
    {
        
        
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Flag_Down();
           
        }
    }
   
    public void Flag_Down()
    {
        StartCoroutine(Drop());
    }

    IEnumerator Drop()
    {
        var groundDist = 100f;
        while (Vector2.Distance(transform.position, pos2) > 1)
        { // 계속내려감 [땅이 닿을때까지]
            transform.position  = Vector2.MoveTowards(transform.position, pos2, 0.1f);
            yield return new WaitForSeconds(0.05f);

            move = true;
        }
       
        
        yield return null;

       // dropCoroutine = null;
    }
}
