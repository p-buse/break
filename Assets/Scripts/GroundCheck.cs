using UnityEngine;
using System.Collections;

public class GroundCheck : MonoBehaviour {
	public float distanceDownToCheck; // The distance below to check for ground
	private float colliderRadius;
	private bool grounded;
	private bool onMover;
	private IMover mover;
	private int moverID;
	private RaycastHit2D[] theGround = new RaycastHit2D[1];

	void Awake()
	{
		this.colliderRadius = GetComponent<CircleCollider2D>().radius;
	}

	void Update()
	{
		Vector3 bottomOfCollider = transform.position - new Vector3(0f, colliderRadius + distanceDownToCheck, 0f);

		// CHECK FOR GROUND
		// Get the layer ID of the ground
		int groundLayer = 1 << LayerMask.NameToLayer("Ground");
		// Cast a line and check if it collides with ground
		int numGround = Physics2D.LinecastNonAlloc (transform.position, bottomOfCollider, theGround, groundLayer);
		// If we collided with > 0 "Ground" objects, then we're grounded!
		this.grounded = (numGround > 0);


		// CHECK FOR MOVER
		Collider2D collider = theGround[0].collider;
		if (collider.gameObject.tag == "Moving")
		{
			this.onMover = true;
			mover = (IMover) collider.gameObject.GetComponent(typeof(IMover));
			moverID = collision.gameObject.GetInstanceID();
			Debug.Log ("current mover: " + moverID);
		}
	}

	public bool isGrounded()
	{
		return this.grounded;
	}

	public bool isOnMover()
	{
		return this.onMover;
	}
}