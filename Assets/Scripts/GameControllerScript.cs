using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using BreakGlobals;

public class GameControllerScript : MonoBehaviour, IReset {

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

	private bool resetting;
	private float currentResetTime;

	private ResetLevel resetScript;

	void Awake () {
		this.resetScript = GetComponent<ResetLevel>();
		this.resetting = false;
		this.currentResetTime = 0f;
		SetLoopTime();
		FindPlayersInScene();
		currentPlayer = redPlayer;
		this.FindThingsToReset();
	}

	private void SetLoopTime()
	{
		this.levelCompletionTime = GameObject.FindGameObjectWithTag("LevelStats").GetComponent<LevelStats>().levelCompletionTime;
		this.timeElapsedInLoop = 0;
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
		if (!resetting)
		{
			this.timeElapsedInLoop += 1;
			if (timeElapsedInLoop >= levelCompletionTime)
			{
				this.timeElapsedInLoop = -1;
				resetScript.StartReset(resetList);
				this.resetting  = true;
			}
			else if (currentPlayer != null && currentPlayer.gameObject.activeSelf == false)
				SwitchPlayer();
		}
		
	}

	public void Resetting(float resetTime)
	{
		this.resetting = true;
		this.currentResetTime = resetTime;
	}
	
	public void Reset()
	{
		this.timeElapsedInLoop = 0;
		this.resetting = false;
		this.currentResetTime = 1f;
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
		float rectWidth = Screen.width;
		float rectHeight = Screen.height;
		float normalizedTime;

		if (this.resetting)
			normalizedTime = 1f - currentResetTime;
		else
			normalizedTime = (levelCompletionTime-timeElapsedInLoop) / (levelCompletionTime*1f);

		Color rectColor;
		if (currentPlayer != null)
			rectColor = currentPlayer.GetPlayerColor();
		else
			rectColor = new Color(255,255,255);
		rectColor.a = (0.1f);
		Rect loopRect = new Rect(0,0,rectWidth * normalizedTime, rectHeight);
		// Make the rectangle white and transparent
		Drawing.GUIDrawRect(loopRect, rectColor);
	}



	public CapturedInput GetCurrentInput()
	{
		// Capture our current input if we're not resetting
		bool leftKey = (Input.GetAxis ("Horizontal") < 0);
		bool rightKey = (Input.GetAxis ("Horizontal") > 0);
		bool jumpKey = Input.GetButton ("Jump");
		return new CapturedInput(leftKey,rightKey,jumpKey);
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
		{
			if (greenPlayer.gameObject.activeSelf)
				currentPlayer = greenPlayer;
			else if (bluePlayer.gameObject.activeSelf)
				currentPlayer = bluePlayer;
		}
		else if (currentPlayer == greenPlayer)
		{
			if (bluePlayer.gameObject.activeSelf)
				currentPlayer = bluePlayer;
			else if (redPlayer.gameObject.activeSelf)
				currentPlayer = redPlayer;
		}
		else if (currentPlayer == bluePlayer)
		{
			if (redPlayer.gameObject.activeSelf)
				currentPlayer = redPlayer;
			else if (greenPlayer.gameObject.activeSelf)
				currentPlayer = greenPlayer;
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
