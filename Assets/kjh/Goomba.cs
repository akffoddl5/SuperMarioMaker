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
    }

    // Update is called once per frame
   protected override void  Update()
    {
        base.Update();
        TouchDown();

    }

    private void TouchDown()
    {

       
        if (!IsSkyDetected())
        {
            Move(true);
        }
        else
        {
            Move(false);
            Destroy(gameObject, 1);
        }
    }
}
