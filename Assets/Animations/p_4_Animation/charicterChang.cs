using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;

public class charicterChang : MonoBehaviour
{ Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim=GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
      
        
    }

    public void Big() => Input.GetKeyDown(KeyCode.Escape);
  

    public void Small() => Input.GetKeyUp(KeyCode.Escape);
   
    public void Fire()
    {

    }

}
