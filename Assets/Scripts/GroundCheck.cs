using UnityEngine;
using System.Collections;

public class GroundCheck : MonoBehaviour {
	public float distanceDownToCheck; // The distance below to check for ground
	private float colliderRadius;
	private bool grounded;
	private bool onMover;
	private int groundLayer;

	private RaycastHit2D[] theGround = new RaycastHit2D[1];
	private IMover mover; // The mover script

	void Awake()
	{
		this.colliderRadius = GetComponent<CircleCollider2D>().radius;
		this.groundLayer = 1 << LayerMask.NameToLayer("Ground");
	}

	void Update()
	{
		Vector3 bottomOfCollider = transform.position - new Vector3(0f, colliderRadius + distanceDownToCheck, 0f);
		// Cast a line and check if it collides with ground
		int numGround = Physics2D.LinecastNonAlloc (transform.position, bottomOfCollider, theGround, groundLayer);
		Debug.DrawLine (transform.position,bottomOfCollider,Color.red);
		// If we collided with > 0 "Ground" objects, then we're grounded!
		this.grounded = (numGround > 0);


		// CHECK FOR MOVER

		if (this.grounded)
		{
			Collider2D collider = theGround [0].collider;
			if (collider.gameObject.tag == "Moving")
			{
				this.onMover = true;
				mover = (IMover)collider.gameObject.GetComponent (typeof(IMover));
			}
		}
		else
		{
			this.onMover = false;
			mover = null;
		}
	}

	public bool IsGrounded()
	{
		return this.grounded;
	}

	public bool IsOnMover()
	{
		return this.onMover;
	}
	public Vector3 GetMovement()
	{
		return this.mover.Movement();
	}
}