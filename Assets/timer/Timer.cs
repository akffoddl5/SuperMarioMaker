using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public float timeSet = 100;//������ �ð���
    public int min = 0;
    public float sec = 0;
   public float sec2 = 0;
    int _S = 0;
    int _M = 0;
   
   
   Text timetextM;
   Text timetextS;




    void Start()
    {//�ð� ����(�ʱ�ȭ����)
        timetextM=GameObject.Find("Min").GetComponent<Text>();
        timetextS=GameObject.Find("Sec").GetComponent<Text>();
      
        if (timeSet / 60 > 0)
        {
            if (timeSet > 60)
            {
               
                min = (int)timeSet / 60;


            }
        }
        if (timeSet %60 < 60)
        {
            sec = (int)timeSet %60;

        }

        

    }
   void Update()
    {//���ڸ�������
        timetextM.text = string.Format("{0:d2}",min);
       // timetextS.text = string.Format("{0:d2}",sec);
        timetextS.text = string.Format("{0:00.00}",sec);
        
    }
    void FixedUpdate()
    {
        if (timeSet > 0)//�ð��� +�ϋ� �帧
        {
            timeSet -= Time.deltaTime;

            if (timeSet % 60 <1)
            {
                
                
                    min = ((int)timeSet-1)/60;//���������� �ð���60������ ������������ -1����
               
            }
            if(timeSet<60)
            {
                min = 0;
            }
            //if (timeSet % 60 < 60)
            //{

            //    sec = (int)timeSet % 60;

            //}

            if (timeSet % 60 < 60)
            {
                sec = (int)timeSet % 60;
              sec2=timeSet%60- (int)timeSet % 60;

                sec = sec + sec2;
            }




        }




        else//�ð��� -�ϋ� 0���� �ʱ�ȭ
        {
            min = 0;
            sec = 0;
        }




    }

}
