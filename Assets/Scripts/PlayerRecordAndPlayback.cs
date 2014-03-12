using UnityEngine;
using System.Collections.Generic;

public class PlayerRecordAndPlayback : MonoBehaviour {

	public Texture2D recordSymbol;
	public Texture2D playSymbol;

	public Transform playerTransform;
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
	}

	

	void Awake()
	{
		this.recordedFrames = new LinkedList<RecordedInput>();
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

	void Update()
	{
		if (Input.GetKeyDown (KeyCode.R))
		{
			SwitchState ();
		}

		switch (this.recordingState)
		{

		case RecordingState.Play:
			if (playbackHead.)
			else
			{
				recordingState = RecordingState.Idle;
			}
			break;

		case RecordingState.Record:
			RecordedInput thisFrame = new RecordedInput(playerTransform.position);
			recordingQueue.Enqueue(thisFrame);
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

	
}
