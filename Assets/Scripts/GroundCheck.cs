using UnityEngine;
using System.Collections;

public class GroundCheck : MonoBehaviour {
	public float distanceDownToCheck; // The distance below to check for ground
	private float colliderRadius;
	private bool grounded;
	private RaycastHit2D[] theGround = new RaycastHit2D[1];

	void Awake()
	{
		this.colliderRadius = GetComponent<CircleCollider2D>().radius;
	}

	void Update()
	{
		Vector3 bottomOfCollider = transform.position - new Vector3(0f, colliderRadius + distanceDownToCheck, 0f);

		// Get the layer ID of the ground
		int groundLayer = 1 << LayerMask.NameToLayer("Ground");
		// Cast a line and check if it collides with ground
		int numGround = Physics2D.LinecastNonAlloc (transform.position, bottomOfCollider, theGround, groundLayer);
		// If we collided with > 0 "Ground" objects, then we're grounded!
		this.grounded = (numGround > 0);
	}

	public bool isGrounded()
	{
		return this.grounded;
	}
}