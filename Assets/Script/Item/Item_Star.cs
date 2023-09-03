using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Star : Item
{
    public float moveSpeed = 4f;
    public float jumpPower = 10f;
	
	bool isStarJump =false;
	float moveflip = 1;
	float jumpStartTime = 1f;
	[SerializeField] private Transform groundCheck;
	[SerializeField] private float groundCheckDistance;
	[SerializeField] private Transform wallLCheck;
	[SerializeField] private Transform wallRCheck;
	[SerializeField] private float wallCheckDistance;
	[SerializeField] private LayerMask whatIsGround;

	//new string prefabPath = "Prefabs/Item_Star";
    


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
		if (isSpawn && !isStarJump)
		{
			StartCoroutine(starJump());
			isStarJump = true;
		}

		Move();
	}

	protected virtual void Move()
	{
		if (isSpawn)
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

	IEnumerator starJump()
	{
		yield return new WaitForSeconds(jumpStartTime);
		while (true)
		{
			if (IsGroundDetected())
			{
				rb.velocity = new Vector2(rb.velocity.x, 0);
				rb.AddForce(new Vector2(0, jumpPower), ForceMode2D.Impulse);
				yield return new WaitForSeconds(0.2f);
			}
			yield return new WaitForEndOfFrame();
		}
	}

	public bool IsGroundDetected() => Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);
	public bool IsWallLDetected() => Physics2D.Raycast(wallLCheck.position, Vector2.right * moveflip, wallCheckDistance, whatIsGround);
	public bool IsWallRDetected() => Physics2D.Raycast(wallRCheck.position, Vector2.right * moveflip, wallCheckDistance, whatIsGround);

	private void OnDrawGizmos()
	{
		Gizmos.DrawLine(groundCheck.position, new Vector3(groundCheck.position.x,
			groundCheck.position.y - groundCheckDistance));

		Gizmos.DrawLine(wallLCheck.position, new Vector3(wallLCheck.position.x - wallCheckDistance,
			wallLCheck.position.y));

		Gizmos.DrawLine(wallRCheck.position, new Vector3(wallRCheck.position.x + wallCheckDistance,
		   wallRCheck.position.y));
	}

    public override string Get_Prefab_Path()
    {
		return "Prefabs/Item_Star";
	}
}
