using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour,IReset {
	public float jumpForce; // Our jump force
	public float moveSpeed; // Our horizontal movement speed
	public string playerName; // Our player's name
	private Color playerColor; // Our player's color (set in the Sprite)

	private GroundCheck groundCheck; // Ground check script
	private bool jump; // Are we jumping?
	private float horizontalMovement = 0f; // Holds our current horizontal movement.
	private Vector3 originalPosition;

	private Hashtable activatorsList; // Mechanical things you are touching. Key: Instance ID of the thing. Value: The Activator
	
	void Awake()
	{
		groundCheck = transform.Find("Ground Check").GetComponent<GroundCheck>();
		activatorsList = new Hashtable();
		this.playerColor = GetComponent<SpriteRenderer>().color;
		this.originalPosition = transform.position;
	}

	void OnLevelWasLoaded()
	{
		activatorsList = new Hashtable(); // Clear the activators list
	}

	void OnTriggerEnter2D(Collider2D collider)
	{
		if (collider.gameObject.tag.Equals ( "Mechanical"))
		{
			// Check whether this contains an activator component
			IActivator activator = (IActivator) collider.gameObject.GetComponent(typeof(IActivator));
			if (activator != null)
			{
				int activatorID = collider.gameObject.GetInstanceID();
				// If it isn't in the table
				if (!activatorsList.ContainsKey(activatorID))
				{
					// Add it to the table
					activatorsList.Add(activatorID,activator);
				}

			}

		}
	}

	void OnTriggerExit2D(Collider2D collider)
	{
		int activatorID = collider.gameObject.GetInstanceID();
		if (activatorsList.ContainsKey (activatorID))
		{
			activatorsList.Remove(activatorID);
		}
	}

	void FixedUpdate()
	{
		// Set the velocity to negative or positive of our movespeed
		rigidbody2D.velocity = new Vector2(this.horizontalMovement * this.moveSpeed, rigidbody2D.velocity.y);
		
		if (this.jump)
		{
			rigidbody2D.AddForce(new Vector2(0f, jumpForce));
			this.jump = false;
		}
		// If we're on a mover
		if (groundCheck.IsOnMover())
		{
			transform.Translate(groundCheck.GetMovement());
		}
		
	}

	/*
	 ************** PUBLIC METHODS ***************
	 */

	public Color GetPlayerColor()
	{
		return this.playerColor;
	}

	public void ReceiveInput(CapturedInput capturedInput)
	{
		// Process horizontal movement
		if (capturedInput.getLeft ())
		{
			this.horizontalMovement = -moveSpeed;
		}
		else if (capturedInput.getRight())
		{
			this.horizontalMovement = moveSpeed;
		}
		else
			this.horizontalMovement = 0f;

		// Process whether we're jumping
		if (capturedInput.getJump() && groundCheck.IsGrounded())
			this.jump = true;

		// Process whether we're activating
		if (capturedInput.getAction())
		{
			foreach (IActivator activator in activatorsList.Values)
				activator.Activate();
		}
	}

	public void Reset()
	{
		this.transform.position = this.originalPosition;
	}
}
