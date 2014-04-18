using UnityEngine;
using System.Collections;

public class GroundCheck : MonoBehaviour {
	public float distanceDownToCheck; // The distance below to check for ground
	private float circleColliderRadius;
	private bool grounded;
	private int groundLayer;

	private RaycastHit2D[] theGround = new RaycastHit2D[1];

	void Awake()
	{
		this.circleColliderRadius = GetComponent<CircleCollider2D>().radius;
		this.groundLayer = 1 << LayerMask.NameToLayer("Ground");
	}

	void FixedUpdate()
	{
		Vector3 bottomOfCollider = transform.position - new Vector3(0f, circleColliderRadius + distanceDownToCheck, 0f);
		// Cast a line and check if it collides with ground
		int numGround = Physics2D.LinecastNonAlloc (transform.position, bottomOfCollider, theGround, groundLayer);
		// If we collided with > 0 "Ground" objects, then we're grounded!
		this.grounded = (numGround > 0);

	}

	public bool IsGrounded()
	{
		return this.grounded;
	}

}