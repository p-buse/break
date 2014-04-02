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
		// Change us to red initially
		SwitchPlayer();
	}

	CapturedInput CaptureInput()
	{
		// Capture our current input
		bool leftKey = (Input.GetAxis ("Horizontal") < 0);
		bool rightKey = (Input.GetAxis ("Horizontal") > 0);
		bool jumpKey = Input.GetButtonDown ("Jump");
		bool actionKey = Input.GetButtonDown("Action");
		return new CapturedInput(leftKey,rightKey,jumpKey,actionKey);
	}

	void Update () {
		// Switch players, if pressing the switch button
		if (Input.GetButton("SwitchPlayer") && Time.time > nextSwitchTime)
		{
			nextSwitchTime = Time.time + this.switchCooldown;
			SwitchPlayer ();
		}

		// Capture our current input
		CapturedInput currentInput = this.CaptureInput();
		// Send our captured input to the current player
		currentPlayer.ReceiveInput(currentInput);
	}

	public void ReceiveInput(CapturedInput capturedInput)
	{

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

		// Start as the red player
		if (currentPlayer == null)
			currentPlayer = redPlayer;

		guiText.text = "Player: " + currentPlayer.playerName;
		guiText.color = currentPlayer.GetPlayerColor();
	}

	public PlayerController CurrentPlayer()
	{
		return this.currentPlayer;
	}
}
