using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour,IReset {
	public float jumpForce; // Our jump force
	public float moveSpeed; // Our horizontal movement speed
	public string playerName; // Our player's name
	private Color playerColor; // Our player's color (set in the Sprite)
	public int activateCooldown = 10; // How long between activations (in 50ths of a second)
	private int activateTimer; // Timer for activations
	private GameControllerScript gameController;
	private GroundCheck groundCheck; // Ground check script
	private bool jump; // Are we jumping?
	private Vector3 originalPosition;
	/// <summary>
	/// The most recent button presses sent to the player
	/// </summary>
	private CapturedInput currentInput;

	/// <summary>
	/// Array for storing recorded input of this character
	/// </summary>
	private CapturedInput[] recordedInput;

	private Hashtable activatorsList; // Mechanical things you are touching. Key: Instance ID of the thing. Value: The Activator
	/// <summary>
	/// If true, will overwrite loop with emptiness as it goes.
	/// </summary>
	private bool overwriteLoop;
	void Awake()
	{
		// Set our activate cooldown
		this.activateTimer = 0;
		// Find our game controller
		this.gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameControllerScript>();
		int levelCompletionTime = GameObject.FindGameObjectWithTag("LevelStats").GetComponent<LevelStats>().levelCompletionTime;
		// Allocate enough space in the array for recorded input
		this.recordedInput = new CapturedInput[levelCompletionTime];
		this.groundCheck = GetComponent<GroundCheck>();
		this.playerColor = GetComponent<SpriteRenderer>().color;
		this.originalPosition = transform.position;
		this.currentInput = new CapturedInput();
		this.activatorsList = new Hashtable();
		this.overwriteLoop = false;
	}

	void OnLevelWasLoaded()
	{
		activatorsList = new Hashtable(); // Clear the activators list
	}

	void OnTriggerEnter2D(Collider2D collider)
	{

		if (collider.gameObject.name.Equals("Exit Door"))
		{
			gameController.GetComponent<AdvanceLevel>().SetComplete(playerName, true);
			gameObject.SetActive(false);
		}
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
		this.activateTimer -= 1;
		int currentPositionInLoop = gameController.GetCurrentPositionInLoop();
		// Our player has not pressed anything
		if (this.currentInput.isEmpty())
		{

			if (this.overwriteLoop)
			{
				// If we're overwriting, record a "no-op" and do nothing
				recordedInput[currentPositionInLoop] = currentInput;
				this.ActUsingInput(currentInput);
			}
			else
			{

				if (recordedInput[currentPositionInLoop] != null)
				{
					// If we have no input from the player AND we have recorded stuff, use that!
					this.ActUsingInput((CapturedInput)recordedInput[currentPositionInLoop]);
				}
				else
				{
					// If we have no input from the player and don't have recorded stuff, stop movement.
					this.ActUsingInput(currentInput);
				}
			}

		}
		// Our player has pressed something
		else
		{
			recordedInput[currentPositionInLoop] = currentInput;
			this.ActUsingInput(currentInput);
			this.overwriteLoop = true;
		}
	}

	public Color GetPlayerColor()
	{
		return this.playerColor;
	}

	private void ActUsingInput(CapturedInput theInput)
	{
		// Process horizontal movement
		float horizontalMovement = 0f;
		if (theInput.getLeft ())
			horizontalMovement = -moveSpeed;
		else if (theInput.getRight())
			horizontalMovement = moveSpeed;
		horizontalMovement *= moveSpeed;
		rigidbody2D.velocity = new Vector2(horizontalMovement,rigidbody2D.velocity.y);
		// Process jumping
		if (theInput.getJump() && groundCheck.IsGrounded())
			rigidbody2D.AddForce(new Vector2(0f, jumpForce));

		// Process activating
		if (theInput.getAction())
		{
			this.Activate ();
		}

		// Add platform movement
		rigidbody2D.velocity += new Vector2(groundCheck.GetMovement().x, 0f);
	}

	private void Activate()
	{

		if (activateTimer <= 0)
		{
			foreach (IActivator activator in activatorsList.Values)
			{
				activator.Activate();
			}
			activateTimer = activateCooldown;
		}
	}

	public void SetInput(CapturedInput capturedInput)
	{
		this.currentInput = capturedInput;
	}

	public void Reset()
	{
		gameObject.SetActive(true);
		this.transform.position = this.originalPosition;
		this.rigidbody2D.velocity = new Vector2();
		this.activatorsList = new Hashtable(); // Clear the activators list
		this.overwriteLoop = false;
	}
}
