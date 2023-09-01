 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flag : MonoBehaviour
{
    public GameObject castle;
    //Coroutine
    Vector2 pos;
    Vector2 pos2;
    bool isCoroutineStart = false;

	// Start is called before the first frame update
	void Start()
    {
        pos = transform.position;
        pos2 = new Vector2(pos.x, pos.y - 8.66f);
    }
    
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !isCoroutineStart)
        {
            Flag_Down();
        }
    }
   
    public void Flag_Down()
    {
        StartCoroutine(Drop());
        castle.GetComponentInChildren<Castle>().Flag_up();
	}

    IEnumerator Drop()
    {
        //var groundDist = 100f;
        while (Vector2.Distance(transform.position, pos2) > 1)
        { // 계속내려감 [땅이 닿을때까지]
            transform.position  = Vector2.MoveTowards(transform.position, pos2, 0.1f);
            yield return new WaitForSeconds(0.05f);
        }
        yield return null;
    }
}
