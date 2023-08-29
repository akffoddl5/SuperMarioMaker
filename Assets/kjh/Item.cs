using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    Vector3 current_pos;
    Vector3 des_pos;
    Collider2D collider;

    private void Awake()
    {
        collider = GetComponent<Collider2D>();
        collider.enabled = false;
        current_pos = transform.position;
        des_pos = current_pos + new Vector3(0, 1f, 0);
        Spawn();
    }

    void Start()
    {
        
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
        yield break;

    }

    public void Spawn()
    {
        StartCoroutine(ISpawn());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
