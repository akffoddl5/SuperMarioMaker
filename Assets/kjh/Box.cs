using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Unity.VisualScripting;
using static Unity.Burst.Intrinsics.X86.Avx;

public class Box : MonoBehaviour
{

    float posy;
    Animator anim;
    public float upForce = 3f;
    public float resetTimer = 0.2f;
    public Queue<int> items = new Queue<int>();
    public int stateNum = 0; //(기본 블럭 : 0, 물음표 : 1)
    public float collision_cool_max;
    public float collision_cool;

    public GameObject _mushroom;
    public GameObject _star;
    public GameObject _flower;
    public GameObject _coin;
    List<GameObject> item_list = new List<GameObject>();

    public List<int> init_item_num_list = new List<int>();

    public SpriteRenderer SR;
    public Sprite empty_SR;

    public GameObject brokenBrick;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        posy = transform.position.y;
        for (int i = 0; i < init_item_num_list.Count; i++)
        {
            items.Enqueue(init_item_num_list[i]);

        }

        item_list.Add(_mushroom);
        item_list.Add(_star);
        item_list.Add(_flower);
        item_list.Add(_coin);


        collision_cool_max = 15 * Time.deltaTime;
    }

    // Update is called once per frame
    void Update()
    {
        collision_cool -= Time.deltaTime;
        //Debug.Log(items.Count);
    }

    public void Add_Item(int obj)
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
        if (collision.gameObject.tag == "Player" && collision_cool < 0 && collision.gameObject.GetComponent<PhotonView>().IsMine)
        {
            collision_cool = collision_cool_max;
            if (collision.otherCollider.gameObject.name == "boxmove") return;
            //anim.SetBool("Move", true);
            //gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.up*upForce);
            transform.Translate(new Vector2(0, 0.1f));
            StartCoroutine(IJump((collision.gameObject)));

            if (items.Count > 0)
            {
                int tmp1 = items.Dequeue();
                GameObject tmp;
                tmp = item_list[tmp1];

                Debug.Log((tmp.GetComponent<Item>().Get_Prefab_Path() + " 스폰되야함" + tmp.name + " " + tmp.GetComponent<Item>().isSpawn));
                var a = PhotonNetwork.Instantiate(tmp.GetComponent<Item>().Get_Prefab_Path(), transform.position, Quaternion.identity);
                a.GetComponent<Item>().Spawn();

                
            }
            else if (items.Count <= 0)
            {
                if (stateNum == 0)
                {
                    //부셔지기
                    
                    Destroy(PhotonNetwork.Instantiate("Prefabs/BrokenBrick", transform.position, Quaternion.identity), 0.5f);
                    Destroy(gameObject);
                }
                else
                {
                    //나무로 바뀌기
                    anim.Play("");
                    SR.sprite = empty_SR;
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



