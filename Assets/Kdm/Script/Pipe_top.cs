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

    public GameObject connectPipe;

    int pipeOriDir;
    int pipeConDir;

    private void Awake()
    {
        if (Player != null)
        {
            Player = GameObject.FindGameObjectsWithTag("Player");
            rb = Player[playerNum].GetComponent<Rigidbody2D>();
            cc = Player[playerNum].GetComponent<CapsuleCollider2D>();
            sr = Player[playerNum].GetComponent<SpriteRenderer>();

            Player[playerNum].GetComponent<Transform>();
            Player[playerNum].GetComponent<Animator>();
        }
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
            }
            else
            {
                lineRenderer.SetPosition(1, linkObjectPos);
            }

            connectPipeVec = lineRenderer.GetPosition(1);
            oriPipeVec = lineRenderer.GetPosition(0);

            pipeOriDir = this.gameObject.GetComponent<Pipe_top>().dirInfo;

            Debug.Log(oriPipeVec);
            Debug.Log(connectPipeVec);
            Debug.Log(pipeConDir);
            Debug.Log(pipeOriDir);
        }


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

        //파이프 동작 코드
        {

        }
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
    }

    #region slowDir_pipeIn

    IEnumerator slowDown()
    {
        rb.gravityScale = 0;
        cc.enabled = false;
        sr.sortingOrder = -1;

        for (int i = 0; i < 4; i++)
        {
            rb.velocity = Vector2.zero;
            yield return new WaitForSeconds(0.2f);
            Player[playerNum].transform.Translate(new Vector2(0, -0.4f));
        }

        pipeMovement();
    }

    IEnumerator slowUp()
    {
        rb.gravityScale = 0;
        cc.enabled = false;
        sr.sortingOrder = -1;

        for (int i = 0; i < 4; i++)
        {
            rb.velocity = Vector2.zero;
            yield return new WaitForSeconds(0.2f);
            Player[playerNum].transform.Translate(new Vector2(0, 0.4f));
        }

        rb.gravityScale = 3;
        cc.enabled = true;
        sr.sortingOrder = 1;

        pipeMovement();
    }

    IEnumerator slowLeft()
    {
        rb.gravityScale = 0;
        cc.enabled = false;
        sr.sortingOrder = -1;

        for (int i = 0; i < 4; i++)
        {
            rb.velocity = Vector2.zero;
            yield return new WaitForSeconds(0.2f);
            Player[playerNum].transform.Translate(new Vector2(-0.4f, 0));
        }

        rb.gravityScale = 3;
        cc.enabled = true;
        sr.sortingOrder = 1;

        pipeMovement();
    }

    IEnumerator slowRight()
    {
        rb.gravityScale = 0;
        cc.enabled = false;
        sr.sortingOrder = -1;

        for (int i = 0; i < 4; i++)
        {
            rb.velocity = Vector2.zero;
            yield return new WaitForSeconds(0.2f);
            Player[playerNum].transform.Translate(new Vector2(0.4f, 0));
        }

        rb.gravityScale = 3;
        cc.enabled = true;
        sr.sortingOrder = 1;

        pipeMovement();
    }
    #endregion

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject == Player[playerNum])
        {
            pipeConDir = pipeOriDir;
        }
    }

    private void pipeMovement()
    {
        if (Player[playerNum].transform.position.x - oriPipeVec.x < 1.2f
            && Player[playerNum].transform.position.x - oriPipeVec.x > -1.2f)
        {
            Player[playerNum].transform.position = connectPipeVec;
            StartCoroutine(pipeOutMovement(pipeConDir));
        }

        else if (Player[playerNum].transform.position.x - connectPipeVec.x < 1.2f
            && Player[playerNum].transform.position.x - connectPipeVec.x > -1.2f)
        {
            Player[playerNum].transform.position = oriPipeVec;
            StartCoroutine(pipeOutMovement(pipeOriDir));
        }
    }

    IEnumerator pipeOutMovement(int InOut)
    {
        switch (InOut)
        {
            //입구 위
            case 0:
                for (int i = 0; i < 4; i++)
                {
                    rb.velocity = Vector2.zero;
                    Player[playerNum].transform.Translate(new Vector2(0, +0.4f));
                }
                break;

            //입구 오른쪽
            case 1:
                for (int i = 0; i < 4; i++)
                {
                    rb.velocity = Vector2.zero;
                    Player[playerNum].transform.Translate(new Vector2(+0.4f, 0));
                }
                break;

            //입구 왼쪽
            case 3:
                for (int i = 0; i < 4; i++)
                {
                    rb.velocity = Vector2.zero;
                    Player[playerNum].transform.Translate(new Vector2(-0.4f, 0));
                }
                break;

            //입구 아래
            case 2:
                for (int i = 0; i < 4; i++)
                {
                    rb.velocity = Vector2.zero;
                    Player[playerNum].transform.Translate(new Vector2(0, -0.4f));
                }
                break;

            default: break;
        }

        rb.gravityScale = 3;
        cc.enabled = true;
        sr.sortingOrder = 1;

        yield return new WaitForSeconds(0.5f);
    }
}


