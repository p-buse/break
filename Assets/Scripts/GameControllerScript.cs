using UnityEngine;
using System.Collections;
using BreakGlobals;

public class GameControllerScript : MonoBehaviour {

	public float switchCooldown = 0.25f;
	public PlayerController redPlayer;
	public PlayerController greenPlayer;
	public PlayerController bluePlayer;
	private PlayerController currentPlayer;


	private float nextSwitchTime = 0f;

	public PlayerRecordAndPlayback looper;
	
	void Start () {
		// Change us to red initially
		SwitchPlayer();
	}

	public CapturedInput CaptureInput()
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

		if (Input.GetButtonDown("Record"))
		{
			looper.SwitchState();
		}

		// Capture our current input
		CapturedInput currentInput = this.CaptureInput();
		// Send our captured input to the current player
		currentPlayer.ReceiveInput(currentInput);
	}

	public void ReceiveInputFromLooper(CombinedInput looperInput)
	{
		redPlayer.ReceiveInput(looperInput.GetPlayerInput(PlayerColor.Red));
		greenPlayer.ReceiveInput(looperInput.GetPlayerInput(PlayerColor.Green));
		bluePlayer.ReceiveInput(looperInput.GetPlayerInput(PlayerColor.Blue));
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

	public PlayerColor CurrentPlayerColor()
	{
		if (currentPlayer == redPlayer)
			return PlayerColor.Red;
		if (currentPlayer == greenPlayer)
			return PlayerColor.Green;
		if (currentPlayer == bluePlayer)
			return PlayerColor.Blue;
		Debug.LogError("Current player is not the red, green, or blue player!");
		return PlayerColor.Red;
	}

}
