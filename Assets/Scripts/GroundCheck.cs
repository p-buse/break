using UnityEngine;
using System.Collections;

public class GroundCheck : MonoBehaviour {
	private float distanceDownToCheck; // The distance below to check for ground

	private bool hasJumped;
	private bool grounded;
	private int groundAndDefaultLayerMask;

	private IMover mover;

	private float bottomOfColliderY;
	private float leftOfColliderX;
	private float rightOfColliderX;

	private RaycastHit2D[] theGround = new RaycastHit2D[2];

	void Awake()
	{
		this.hasJumped = false;
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
		// If we collided with > 0 "Ground" objects, then we might be grounded!
		if (numGround > 0)
		{
			// Quick fix to avoid the player in the array preventing the platform from being detected
			if (theGround[0].collider.gameObject == this.gameObject && theGround[1])
			{
				theGround[0] = theGround[1];
			}
			this.grounded = (theGround[0].collider.gameObject != this.gameObject && !this.hasJumped);
		}
		else
			this.grounded = false;



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
		theGround = new RaycastHit2D[2];
		numGround = 0;
		
	}

	public bool IsGrounded()
	{
		return this.grounded;
	}

	/// <summary>
	/// Sets whether we have jumped
	/// </summary>
	/// <param name="hasJumped">If set to <c>true</c>, we're jumping!.</param>
	public void SetJumped(bool hasJumped)
	{
		this.hasJumped = hasJumped;
	}

	/// <summary>
	/// Gets the layer mask of layers that allow players to jump from.
	/// </summary>
	/// <returns>The jumpable layer mask.</returns>
	public int GetJumpableLayerMask()
	{
		return this.groundAndDefaultLayerMask;
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