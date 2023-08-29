using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    Vector3 current_pos;
    Vector3 des_pos;

    public bool isSpawn = false;

    Collider2D collider;
	public Rigidbody2D rb;
	protected virtual void Awake()
    {
        collider = GetComponent<Collider2D>();
		rb = GetComponent<Rigidbody2D>();
		collider.enabled = false;
        rb.gravityScale = 0f;
        current_pos = transform.position;
        des_pos = current_pos + new Vector3(0, 1f, 0);
        Spawn();
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void Spawn()
    {
        StartCoroutine(ISpawn());
    }

    IEnumerator ISpawn()
    {
        while (true)
        {
            if (Vector3.Distance(transform.position, des_pos) < 0.2f) break;
            transform.position = Vector3.MoveTowards(transform.position, des_pos, 0.1f);
            yield return new WaitForSeconds(0.15f);
        }

        collider.enabled = true;
        rb.gravityScale = 3f;
        isSpawn = true;
        yield break;
        
    }



}
