using Cinemachine.Utility;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.PackageManager;
using UnityEngine;


public class Pipe_top : MonoBehaviour
{
    public bool isActive = false;
    LineRenderer lineRenderer;
    public Transform MyTransform;
    public Transform myTransform { get { return MyTransform; } }
    public Vector3 linkObjectPos { get; set; } = new Vector3(0, 0, -100);
    public Vector3 LinkObjectPos;

    public GameObject linkObject;
    Collider2D linkObjectCol;
    public bool lineActive { get; set; } = false;

    [SerializeField] float lineWidth = 0.15f;

    public int dirInfo = 0;     //(위 : 0, 오른 : 1, 아래 : 2, 왼 : 3)

    BoxCollider2D bc;

    GameObject Player;
    //플레이어 숫자 변수
    int playerNum;
    //기존 파이프 vector2값
    public Vector3 oriPipeVec;
    //이어진 파이프 vector2값
    public Vector3 connectPipeVec;

    Vector2 pipeDir;
    Vector2 moveDir;
    [SerializeField] Transform rayBox;
    public float rayBoxX = 1;
    public float rayBoxY = 1;
    [SerializeField] LayerMask layerMask;

    private void Awake()
    {
        //Player = GameObject.FindGameObjectWithTag("Player");

        bc = GetComponent<BoxCollider2D>();



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
        LinkObjectPos = linkObjectPos;
        //Vector2 mouseposition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        //if (Input.GetMouseButtonDown(0))
        //{
        //    connectPipeVec = mouseposition;
        //}

        //if (Input.GetMouseButtonDown(1))
        //{
        //    oriPipeVec = mouseposition;
        //}
        linkObjectCol = Physics2D.OverlapArea(linkObjectPos, Vector2.one * 0.01f);
        linkObject = linkObjectCol.gameObject;


        switch (dirInfo)
        {
            case 0:
                pipeDir = Vector2.up;
                moveDir = Vector2.down;
                break;
            case 1:
                pipeDir = Vector2.right;
                moveDir = Vector2.left;
                break;
            case 2:
                pipeDir = Vector2.down;
                moveDir = Vector2.up;
                break;
            case 3:
                pipeDir = Vector2.left;
                moveDir = Vector2.right;
                break;
        }

        if (IsPlayerDetected(out Player))
        {
            //Debug.Log("인식");
            switch (dirInfo)
            {
                case 0:
                    if (Input.GetKeyDown(KeyCode.DownArrow))
                    {
                        StartCoroutine("moveDown");
                    }
                    break;
                case 1:
                    if (Input.GetKeyDown(KeyCode.LeftArrow))
                    {
                        StartCoroutine("moveDown");
                    }
                    break;
                case 2:
                    if (Input.GetKeyDown(KeyCode.UpArrow))
                    {
                        StartCoroutine("moveDown");
                    }
                    break;
                case 3:
                    if (Input.GetKeyDown(KeyCode.RightArrow))
                    {
                        StartCoroutine("moveDown");
                    }
                    break;
            }

        }
    }

    //private void OnCollisionStay2D(Collision2D collision)
    //{
    //    if (collision.gameObject.CompareTag("Player"))
    //    {
    //        if (Input.GetKeyDown(KeyCode.DownArrow))
    //        {
    //            StartCoroutine("moveDown");
    //        }
    //    }
    //}

    IEnumerator moveDown()
    {
        bc.isTrigger = true;

        for (int i = 0; i < 4; i++)
        {
            Player.transform.Translate(moveDir * 0.2f);
        }
        yield return new WaitForSeconds(0.6f);

        pipeMovement();
    }

    private void pipeMovement()
    {
        StartCoroutine("moveUp");
    }

    IEnumerator moveUp()
    {

        Player.transform.position = linkObjectPos;


        for (int i = 0; i < 4; i++)
        {
            Player.transform.Translate(linkObject.GetComponent<Pipe_top>().pipeDir * 0.2f);
        }

        yield return new WaitForSeconds(0.6f);

        bc.isTrigger = false;
    }

    public bool IsPlayerDetected(out GameObject player)
    {
        RaycastHit2D hit = Physics2D.BoxCast(rayBox.position, new Vector2(rayBoxX, rayBoxY), 0f, pipeDir, layerMask);
        player = hit.collider.gameObject;

        return hit;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawCube(rayBox.position, new Vector3(rayBoxX, rayBoxY));
    }

}