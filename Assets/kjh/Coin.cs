using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : Item
{
    

    

    public override void Spawn()
    {
        StartCoroutine(Coin_Spawn());
    }

    IEnumerator Coin_Spawn() {

         GetComponent<Collider2D>().enabled = false;

        Vector3 des_pos = transform.position + new Vector3(0, 3.5f, 0);
        while (true)
        {
            if (Vector3.Distance(transform.position, des_pos) < 0.01f) break;
            transform.position = Vector3.Lerp(transform.position, des_pos, 0.1f);
            yield return null;
        }
        Destroy(gameObject,0.1f);
        yield break;
        
     }

    protected override void Awake()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Player"))
     {
            Destroy(gameObject);
      AudioManager.instance.PlayerOneShot(MARIO_SOUND.COIN, false, 2);


        }

    }

    public override string Get_Prefab_Path()
    {
        return "Prefabs/Coin1";
    }
}
