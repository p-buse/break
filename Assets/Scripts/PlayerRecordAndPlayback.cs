using UnityEngine;
using System.Collections.Generic;

public class PlayerRecordAndPlayback : MonoBehaviour {

	public Texture2D recordSymbol;
	public Texture2D playSymbol;

	private GameControllerScript gameController;
	private LinkedList<RecordedInput> recordedFrames;
	private IEnumerator<RecordedInput> playbackHead;
	private RecordingState recordingState;
	enum RecordingState {Idle, Record, Play};
	class RecordedInput
	{
		public float inputHorizontal;
		public bool jumpInput;
		public bool activateInput;

		/// <summary>
		/// Initializes a new recorded frame.
		/// </summary>
		/// <param name="inputHorizontal">Horizontal input.</param>
		/// <param name="jumpInput">If set to <c>true</c> we're jumping</param>
		/// <param name="activateInput">If set to <c>true</c> awe're activating.</param>
		public RecordedInput(float inputHorizontal, bool jumpInput, bool activateInput)
		{
			this.inputHorizontal = inputHorizontal;
			this.jumpInput = jumpInput;
			this.activateInput = activateInput;
		}
		public RecordedInput()
		{
			this.inputHorizontal = 0f;
			this.jumpInput = false;
			this.activateInput = false;
		}
	}

	

	void Awake()
	{
		this.recordedFrames = new LinkedList<RecordedInput>();
		this.gameController = GetComponent<GameControllerScript>();
		recordingState = RecordingState.Idle;
	}

	void SwitchState()
	{
		if (this.recordingState == RecordingState.Idle)
		{
			this.recordingState = RecordingState.Record;
			// Clear the recorded frames
			recordedFrames = new LinkedList<RecordedInput>();
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

			// Capture our input
			inputFrame.inputHorizontal = Input.GetAxis ("Horizontal");
			inputFrame.jumpInput = Input.GetButtonDown ("Jump");
			inputFrame.activateInput = Input.GetButtonDown ("Action");

			// And append it to the list
			recordedFrames.AddLast (inputFrame);
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

	void SendInput(RecordedInput input)
	{
		// Get the current player from our game controller
		PlayerController currentPlayer = gameController.CurrentPlayer();
		// Send our horizontal motion
		currentPlayer.MoveHorizontal(input.inputHorizontal);
		// Jump if we're jumping
		if (input.jumpInput)
			currentPlayer.Jump ();
		// Activate if we're activating
		if (input.activateInput)
			currentPlayer.Activate();
	}

	
}
