using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Castle : MonoBehaviour
{
    //flag flag = new flag();
    GameObject flag;
    Vector2 pos;
	Vector2 pos2;
    bool isCatleCo = false;


    void Start()
    {
		flag = GameObject.Find("flag_finish");
		//Debug.Log(flag);

        pos = transform.position;
        pos2 = new Vector2(pos.x, pos.y + 2.2f);
    }

    // Update is called once per frame
    void Update()
    {
        if (flag.GetComponent<Flag>().isCoroutineStart && !isCatleCo)
        {
			isCatleCo = true;
			StartCoroutine(CastleFlagUp());
		}
	}

	IEnumerator CastleFlagUp()
	{
        AudioManager.instance.PlayerOneShot(MARIO_SOUND.CASTLE, false, 1);
		//Debug.Log("CastleFlagUp() µé¾î¿È!!");
		while (Vector2.Distance(transform.position, pos2) > 1)
		{
			//Debug.Log("CastleFlagUp()  while µé¾î¿È!!");
			transform.position = Vector2.MoveTowards(transform.position, new Vector2(pos.x, pos2.y), 0.02f);
			//Debug.Log("CastleFlagUp()  castleFlag.position ¼öÁ¤ÄÚµå µé¾î¿È!!");
			yield return new WaitForSeconds(0.05f);
		}

		yield return null;
    }
}
