 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flag : MonoBehaviour
{
    //Coroutine
    Vector2 pos;
    Vector2 pos2;
    public bool isCoroutineStart = false;
    GameObject finishEffect;

	// Start is called before the first frame update
	void Start()
    {
        pos = transform.position;
        pos2 = new Vector2(pos.x, pos.y - 8.66f);
        finishEffect = GameObject.Find("soloStage");

	}
    
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !isCoroutineStart)
        {
            isCoroutineStart = true;
			StartCoroutine(Drop());
		}
    }

    IEnumerator Drop()
    {
        while (Vector2.Distance(transform.position, pos2) > 1)
        {
            transform.position  = Vector2.MoveTowards(transform.position, pos2, 0.1f);
            yield return new WaitForSeconds(0.05f);
        }

		// When the while() statement is exited, execute.
		finishEffect.GetComponent<FinishSoloStage>().isFinishGame = true;
		yield return null;
    }
}
