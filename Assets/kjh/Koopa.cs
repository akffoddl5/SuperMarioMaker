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
        else if (IsSkyDetected())//�ź����϶�
        {
            Move(true);
            overShellSpeed = 0;
           
            StartCoroutine(Rekoopa());
          
        }


        //if (overShell == true && !IsGroundDetected())//Ż����
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
    private void dieplay()//�÷��̾ ��� �ð��� ����
    {

     
        
            
            Destroy(gameObject);
            Instantiate(coopadie,new Vector3(posX,posY,0),Quaternion.identity);

        coopadie.GetComponent<CapsuleCollider2D>().isTrigger = true;
            



    }
    private IEnumerator Rekoopa()//�÷��̾ ��� �ð��� ����
    {
       



           yield return new WaitForSeconds(5f);
            Move(true);
            overShellSpeed = 1;
            anim.SetBool("Flat", false);
            overShell = false;
          

        
    }
}
