using UnityEngine;
using System.Collections;

public class GroundCheck : MonoBehaviour {
	private float distanceDownToCheck; // The distance below to check for ground

	bool grounded;
	private int groundAndDefaultLayerMask;

	private IMover mover;

	private float bottomOfColliderY;
	private float leftOfColliderX;
	private float rightOfColliderX;

	private RaycastHit2D[] theGround = new RaycastHit2D[1];

	void Awake()
	{
		this.distanceDownToCheck = 0.05f;
		BoxCollider2D boxCol = GetComponent<BoxCollider2D>();
		this.bottomOfColliderY = boxCol.center.y - boxCol.size.y / 2;
		this.leftOfColliderX = boxCol.center.x - boxCol.size.x / 2;
		this.rightOfColliderX = boxCol.center.x + boxCol.size.x / 2;

		// Create the mask that lets us search for ground and other players below us
		this.groundAndDefaultLayerMask = 1 << LayerMask.NameToLayer("Ground") | 
			1 << LayerMask.NameToLayer("Default");

		this.mover = null;
	}


	/// <summary>
	/// Checks if we're on the ground and if we're on a mover.
	/// </summary>
	public void FixedUpdate()
	{
		Vector3 bottomLeft = transform.position + new Vector3(leftOfColliderX, bottomOfColliderY - distanceDownToCheck, 0f);
		Vector3 bottomRight = transform.position + new Vector3(rightOfColliderX, bottomOfColliderY - distanceDownToCheck, 0f);
		// Cast a line and check if it collides with ground
		int numGround = Physics2D.LinecastNonAlloc (bottomLeft, bottomRight, theGround,	groundAndDefaultLayerMask);
		Debug.DrawLine (bottomLeft,bottomRight);
		// If we collided with > 0 "Ground" objects, then we're grounded!
		this.grounded = (numGround > 0 && theGround[0].collider.gameObject != this.gameObject);

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

	/// <summary>
	/// Sets whether we are grounded.
	/// </summary>
	/// <param name="newGroundedState">If set to <c>true</c>, we're grounded.</param>
	public void SetGrounded(bool newGroundedState)
	{
		this.grounded = newGroundedState;
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