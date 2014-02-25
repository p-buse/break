using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
	
	
	public PlayerCharacteristics redPlayer;
	public PlayerCharacteristics greenPlayer;
	public PlayerCharacteristics bluePlayer;
	private PlayerCharacteristics currentPlayer; // Reference to the currently selected player
	private float jumpForce; // Our current jump force
	private float moveSpeed; // Our current move speed
	
	public float switchCooldown = .25f; // Cooldown between switches
	private float nextSwitchTime = 0f; // Time at which we can switch characters next
	
	private Transform groundCheckLeft; // A position to check if we're grounded on the left
	private Transform groundCheckRight; // A position to check if we're grounded on the right
	
	private bool grounded; // Check for whether we're on the ground
	private bool jump; // Are we jumping?
	private bool onMover; // Are we riding something?
	private IMover mover;

	private Hashtable activatorsList; // Mechanical things you are touching. Key: Instance ID of the thing. Value: The Activator

	
	
	
	
	
	
	
	void Awake()
	{
		groundCheckLeft = transform.Find("Ground Check Left");
		groundCheckRight = transform.Find("Ground Check Right");
		SwitchPlayer();
		activatorsList = new Hashtable();
		
	}
	
	void SwitchPlayer()
	{
		// Initial setup
		if (currentPlayer == null)
			currentPlayer = bluePlayer;
		
		// Cycle through the three different players
		if (currentPlayer == redPlayer)
			currentPlayer = greenPlayer;
		else if (currentPlayer == greenPlayer)
			currentPlayer = bluePlayer;
		else
			currentPlayer = redPlayer;
		
		// Update our local values
		this.jumpForce = currentPlayer.jumpForce;
		this.moveSpeed = currentPlayer.moveSpeed;
		this.gameObject.renderer.material = currentPlayer.playerMaterial;
		
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
	
	
	void Update ()
	{
		// Check if we are grounded on the left OR on the right
		this.grounded = Physics2D.Linecast (transform.position, groundCheckLeft.position, 1 << LayerMask.NameToLayer ("Ground"))
			|| Physics2D.Linecast (transform.position, groundCheckRight.position, 1 << LayerMask.NameToLayer("Ground"));
		
		if (Input.GetButtonDown ("Jump") && this.grounded)
		{
			this.jump = true; // We jump!
			this.grounded = false;
		}
		
		if (Input.GetButton("SwitchPlayer") && Time.time > nextSwitchTime)
		{
			nextSwitchTime = Time.time + this.switchCooldown;
			SwitchPlayer ();
		}

		if (Input.GetButtonDown ("Action"))
		{
			// Activate anything we're currently touching
			foreach (IActivator activator in activatorsList.Values)
			{
				activator.Activate();
			}
		}
	}

	void OnColliderEnter2D(Collision2D collision)
	{
		if (collision.gameObject.tag == "Moving")
		{
			this.onMover = true;
			mover = (IMover) collision.gameObject.GetComponent(typeof(IMover));
		}
	}
	
	void FixedUpdate()
	{
		// Make us move horizontally
		float inputHorizontal = Input.GetAxis ("Horizontal");
		inputHorizontal *= moveSpeed;
		rigidbody2D.velocity = new Vector2(inputHorizontal, rigidbody2D.velocity.y);
		
		if (this.jump)
		{
			rigidbody2D.AddForce(new Vector2(0f, jumpForce));
			this.jump = false;
		}

		if (this.onMover)
		{
			transform.Translate (mover.Movement ());
		}
	}
}
