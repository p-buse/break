﻿using UnityEngine;
using System.Collections;

public class GameControllerScript : MonoBehaviour {

	public PlayerController redPlayer;
	public PlayerController greenPlayer;
	public PlayerController bluePlayer;
	private PlayerController currentPlayer;

	private float nextSwitchTime = 0f;
	private float switchCooldown = 0.25f;

	
	void Start () {
		// Change us to red initially
		SwitchPlayer();
	}

	void Update () {

		/*
		 * ROUTE INPUT *
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
		// Stop movement of current player
		if (currentPlayer != null)
			currentPlayer.MoveHorizontal(0f);

		// Cycle through the three different players
		if (currentPlayer == redPlayer)
			currentPlayer = greenPlayer;
		else if (currentPlayer == greenPlayer)
			currentPlayer = bluePlayer;
		else
			currentPlayer = redPlayer;

		// Start as the red player
		if (currentPlayer == null)
			currentPlayer = redPlayer;

		guiText.text = "Player: " + currentPlayer.playerName;
		guiText.color = currentPlayer.GetPlayerColor();


		
	}
}
