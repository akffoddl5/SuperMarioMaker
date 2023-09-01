using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishSoloStage : MonoBehaviour
{
    public bool isFinishGame = false;
    bool isCoroutineStart = false;
    Vector2 minusVector = new Vector2(0.1f, 0.1f); 
    Vector2 finishScale = new Vector2(3, 3);
    // Start is called before the first frame update
    void Start()
    {
        isFinishGame = true;
        transform.localPosition = Vector3.zero;
        transform.localScale = new Vector2(40, 40);
    }

    // Update is called once per frame
    void Update()
    {
        if (isFinishGame && !isCoroutineStart)
        {
            isCoroutineStart = true;
            StartCoroutine(FinishGame_EF());
        }
    }
    IEnumerator FinishGame_EF()
    {
        while(transform.localScale.x <= finishScale.x)
        {
            Debug.Log("IEnumerator FinishGame_EF() µé¾î¿È!!!");
            Vector2 tmp = new Vector2(transform.localScale.x - minusVector.x, transform.localScale.y - minusVector.y);
            transform.localScale = tmp;
            yield return new WaitForSeconds(0.5f);

        }
    }
}
