﻿using UnityEngine;
using System.Collections.Generic;

public class PlayerRecordAndPlayback : MonoBehaviour {

	public Texture2D recordSymbol;
	public Texture2D playSymbol;

	public Transform playerTransform;
	private Queue<RecordedFrame> recordingQueue;
	private RecordingState recordingState;
	enum RecordingState {Idle, Record, Play};
	class RecordedFrame
	{
		public Vector3 playerPosition;

		/// <summary>
		/// Initializes a new recorded frame which includes a delta time and x,y,z position.
		/// </summary>
		/// <param name="playerPosition">Player position.</param>
		public RecordedFrame(Vector3 playerPosition)
		{
			this.playerPosition = playerPosition;
		}
	}
	

	void Awake()
	{
		this.recordingQueue = new Queue<RecordedFrame>();
		recordingState = RecordingState.Idle;
	}

	void SwitchState()
	{
		if (this.recordingState == RecordingState.Idle)
		{
			this.recordingState = RecordingState.Record;
			// Clear the recorded frames
			recordingQueue = new Queue<RecordedFrame>();
		}
		else if (this.recordingState == RecordingState.Record)
		{
			this.recordingState = RecordingState.Play;
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
			if (recordingQueue.Count != 0)
			{
					RecordedFrame nextFrame = recordingQueue.Dequeue();
					playerTransform.position = nextFrame.playerPosition;
			}
			else
			{
				recordingState = RecordingState.Idle;
			}
			break;

		case RecordingState.Record:
			RecordedFrame thisFrame = new RecordedFrame(playerTransform.position);
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
