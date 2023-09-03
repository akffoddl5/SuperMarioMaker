using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_mushroom : Item
{
	public float moveSpeed=3f;
	
	float moveflip = 1;
    //new string prefabPath = "Prefabs/Item_mushroom";

	[SerializeField] private Transform wallLCheck;
	[SerializeField] private Transform wallRCheck;
	[SerializeField] private float wallCheckDistance;
	[SerializeField] private LayerMask whatIsGround;

	protected override void Awake()
	{
		base.Awake();
		
	}

	void Start()
    {

	}

    // Update is called once per frame
    void Update()
	{
		Move();
	}

	private void Move()
	{
		if (base.isSpawn)
		{
			rb.velocity = new Vector2(moveSpeed * moveflip, rb.velocity.y);

			// if wall collision, transform Rotate
			if (IsWallLDetected() || IsWallRDetected())
			{
				moveflip = -moveflip;
				transform.Rotate(0, 180, 0);
			}
		}
	}

	public bool IsWallLDetected() => Physics2D.Raycast(wallLCheck.position, Vector2.right * moveflip, wallCheckDistance, whatIsGround);
	public bool IsWallRDetected() => Physics2D.Raycast(wallRCheck.position, Vector2.right * moveflip, wallCheckDistance, whatIsGround);

	private void OnDrawGizmos()
	{
		Gizmos.DrawLine(wallLCheck.position, new Vector3(wallLCheck.position.x - wallCheckDistance,
			wallLCheck.position.y));

		Gizmos.DrawLine(wallRCheck.position, new Vector3(wallRCheck.position.x + wallCheckDistance,
		   wallRCheck.position.y));
	}

	public override string Get_Prefab_Path()
    {
		return "Prefabs/Item_mushroom";
	}
}
