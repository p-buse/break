using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using BreakGlobals;

public class GameControllerScript : MonoBehaviour {

	public float switchCooldown = 0.25f;
	private PlayerController redPlayer;
	private PlayerController greenPlayer;
	private PlayerController bluePlayer;
	private PlayerController currentPlayer;
	/// <summary>
	/// The level completion time, measured in 50ths of a second (timescale = 0.02)
	/// </summary>
	private int levelCompletionTime;
	private int timeElapsedInLoop = 0;
	/// <summary>
	/// List of objects to reset when the loop completes
	/// </summary>
	LinkedList<IReset> resetList;

	private float nextSwitchTime = 0f;

	void Awake () {
		SetLoopTime();
		FindPlayersInScene();
		// Change us to red initially
		currentPlayer = redPlayer;
		this.FindThingsToReset();


	}

	private void SetLoopTime()
	{
		this.levelCompletionTime = GameObject.FindGameObjectWithTag("LevelStats").GetComponent<LevelStats>().levelCompletionTime;
	}

	private void FindPlayersInScene()
	{
		GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
		foreach (GameObject currentPlayer in players)
		{
			if (currentPlayer.name.Equals("Red Player"))
				this.redPlayer = currentPlayer.GetComponent<PlayerController>();
			if (currentPlayer.name.Equals("Green Player"))
				this.greenPlayer = currentPlayer.GetComponent<PlayerController>();
			if (currentPlayer.name.Equals("Blue Player"))
				this.bluePlayer = currentPlayer.GetComponent<PlayerController>();
		}
		if (redPlayer == null || greenPlayer == null || bluePlayer == null)
		{
			Debug.LogError("Not all players detected in scene!");
		}
	}

	void OnLevelWasLoaded()
	{
		// Set the level's time to complete
		SetLoopTime();
		// Reset the resetlist
		FindThingsToReset();
		FindPlayersInScene();
		currentPlayer = redPlayer;
	}

	void FixedUpdate()
	{
		this.timeElapsedInLoop += 1;
		if (timeElapsedInLoop >= levelCompletionTime)
		{
			timeElapsedInLoop = 0;
			this.ResetEverything ();
		}

		if (currentPlayer.gameObject.activeSelf == false)
			SwitchPlayer();
		
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
		if (currentPlayer != null)
			currentPlayer.SetInput(currentInput);
		
	}

	void OnGUI()
	{
		
		GUI.Box (new Rect(100,0,100,50),Mathf.FloorToInt((levelCompletionTime-timeElapsedInLoop) * Time.fixedDeltaTime).ToString());
		if (currentPlayer != null)
		{
			guiText.text = "Player: " + currentPlayer.playerName;
			guiText.color = currentPlayer.GetPlayerColor();
		}
		else{
			guiText.text = "No player selected";
			guiText.color = Color.white;
		}

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

	public PlayerController CurrentPlayer()
	{
		return this.currentPlayer;
	}

	public int GetCurrentPositionInLoop()
	{
		return this.timeElapsedInLoop;
	}

	public void SwitchPlayer()
	{
		
		// Stop movement of previous player
		if (currentPlayer != null)
			currentPlayer.SetInput(new CapturedInput());

		// Cycle through the three different players
		if (currentPlayer == redPlayer)
			currentPlayer = greenPlayer;
		else if (currentPlayer == greenPlayer)
			currentPlayer = bluePlayer;
		else
			currentPlayer = redPlayer;
	}

	private void ResetEverything()
	{
		foreach (IReset resetScript in this.resetList)
		{
			resetScript.Reset ();
		}
	}
	
	private void FindThingsToReset()
	{
		this.resetList = new LinkedList<IReset>();
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



}
