using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class PlayerRecordAndPlayback : MonoBehaviour {

	// Image assets
	public Texture2D recordSymbol;
	public Texture2D playSymbol;

	private GameControllerScript gameController;
	private LinkedList<CombinedInput> recordedFrames;
	private IEnumerator<CombinedInput> playbackHead;
	enum RecordingState {Idle, Record, Play};
	private RecordingState recordingState;
	enum PlayerColor {Red = 0, Green = 1, Blue = 2};
	class CombinedInput
	{
		private CapturedInput redInput;
		private CapturedInput greenInput;
		private CapturedInput blueInput;

		public CombinedInput()
		{
			redInput = null;
			greenInput = null;
			blueInput = null;
		}


		/// <summary>
		/// Adds the player input to this input frame. This replaces already existing input!
		/// </summary>
		/// <param name="playerColor">Player color to add</param>
		/// <param name="recordedInput">Recorded input to add</param>
		public void AddPlayerInput(PlayerColor playerColor, CapturedInput capturedInput)
		{
			switch (playerColor)
			{
			case PlayerColor.Red:
				if (redInput != null)
					Debug.LogWarning("Replaced red input where input already existed!");
				redInput = capturedInput;
				break;
			case PlayerColor.Green:
				if (greenInput != null)
					Debug.LogWarning("Replaced green input where input already existed!");
				greenInput = capturedInput;
				break;
			case PlayerColor.Blue:
				if (blueInput != null)
					Debug.LogWarning("Replaced blue input where input already existed!");
				blueInput = capturedInput;
				break;
			}
		}
		/// <summary>
		/// Clears the player input for a particular color.
		/// </summary>
		/// <param name="playerColor">Player color to clear input of.</param>
		public void ClearRecordedInput(PlayerColor playerColor)
		{
			switch (playerColor)
			{
			case PlayerColor.Red:
				redInput = null;
				break;
			case PlayerColor.Green:
				greenInput = null;
				break;
			case PlayerColor.Blue:
				blueInput = null;
				break;
			}
		}

		/// <summary>
		/// Gets the input for a particular color
		/// </summary>
		/// <returns>A recorded frame of input</returns>
		/// <param name="playerColor">Player color to get input of</param>
		public CapturedInput GetPlayerInput(PlayerColor playerColor)
		{
			switch(playerColor)
			{
			case PlayerColor.Red:
				if (redInput != null)
					return redInput;
				else
					return new CapturedInput();
				break;
			case PlayerColor.Green:
				if (greenInput != null)
					return greenInput;
				else
					return new CapturedInput();
				break;
			case PlayerColor.Blue:
				if (blueInput != null)
					return blueInput;
				else
					return new CapturedInput();
				break;
			default:
				Debug.LogError ("Invalid player color for GetPlayerInput!");
				return new CapturedInput();
				break;
			}
		}

	}

	

	void Awake()
	{
		this.recordedFrames = new LinkedList<CombinedInput>();
		this.gameController = GetComponent<GameControllerScript>();
	}

	PlayerColor GetCurrentPlayer()
	{
		if (gameController.CurrentPlayer() == gameController.redPlayer)
			return PlayerColor.Red;
		else if (gameController.CurrentPlayer() == gameController.greenPlayer)
			return PlayerColor.Green;
		else if (gameController.CurrentPlayer() == gameController.bluePlayer)
			return PlayerColor.Blue;
		else
		{
			Debug.LogError("Error: Current player is not red, green, or blue!");
			return PlayerColor.Red;
		}
	}

	void SwitchState()
	{
		if (this.recordingState == RecordingState.Idle)
		{
			this.recordingState = RecordingState.Record;
			// Clear the recorded frames
			recordedFrames = new LinkedList<CombinedInput>();
		}
		else if (this.recordingState == RecordingState.Record)
		{
			// Start playing and set the playback head to the beginning of the linked list
			this.recordingState = RecordingState.Play;
			this.playbackHead = recordedFrames.GetEnumerator();
		}
		else if (this.recordingState == RecordingState.Play)
		{
			this.recordingState = RecordingState.Idle;
		}
	}

	void LateUpdate()
	{
		if (Input.GetKeyDown (KeyCode.R))
		{
			SwitchState ();
		}

		switch (this.recordingState)
		{

		case RecordingState.Play:
			// If the playback head is not at the end, send our input, otherwise, loop!
			if (playbackHead.MoveNext() != false)
			{
				SendInput(playbackHead.Current);
			}
			else
			{
				playbackHead.Reset();
			}
			break;

		case RecordingState.Record:
			RecordedInput inputFrame = new RecordedInput();
			PlayerColor playerColor = this.GetCurrentPlayer();

			// Capture our input
			inputFrame.inputHorizontal = Input.GetAxis ("Horizontal");
			inputFrame.jumpInput = Input.GetButtonDown ("Jump");
			inputFrame.activateInput = Input.GetButtonDown ("Action");

			CombinedInput combinedInput = new CombinedInput();
			combinedInput.AddPlayerInput(playerColor, inputFrame);
			recordedFrames.AddLast(combinedInput);
			break;

		case RecordingState.Idle:
			break;
		}
	}

	void OnGUI()
	{
		switch (recordingState)
		{
		case RecordingState.Idle:
			GUI.Box (new Rect (Screen.width - 100,0,100,50), "IDLE");
			break;
		case RecordingState.Record:
			GUI.Box (new Rect (Screen.width - 100,0,100,50), recordSymbol);
			break;
		case RecordingState.Play:
			GUI.Box (new Rect (Screen.width - 100,0,100,50), playSymbol);
			break;
		}


	}

	void SendInput(CombinedInput combinedInput)
	{
		PlayerController currentPlayer = null;
		PlayerColor[] colorList = {PlayerColor.Red, PlayerColor.Green, PlayerColor.Blue}; // HACK CITY
		foreach (PlayerColor currentColor in colorList)
		{
			if (currentColor == PlayerColor.Red)
				currentPlayer = gameController.redPlayer;
			else if (currentColor == PlayerColor.Green)
				currentPlayer = gameController.greenPlayer;
			else if (currentColor == PlayerColor.Blue)
				currentPlayer = gameController.bluePlayer;

			RecordedInput currentInput = combinedInput.GetPlayerInput(currentColor);
			// Send our horizontal motion
			currentPlayer.MoveHorizontal(currentInput.inputHorizontal);
			// Jump if we're jumping
			if (currentInput.jumpInput)
				currentPlayer.Jump ();
			// Activate if we're activating
			if (currentInput.activateInput)
				currentPlayer.Activate();
		}
	}


	
}
