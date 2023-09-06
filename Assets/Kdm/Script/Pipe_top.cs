using Cinemachine.Utility;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

    BoxCollider2D bc;
    SpriteRenderer sr;

    GameObject Player;
    //플레이어 숫자 변수
    int playerNum;
    //기존 파이프 vector2값
    public Vector3 oriPipeVec;
    //이어진 파이프 vector2값
    public Vector3 connectPipeVec;

    private void Awake()
    {
        Player = GameObject.FindGameObjectWithTag("Player");

        bc = GetComponent<BoxCollider2D>();
        sr = GetComponentInChildren<SpriteRenderer>();

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

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                StartCoroutine("moveDown");
            }
        }
    }

    IEnumerator moveDown()
    {
        bc.isTrigger = true;
        sr.sortingOrder = 2;

        for (int i = 0; i < 4; i++)
        {
            Player.transform.Translate(Vector3.down * 0.2f);
        }
        yield return new WaitForSeconds(0.6f);

        pipeMovement();
    }

    private void pipeMovement()
    {
        if (Player.transform.position.x - myTransform.position.x < 1.2f
            && Player.transform.position.x - myTransform.position.x > -1.2f)
        {
            StartCoroutine("moveUp");
        }

        else if (Player.transform.position.x - linkObjectPos.x < 1.2f
            && Player.transform.position.x - linkObjectPos.x > -1.2f)
        {
            StartCoroutine("moveUp");
        }
    }

    IEnumerator moveUp()
    {
        if (Player.transform.position.x - myTransform.position.x < 1.2f
    && Player.transform.position.x - myTransform.position.x > -1.2f)
        {
            Player.transform.position = linkObjectPos;
        }

        else if (Player.transform.position.x - linkObjectPos.x < 1.2f
    && Player.transform.position.x - linkObjectPos.x > -1.2f)
        {
            Player.transform.position = myTransform.position;
        }

        for (int i = 0; i < 4; i++)
        {
            Player.transform.Translate(Vector3.up * 0.2f);
        }

        yield return new WaitForSeconds(0.6f);

        bc.isTrigger = false;
        sr.sortingOrder = 1;
    }
}