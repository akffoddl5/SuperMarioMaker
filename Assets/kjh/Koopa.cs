using System.Collections;
using System.Collections.Generic;
using System.Text;
using Unity.VisualScripting;
using UnityEngine;

public class Koopa : Enemy
{
    // public GameObject coopadie;



    //protected override void Start()
    //{
    //    base.Start();



    //}


    //protected override void Update()
    //{

    //    if (!IsGroundDetected() && overShell == true)

    //    {
    //        if (overShellSpeed == 0 && IsPlayerRDetected())
    //        {
    //            Move(false);
    //            moveflip = -1;
    //            overShellSpeed = 10;
    //            rb.velocity = new Vector2(overShellSpeed * moveflip, spdY);


    //        }
    //        else if (overShellSpeed == 0 && IsPlayerLDetected())
    //        {
    //            Move(false);
    //            moveflip = 1;
    //            overShellSpeed = 10;
    //            rb.velocity = new Vector2(overShellSpeed * moveflip, spdY);


    //        }

    //        if (IsGroundDetected())
    //        {
    //            overShell = false;
    //        }


    //        if (IswallDetected())
    //        {
    //            overShell = false;
    //        }
            
    //    }  
    //    else if (overShell == false)
    //    {


            
    //        base.Update();
    //        TouchDown();

    //    }


    //}

    //private void TouchDown()
    //{


    //    if (!IsSkyDetected())
    //    {
    //        Move(true);



    //    }
    //    else if (IsSkyDetected())//거북이일때
    //    {
    //        Move(false);
    //        overShellSpeed = 0;

    //        //StartCoroutine(Rekoopa());

    //    }





    //    if (overShellSpeed == 0 && IsPlayerRDetected())
    //    {
    //        moveflip = -1;
    //        overShellSpeed = 10;
    //        overShell = true;
    //        Move(false);
    //    }
    //    else if (overShellSpeed == 0 && IsPlayerLDetected())
    //    {

    //        moveflip = 1;
    //        overShellSpeed = 10;
    //        overShell = true;
    //        Move(false);

    //    }






    //}
   
    //private IEnumerator Rekoopa()//플레이어가 밟고 시간이 지남
    //{

        


    //    yield return new WaitForSeconds(5f);
    //    if (overShellSpeed == 10)//거북이 걷고있음
    //    {
    //        Move(true);
    //        overShellSpeed = 1;
    //        anim.SetBool("Flat", false);
    //        overShell = false;
    //        yield break;
    //    }
    //    Move(true);//껍질상태
    //    overShellSpeed = 1;
    //    anim.SetBool("Flat", false);
    //    overShell = false;


    //}
}
