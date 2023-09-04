using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_flower : Item
{
	protected override void Awake()
	{
		base.Awake();
	}

    public override string Get_Prefab_Path()
    {
        return "Prefabs/Item_flower";
    }
}
