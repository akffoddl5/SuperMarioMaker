using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Koopa : Enemy
{
   public  GameObject shell;
    public Enemy_Koopa()
    {
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();

        Move(true);

      
    }
    private void FixedUpdate()
    {
        if (IsSkyDetected())
        {
            Destroy(gameObject);
            Instantiate(shell, new Vector2(Enemy.spdX, Enemy.spdY), Quaternion.identity);
        }
    }

}
