using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pipe_top : MonoBehaviour
{
    public bool isActive = false;
    LineRenderer lineRenderer;
    public Transform MyTransform;
    public Transform myTransform { get { return MyTransform; } }
    public Transform linkObjectTransform { get; set; }
    public bool lineActive { get; set; } = false;

    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        lineRenderer.SetPosition(0, myTransform.position);
    }

    // Update is called once per frame
    void Update()
    {
        //라인 연결 코드
        if (lineActive)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            if (linkObjectTransform == null)
                lineRenderer.SetPosition(1, mousePos);
            else
                lineRenderer.SetPosition(1, linkObjectTransform.position);
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
}
