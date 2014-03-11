using UnityEngine;
using System.Collections;

public class GameControllerScript : MonoBehaviour {

	public PlayerController redPlayer;
	public PlayerController greenPlayer;
	public PlayerController bluePlayer;
	private PlayerController currentPlayer;
	private float nextSwitchTime = 0f;
	private float switchCooldown = 0.25f;

	
	void Start () {
		// Sets the current player to Red
		SwitchPlayer ();
	
	}

	void Update () {

		/*
		 * INPUT *
		 */

		if (Input.GetButton("SwitchPlayer") && Time.time > nextSwitchTime)
		{
			nextSwitchTime = Time.time + this.switchCooldown;
			SwitchPlayer ();
		}

		// Get our horizontal input and send it to the current player
		float horizontalMovement = Input.GetAxis ("Horizontal");
		currentPlayer.MoveHorizontal(horizontalMovement);

		if (Input.GetButtonDown("Jump"))
		{
			currentPlayer.Jump ();
		}

		if (Input.GetButtonDown ("Action"))
		{
			currentPlayer.Activate();
			
		}
	}

	void SwitchPlayer()
	{
		// Cycle through the three different players
		if (currentPlayer == redPlayer)
			currentPlayer = greenPlayer;
		else if (currentPlayer == greenPlayer)
			currentPlayer = bluePlayer;
		else
			currentPlayer = redPlayer;
	
		// For initializing
		if (currentPlayer == null)
			currentPlayer = redPlayer;

		guiText.text = "Player: " + currentPlayer.playerName;
		guiText.color = currentPlayer.GetPlayerColor();

		// Stop movement of current player
		currentPlayer.MoveHorizontal(0f);
		
	}
}
