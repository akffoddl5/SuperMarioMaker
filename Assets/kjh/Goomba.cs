using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goomba : Enemy
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        
            move = true;




    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        TouchDown();

    }

    private void TouchDown()
    {

       if (IsSkyDetected())
        {
            
            move = false;

            Destroy(gameObject, 1);
        }
    }
}
