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
	
	private GroundCheck groundCheck; // Ground check script
	private bool jump; // Are we jumping?

	// Riding a mover
	private bool onMover; // Are we riding something?
	private IMover mover; // The Mover script of the mover we're riding
	private int moverID = -1; // ID of the current Mover we're riding

	private Hashtable activatorsList; // Mechanical things you are touching. Key: Instance ID of the thing. Value: The Activator

	
	
	
	
	
	
	
	void Awake()
	{
		groundCheck = transform.Find("Ground Check").GetComponent<GroundCheck>();
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
		bool grounded = groundCheck.isGrounded();

		if (Input.GetButtonDown ("Jump") && grounded)
		{
			this.jump = true; // We jump!
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

	

	void OnCollisionEnter2D(Collision2D collision)
	{
		Debug.Log ("COLLIDED!");
		if (collision.gameObject.tag == "Moving" && this.grounded)
		{
			this.onMover = true;
			mover = (IMover) collision.gameObject.GetComponent(typeof(IMover));
			moverID = collision.gameObject.GetInstanceID();
			Debug.Log ("current mover: " + moverID);
		}
	}

	void OnCollisionExit2D(Collision2D collision)
	{
		Debug.Log ("exiting ID " + collision.gameObject.GetInstanceID());
		if (collision.gameObject.tag == "Moving" && collision.gameObject.GetInstanceID() == this.moverID)
		{
			Debug.Log ("no longer moving");
			this.onMover = false;
			mover = null;
			moverID = -1;
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
