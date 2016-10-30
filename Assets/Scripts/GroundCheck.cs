using UnityEngine;
using System.Collections;

public class GroundCheck : MonoBehaviour {
	public float distanceDownToCheck; // The distance below to check for ground

	private bool grounded;
	private int groundAndDefaultLayerMask;

	private IMover mover;

	private float bottomOfColliderY;
	private float leftOfColliderX;
	private float rightOfColliderX;

	private RaycastHit2D[] theGround = new RaycastHit2D[1];

	void Awake()
	{
		BoxCollider2D boxCol = GetComponent<BoxCollider2D>();
		this.bottomOfColliderY = boxCol.offset.y - boxCol.size.y / 2;
		this.leftOfColliderX = boxCol.offset.x - boxCol.size.x / 2;
		this.rightOfColliderX = boxCol.offset.x + boxCol.size.x / 2;

		// Create the mask that lets us search for ground and other players below us
		this.groundAndDefaultLayerMask = 1 << LayerMask.NameToLayer("Ground") | 
			1 << LayerMask.NameToLayer("Default");

		this.mover = null;
	}

	void FixedUpdate()
	{
		Vector3 bottomLeft = transform.position + new Vector3(leftOfColliderX, bottomOfColliderY - distanceDownToCheck, 0f);
		Vector3 bottomRight = transform.position + new Vector3(rightOfColliderX, bottomOfColliderY - distanceDownToCheck, 0f);
		// Cast a line and check if it collides with ground
		int numGround = Physics2D.LinecastNonAlloc (bottomLeft, bottomRight, theGround,	groundAndDefaultLayerMask);
		// If we collided with > 0 "Ground" objects, then we're grounded!
		this.grounded = (numGround > 0);

		if (this.grounded)
		{
			Collider2D collider = theGround [0].collider;
			if (collider.gameObject.tag == "Moving")
			{
				mover = (IMover)collider.gameObject.GetComponent (typeof(IMover));
			}
			else
			{
				mover =  null;
			}
		}
		else
		{
			mover = null;
		}
		
	}

	public bool IsGrounded()
	{
		return this.grounded;
	}

	public Vector2 GetMovement()
	{
		if (mover != null)
			return mover.Movement();
		else
			return Vector2.zero;
	}

	public bool IsOnMover()
	{
		return (mover != null);
	}


}