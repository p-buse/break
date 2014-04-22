using UnityEngine;
using System.Collections;

public class GroundCheck : MonoBehaviour {
	public float distanceDownToCheck; // The distance below to check for ground
	private float distanceToBottom;
	private bool grounded;
	private int groundAndDefaultLayerMask;
	float bottomOfColliderY;
	float leftOfColliderX;
	float rightOfColliderX;

	private RaycastHit2D[] theGround = new RaycastHit2D[1];

	void Awake()
	{
		BoxCollider2D boxCol = GetComponent<BoxCollider2D>();
		this.bottomOfColliderY = boxCol.center.y - boxCol.size.y / 2;
		this.leftOfColliderX = boxCol.center.x - boxCol.size.x / 2;
		this.rightOfColliderX = boxCol.center.x + boxCol.size.x / 2;
		this.distanceToBottom = Mathf.Abs (boxCol.center.y - boxCol.size.y / 2);

		this.groundAndDefaultLayerMask = 1 << LayerMask.NameToLayer("Ground") |
			1 << LayerMask.NameToLayer("Default");
	}

	void FixedUpdate()
	{
		Vector3 bottomLeft = transform.position + new Vector3(leftOfColliderX, bottomOfColliderY - distanceDownToCheck, 0f);
		Vector3 bottomRight = transform.position + new Vector3(rightOfColliderX, bottomOfColliderY - distanceDownToCheck, 0f);
		Debug.DrawLine(bottomLeft, bottomRight);
		// Cast a line and check if it collides with ground
		int numGround = Physics2D.LinecastNonAlloc (bottomLeft, bottomRight, theGround);
		// If we collided with > 0 "Ground" objects, then we're grounded!
		this.grounded = (numGround > 0);

	}

	public bool IsGrounded()
	{
		return this.grounded;
	}

}