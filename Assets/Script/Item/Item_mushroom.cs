using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_mushroom : MonoBehaviour
{
	public float moveSpeed=3f;
	
	float moveflip = 1;

	[SerializeField] private Transform wallLCheck;
	[SerializeField] private Transform wallRCheck;
	[SerializeField] private float wallCheckDistance;
	[SerializeField] private LayerMask whatIsGround;

	Rigidbody2D rb;
	void Start()
    {
		rb = GetComponent<Rigidbody2D>();
	}

    // Update is called once per frame
    void Update()
    {
		rb.velocity = new Vector2(moveSpeed * moveflip, rb.velocity.y);

		// if wall collision, transform Rotate
		if (IsWallLDetected() || IsWallRDetected())
		{
			moveflip = -moveflip;
			transform.Rotate(0, 180, 0);
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

}
