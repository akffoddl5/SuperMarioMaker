using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Unity.VisualScripting;

public class Box : MonoBehaviour
{

    float posy;
    Animator anim;
    public float upForce = 3f;
    public float resetTimer = 0.2f;
    public List<GameObject> init_item_list = new List<GameObject>();
    public Queue<GameObject> items = new Queue<GameObject>();
    public int stateNum = 0; //(기본 블럭 : 0, 물음표 : 1)
    public float collision_cool_max;
    public float collision_cool;

    public SpriteRenderer SR;
    public Sprite empty_SR;

    // Start is called before the first frame update
    void Start()
    {
        anim= GetComponentInChildren<Animator>();
        posy = transform.position.y;
        for (int i = 0; i < init_item_list.Count; i++)
        {
            items.Enqueue(init_item_list[i]);
        }

        collision_cool_max = 10 * Time.deltaTime;
    }

    // Update is called once per frame
    void Update()
    {
        collision_cool -= Time.deltaTime;
    }

    public void Add_Item(GameObject obj)
    {
        items.Enqueue(obj);
    }

    public void Add_Item_Num(List<int> objNumList)
    {
        Debug.Log("Add_Item_Num");
        init_item_num_list = objNumList;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" && collision_cool < 0)
        {
            collision_cool = collision_cool_max;
            if (collision.otherCollider.gameObject.name == "boxmove") return;
             //anim.SetBool("Move", true);
            //gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.up*upForce);
            transform.Translate(new Vector2(0, 0.1f));
            StartCoroutine(IJump((collision.gameObject)));

            if (items.Count > 0)
            {
                GameObject tmp = items.Dequeue();
                Debug.Log((tmp.GetComponent<Item>().Get_Prefab_Path() + " 스폰되야함" + tmp.name + " " + tmp.GetComponent<Item>().isSpawn));
                var a = PhotonNetwork.Instantiate(tmp.GetComponent<Item>().Get_Prefab_Path(), transform.position, Quaternion.identity);
                a.GetComponent<Item>().Spawn();

                if (items.Count <= 0)
                {
                    if (stateNum == 0)
                    {
                        //부셔지기
                    }
                    else
                    {
                        //나무로 바뀌기
                        anim.Play("");
                        SR.sprite = empty_SR;
                    }
                }
            }
                
            
            //  Invoke("defort",0.3f);
        }
    }
    IEnumerator IJump(GameObject obj)
    {
        yield return new WaitForSeconds(resetTimer);
        gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        transform.position = new Vector2(transform.position.x, posy);
        //anim.SetBool("Move", false);
    }

}



