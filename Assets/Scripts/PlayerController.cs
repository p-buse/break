using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour,IReset {
	public float jumpForce; // Our jump force
	public float moveSpeed; // Our horizontal movement speed
	public string playerName; // Our player's name
	private Color playerColor; // Our player's color (set in the Sprite)

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
	
	void Awake()
	{
		// Find our game controller
		this.gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameControllerScript>();
		int levelCompletionTime = gameController.levelCompletionTime;
		// Allocate enough space in the array for recorded input
		this.recordedInput = new CapturedInput[levelCompletionTime];
		this.groundCheck = transform.Find("Ground Check").GetComponent<GroundCheck>();
		this.playerColor = GetComponent<SpriteRenderer>().color;
		this.originalPosition = transform.position;
		this.currentInput = new CapturedInput();
		this.activatorsList = new Hashtable();
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
		int currentPositionInLoop = gameController.GetCurrentPositionInLoop();
		if (this.currentInput.isEmpty())
		{
			if (recordedInput[currentPositionInLoop] != null)
			{
				// If we have no input from the player AND we have recorded stuff, use that!
				this.ActUsingInput((CapturedInput)recordedInput[currentPositionInLoop]);
			}
		}
		else
		{
			recordedInput[currentPositionInLoop] = currentInput;
			this.ActUsingInput(currentInput);
		}
//		ActUsingInput(currentInput);
	}

	public Color GetPlayerColor()
	{
		return this.playerColor;
	}

//	public void SetInput(CapturedInput capturedInput)
//	{
//		// Process horizontal movement
//		if (capturedInput.getLeft ())
//		{
//			this.horizontalMovement = -moveSpeed;
//		}
//		else if (capturedInput.getRight())
//		{
//			this.horizontalMovement = moveSpeed;
//		}
//		else
//			this.horizontalMovement = 0f;
//
//		// Process whether we're jumping
//		if (capturedInput.getJump() && groundCheck.IsGrounded())
//			this.jump = true;
//
//		// Process whether we're activating
//		if (capturedInput.getAction())
//		{
//			foreach (IActivator activator in activatorsList.Values)
//				activator.Activate();
//		}
//	}

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
			foreach (IActivator activator in activatorsList.Values)
				activator.Activate();
		}
	}

	public void SetInput(CapturedInput capturedInput)
	{
		this.currentInput = capturedInput;
	}

	public void Reset()
	{
		this.transform.position = this.originalPosition;
		activatorsList = new Hashtable(); // Clear the activators list
	}
}
