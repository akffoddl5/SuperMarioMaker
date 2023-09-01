using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Castle : MonoBehaviour
{
    flag flag = new flag();
    Vector2 pos;
    Vector2 pos2;
    // Start is called before the first frame update
    void Start()
    {
        pos = transform.position;
        pos2 = new Vector2(pos.x, pos.y + 2.2f);
    }

    // Update is called once per frame
    void Update()
    {//
     //Debug.Log(flag.move);
        if (flag.move==true)
        {

        
            Flag_up();
        }
    }





    public void Flag_up()
    {
        StartCoroutine(Drop());
    }

    IEnumerator Drop()
    {
        while (Vector2.Distance(transform.position, pos2) > 1)
        { // 계속내려감 [땅이 닿을때까지]
            transform.position = Vector2.MoveTowards(transform.position, pos2, 0.00005f);
            yield return new WaitForSeconds(0.05f);

        }

        yield return null;

        // dropCoroutine = null;

    }
}
