using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishSoloStage : MonoBehaviour
{
    public bool isFinishGame = false;
    bool isCoroutineStart = false;

    Vector2 minusVector = new Vector2(0.5f, 0.5f); 
    Vector2 finishScale = new Vector2(3, 3);
    Vector2 finishPosition; // 1µîÀÇ position

    // Start is called before the first frame update
    void Start()
    {
		gameObject.SetActive(false);
		transform.localPosition = Vector3.zero;
        transform.localScale = new Vector2(40, 40);
    }

    // Update is called once per frame
    void Update()
    {
        if (isFinishGame && !isCoroutineStart)
        {
            gameObject.SetActive(true);
            isCoroutineStart = true;
            StartCoroutine(FinishGame_EF());
        }
    }
    IEnumerator FinishGame_EF()
    {
        while(transform.localScale.x >= finishScale.x)
        {
            Vector2 tmp = new Vector2(transform.localScale.x - minusVector.x, transform.localScale.y - minusVector.y);
            transform.localScale = tmp;
            //Debug.Log("IEnumerator FinishGame_EF() µé¾î¿È!!!" + transform.localScale);
            yield return new WaitForSeconds(0.01f);

        }
    }
}
