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

	private Hashtable mechanicalThingsYouAreTouching; // Mechanical things you are touching
	
	
	
	
	
	
	
	void Awake()
	{
		groundCheckLeft = transform.Find("Ground Check Left");
		groundCheckRight = transform.Find("Ground Check Right");
		SwitchPlayer();
		
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

	void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.tag == "Mechanical")
		{
			// Check whether this contains an activator component
			IActivator activator = (IActivator) collision.gameObject.GetComponent(typeof(IActivator));
			if (activator != null)
			{
				// And add it to the list of things you are touching
				mechanicalThingsYouAreTouching.Add(collision.gameObject,activator);
			}

		}
	}
	
	
	void Update ()
	{
		// Check if we are grounded on the left OR on the right
		this.grounded = Physics2D.Linecast (transform.position, groundCheckLeft.position, 1 << LayerMask.NameToLayer ("Ground"))
			|| Physics2D.Linecast (transform.position, groundCheckRight.position, 1 << LayerMask.NameToLayer("Ground"));
		
		if (Input.GetButton ("Jump") && this.grounded)
		{
			this.jump = true; // We jump!
			this.grounded = false;
		}
		
		if (Input.GetButton("SwitchPlayer") && Time.time > nextSwitchTime)
		{
			nextSwitchTime = Time.time + this.switchCooldown;
			SwitchPlayer ();
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
	}
}
