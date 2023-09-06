using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Pipe_top : MonoBehaviour
{
    public bool isActive = false;
    LineRenderer lineRenderer;
    public Transform MyTransform;
    public Transform myTransform { get { return MyTransform; } }
    public Vector3 linkObjectPos { get; set; } = new Vector3(0, 0, -100);
    public GameObject linkObject;
    public bool lineActive { get; set; } = false;

    [SerializeField] float lineWidth = 0.15f;

    public int dirInfo = 0;     //(위 : 0, 오른 : 1, 아래 : 2, 왼 : 3)

    Rigidbody2D rb;
    CapsuleCollider2D cc;
    SpriteRenderer sr;

    //전체 플레이어 배열로
    GameObject[] Player;
    //플레이어 숫자 변수
    int playerNum;
    //기존 파이프 vector2값
    public Vector2 oriPipeVec;
    //이어진 파이프 vector2값
    public Vector2 connectPipeVec;

    private void Awake()
    {
            Player = GameObject.FindGameObjectsWithTag("Player");
            rb = Player[playerNum].GetComponent<Rigidbody2D>();
            cc = Player[playerNum].GetComponent<CapsuleCollider2D>();
            sr = GetComponentInChildren<SpriteRenderer>();

            Player[playerNum].GetComponent<Transform>();
            Player[playerNum].GetComponent<Animator>();

        connectPipeVec = lineRenderer.GetPosition(1);
        oriPipeVec = lineRenderer.GetPosition(0);
    }

    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;
        lineRenderer.SetPosition(0, myTransform.position);
        lineRenderer.SetPosition(1, myTransform.position);
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(lineActive);
        //라인 연결 코드
        if (lineActive)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            if (linkObjectPos == new Vector3(0, 0, -100))
            {
                lineRenderer.SetPosition(1, mousePos);

                connectPipeVec = lineRenderer.GetPosition(1);
                oriPipeVec = lineRenderer.GetPosition(0);
            }
            else
            {
                lineRenderer.SetPosition(1, linkObjectPos);
            }
        }

        connectPipeVec = lineRenderer.GetPosition(1);
        oriPipeVec = lineRenderer.GetPosition(0);

        //동작 OnOff, 라인렌더러 OffOn
        if (!isActive)
        {
            lineRenderer.enabled = true;
            return;
        }
        else if (lineRenderer.enabled)
        {
            lineRenderer.enabled = false;
        }

        //Vector2 mouseposition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        //if (Input.GetMouseButtonDown(0))
        //{
        //    connectPipeVec = mouseposition;
        //}

        //if (Input.GetMouseButtonDown(1))
        //{
        //    oriPipeVec = mouseposition;
        //}
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject == Player[playerNum])
        {
            switch (dirInfo)
            {
                //입구 위
                case 0:
                    if (Input.GetKeyDown(KeyCode.DownArrow))
                    {
                        pipeDir("Down");
                    }
                    break;

                //입구 오른쪽
                case 1:
                    if (Input.GetKeyDown(KeyCode.LeftArrow))
                    {
                        pipeDir("Left");
                    }
                    break;

                //입구 왼쪽
                case 3:
                    if (Input.GetKeyDown(KeyCode.RightArrow))
                    {
                        pipeDir("Right");
                    }
                    break;

                //입구 아래
                case 2:
                    if (Input.GetKeyDown(KeyCode.UpArrow))
                    {
                        pipeDir("Up");
                    }
                    break;

                default: break;
            }
        }
    }

    private void pipeDir(string Dir)
    {
        StartCoroutine($"slow{Dir}");

        rb.gravityScale = 0;
        cc.enabled = false;
        sr.sortingOrder = 2;
    }

    IEnumerator slowDown()
    {
        for (int i = 0; i < 4; i++)
        {
            rb.velocity = Vector2.zero;
            yield return new WaitForSeconds(0.2f);
            Player[playerNum].transform.Translate(new Vector2(0, -0.01f));
        }

        pipeMovement();
    }

    IEnumerator slowUp()
    {
        for (int i = 0; i < 4; i++)
        {
            rb.velocity = Vector2.zero;
            yield return new WaitForSeconds(0.2f);
            Player[playerNum].transform.Translate(new Vector2(0, 0.01f));
        }
        pipeMovement();
    }

    IEnumerator slowLeft()
    {
        for (int i = 0; i < 4; i++)
        {
            rb.velocity = Vector2.zero;
            yield return new WaitForSeconds(0.2f);
            Player[playerNum].transform.Translate(new Vector2(-0.01f, 0));
        }
        pipeMovement();
    }

    IEnumerator slowRight()
    {
        for (int i = 0; i < 4; i++)
        {
            rb.velocity = Vector2.zero;
            yield return new WaitForSeconds(0.2f);
            Player[playerNum].transform.Translate(new Vector2(0.01f, 0));
        }

        pipeMovement();
    }

    private void pipeMovement()
    {
        if (Player[playerNum].transform.position.x - oriPipeVec.x < 1.2f
            && Player[playerNum].transform.position.x - oriPipeVec.x > -1.2f)
        {
            new WaitForSeconds(0.3f);
            Player[playerNum].transform.position = connectPipeVec;

            rb.gravityScale = 3;
            cc.enabled = true;
            sr.sortingOrder = 1;

        }

        else if (Player[playerNum].transform.position.x - connectPipeVec.x < 1.2f
            && Player[playerNum].transform.position.x - connectPipeVec.x > -1.2f)
        {
            new WaitForSeconds(0.3f);
            Player[playerNum].transform.position = oriPipeVec;

            rb.gravityScale = 3;
            sr.sortingOrder = 1;
            cc.enabled = true;
        }
    }
}