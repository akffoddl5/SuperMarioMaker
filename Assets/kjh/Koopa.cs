using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Koopa : Enemy
{
    public  GameObject coopadie;
   
    protected override void Start()
    {
        base.Start();
       
      

    }

    
    protected override void Update()
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
        else if (IsSkyDetected())//거북이일때
        {
            Move(true);
            overShellSpeed = 0;
           
            StartCoroutine(Rekoopa());
          
        }


        //if (overShell == true && !IsGroundDetected())//탈락됨
        //{
        //    return;
        //}



        if (overShellSpeed == 0 && IsPlayerRDetected())
        {
            Move(false);
            overShellSpeed = -10;
            rb.velocity = new Vector2(overShellSpeed, 0);
            overShell = true;
            dieplay();
        }
        else if (overShellSpeed == 0 && IsPlayerLDetected())
        {

            Move(false);
            overShellSpeed = +10;
            rb.velocity = new Vector2(overShellSpeed, 0);
            overShell = true;

            dieplay();
        }


    }
    private void dieplay()//플레이어가 밟고 시간이 지남
    {

     
        
            
            Destroy(gameObject);
            Instantiate(coopadie,new Vector3(posX,posY,0),Quaternion.identity);

        coopadie.GetComponent<CapsuleCollider2D>().isTrigger = true;
            



    }
    private IEnumerator Rekoopa()//플레이어가 밟고 시간이 지남
    {
       



           yield return new WaitForSeconds(5f);
            Move(true);
            overShellSpeed = 1;
            anim.SetBool("Flat", false);
            overShell = false;
          

        
    }
}
