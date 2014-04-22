using UnityEngine;
using System.Collections;

public class GroundCheck : MonoBehaviour {
	public float distanceDownToCheck; // The distance below to check for ground
	private float distanceToBottom;
	private bool grounded;
	private int groundLayer;

	private RaycastHit2D[] theGround = new RaycastHit2D[1];

	void Awake()
	{
		BoxCollider2D boxCol = GetComponent<BoxCollider2D>();
		this.distanceToBottom = Mathf.Abs (boxCol.center.y - boxCol.size.y / 2);
		this.groundLayer = 1 << LayerMask.NameToLayer("Ground");
	}

	void FixedUpdate()
	{
		Vector3 bottomOfCollider = transform.position - new Vector3(0f, distanceToBottom + distanceDownToCheck, 0f);
		Debug.DrawLine(transform.position, bottomOfCollider);
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