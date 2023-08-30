using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarColor : MonoBehaviour
{
    SpriteRenderer spriteRenderer;

    public bool isStar = false;
    bool trigger = true;
    Color[] color = {
        Color.white,
        Color.red,
        Color.yellow,
        Color.green,
        Color.blue,
        Color.cyan,
        Color.magenta
    };
    int index = 0;
    public float sec = 0.2f;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isStar && trigger)
        {
            trigger = false;
            Invoke("ColorChange", sec);
        }
        else if (!isStar)
        {
            trigger = true;
            spriteRenderer.color = Color.white;
        }
    }

    void ColorChange()
    {
        if (!isStar)
            return;

        spriteRenderer.color = color[index++];

        if (index >= color.Length)
            index = 0;

        Invoke("ColorChange", sec);
    }
}
