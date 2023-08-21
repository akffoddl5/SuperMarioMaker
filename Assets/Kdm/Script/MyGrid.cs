using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyGrid : MonoBehaviour
{
    LineRenderer lineRenderer;

    [Header("Info")]
    [SerializeField] Vector2 startPos;
    [SerializeField] int xCount;
    [SerializeField] int yCount;
    [SerializeField] float Width;
    [SerializeField] float height;

    
    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void DrawGrid()
    {

    }

}
