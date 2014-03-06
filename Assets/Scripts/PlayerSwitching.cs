//using UnityEngine;
//
//public class PlayerSwitching : MonoBehaviour {
//
//	public float switchCooldown = .25f; // Time between switches
//	public PlayerCharacteristics redPlayer;
//	public PlayerCharacteristics greenPlayer;
//	public PlayerCharacteristics bluePlayer;
//	private PlayerCharacteristics currentPlayer; // Reference to the currently selected player
//	private SpriteRenderer spriteRenderer; // Used to set our current color
//	private GUIText selectedPlayerGUI; // Used to display GUI of who's playing
//	
//	
//	void Awake()
//	{
//
//		spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
//		if (spriteRenderer == null)
//			Debug.LogError("No sprite renderer on the player object!");
//		selectedPlayerGUI = GameObject.FindWithTag("GameController").GetComponent<GUIText>();
//		if (selectedPlayerGUI == null)
//			Debug.LogError ("Player couldn't find GUIText to write to!");
//		SwitchPlayer (); // Will set player initially to Red
//	}
//
//	
//	void SwitchPlayer()
//	{
//		// Cycle through the three different players
//		if (currentPlayer == redPlayer)
//			currentPlayer = greenPlayer;
//		else if (currentPlayer == greenPlayer)
//			currentPlayer = bluePlayer;
//		else
//			currentPlayer = redPlayer;
//
//		// For beginning, when player is null
//		if (currentPlayer == null)
//			currentPlayer = redPlayer;
//		// Update our local values
//		this.jumpForce = currentPlayer.jumpForce;
//		this.moveSpeed = currentPlayer.moveSpeed;
//		spriteRenderer.color = currentPlayer.playerColor;
//		selectedPlayerGUI.text = "Player: " + currentPlayer.playerName;
//		selectedPlayerGUI.color = currentPlayer.playerColor;
//		
//	}
//
//	
//	void Update ()
//	{
//
//		if (Input.GetButton("SwitchPlayer") && Time.time > nextSwitchTime)
//		{
//			nextSwitchTime = Time.time + this.switchCooldown;
//			SwitchPlayer ();
//		}
//
//	}
//
//}
