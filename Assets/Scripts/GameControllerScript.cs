using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using BreakGlobals;

public class GameControllerScript : MonoBehaviour {

	public float switchCooldown = 0.25f;
	public PlayerController redPlayer;
	public PlayerController greenPlayer;
	public PlayerController bluePlayer;
	private PlayerController currentPlayer;
	/// <summary>
	/// The level completion time, measured in 50ths of a second (timescale = 0.02)
	/// </summary>
	public int levelCompletionTime;
	private int timeElapsedInLoop = 0;
	/// <summary>
	/// List of objects to reset when the loop completes
	/// </summary>
	LinkedList<IReset> resetList;

	private float nextSwitchTime = 0f;


	private void ResetEverything()
	{
		print ("resetting!");
		foreach (IReset resetScript in this.resetList)
		{
			resetScript.Reset ();
		}
	}

	private void FindThingsToReset()
	{
		LinkedList<GameObject> sceneObjects = this.getActiveObjects();
		foreach (GameObject currentObject in sceneObjects)
		{
			MonoBehaviour[] scriptList = currentObject.GetComponents<MonoBehaviour>();
			foreach (MonoBehaviour currentScript in scriptList)
			{
				if (currentScript is IReset)
				{
					this.resetList.AddLast((IReset) currentScript);
				}
			}
		}
	}
	
	private LinkedList<GameObject> getActiveObjects()
	{
		LinkedList<GameObject> returnList = new LinkedList<GameObject>();
		object[] allObjects = FindObjectsOfType(typeof(GameObject));
		foreach(object currentObject in allObjects) {
			if (((GameObject) currentObject).activeInHierarchy) {
				returnList.AddLast((GameObject) currentObject);
			}
		}
		return returnList;
	}


	void Start () {
		// Change us to red initially
		SwitchPlayer();
		this.resetList = new LinkedList<IReset>();
		this.FindThingsToReset();
	}

	public CapturedInput GetCurrentInput()
	{
		// Capture our current input
		bool leftKey = (Input.GetAxis ("Horizontal") < 0);
		bool rightKey = (Input.GetAxis ("Horizontal") > 0);
		bool jumpKey = Input.GetButtonDown ("Jump");
		bool actionKey = Input.GetButtonDown("Action");
		return new CapturedInput(leftKey,rightKey,jumpKey,actionKey);
	}

	void FixedUpdate()
	{
		this.timeElapsedInLoop += 1;
		if (timeElapsedInLoop >= levelCompletionTime)
		{
			timeElapsedInLoop = 0;
			this.ResetEverything ();
		}

	}

	void Update () {
		// Switch players, if pressing the switch button
		if (Input.GetButton("SwitchPlayer") && Time.time > nextSwitchTime)
		{
			nextSwitchTime = Time.time + this.switchCooldown;
			SwitchPlayer ();
		}

		// Capture our current input
		CapturedInput currentInput = this.GetCurrentInput();
		// Send input to current player
		currentPlayer.SetInput(currentInput);

	}


//	public void ReceiveInputFromLooper(CombinedInput looperInput)
//	{
//		redPlayer.ReceiveInput(looperInput.GetPlayerInput(PlayerColor.Red));
//		greenPlayer.ReceiveInput(looperInput.GetPlayerInput(PlayerColor.Green));
//		bluePlayer.ReceiveInput(looperInput.GetPlayerInput(PlayerColor.Blue));
//	}

	void SwitchPlayer()
	{

		// Stop movement of current player
		if (currentPlayer != null)
			currentPlayer.SetInput(new CapturedInput());
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

	public int GetCurrentPositionInLoop()
	{
		return this.timeElapsedInLoop;
	}

	void OnGUI()
	{

		GUI.Box (new Rect(100,0,100,50),Mathf.FloorToInt((levelCompletionTime-timeElapsedInLoop) * Time.fixedDeltaTime).ToString());		
		
	}
}
