﻿//using UnityEngine;
//using System.Collections.Generic;
//using System.Collections;
//using BreakGlobals;
//
//public class PlayerRecordAndPlaybackOld : MonoBehaviour {
//
//	// Image assets
//	public Texture2D recordSymbol;
//	public Texture2D playSymbol;
//
//	private GameControllerScript gameController;
//
//	private LinkedList<CombinedInput> recordedFrames;
//	private IEnumerator<CombinedInput> playbackHead;
//	enum RecordingState {Idle, Record, Play};
//	private RecordingState recordingState;
//	// Everything that needs to be reset when we loop
//	private LinkedList<IReset> thingsToReset;
//
//	void Awake()
//	{
//		this.recordedFrames = new LinkedList<CombinedInput>();
//		this.gameController = GetComponent<GameControllerScript>();
//		FindThingsToReset();
//	}
//
//	private void FindThingsToReset()
//	{
//		object[] allObjects = FindObjectsOfType(typeof(GameObject));
//		foreach(object currentObject in allObjects) {
//			if (((GameObject) currentObject).activeInHierarchy) {
//				print (currentObject + " is an active object!")
//			}
//		}
//
////		thingsToReset = new LinkedList<IReset>();
////		GameObject[] gameObjects = GameObject.FindObjectsOfType(typeof(GameObject)) as GameObject[];
////		foreach (GameObject sceneObject in gameObjects)
////		{
////
////			IReset resetScript = sceneObject as IReset;
////			if (resetScript != null)
////				thingsToReset.AddLast(resetScript);
////		}
//	}
//
//	PlayerColor GetCurrentPlayer()
//	{
//		if (gameController.CurrentPlayer() == gameController.redPlayer)
//			return PlayerColor.Red;
//		else if (gameController.CurrentPlayer() == gameController.greenPlayer)
//			return PlayerColor.Green;
//		else if (gameController.CurrentPlayer() == gameController.bluePlayer)
//			return PlayerColor.Blue;
//		else
//		{
//			Debug.LogError("Error: Current player is not red, green, or blue!");
//			return PlayerColor.Red;
//		}
//	}
//
//	public void StartRecording()
//	{
//
//	}
//
//	public void StopRecording()
//	{
//
//	}
//
//
//	public void ResetAll()
//	{
//		foreach (IReset resetMe in thingsToReset)
//		{
//			resetMe.Reset ();
//		}
//	}
//
//	void SwitchState()
//	{
//		if (this.recordingState == RecordingState.Idle)
//		{
//			this.recordingState = RecordingState.Record;
//			// Clear the recorded frames
//			recordedFrames = new LinkedList<CombinedInput>();
//		}
//		else if (this.recordingState == RecordingState.Record)
//		{
//			// Start playing and set the playback head to the beginning of the linked list
//			this.recordingState = RecordingState.Play;
//			this.playbackHead = recordedFrames.GetEnumerator();
//		}
//		else if (this.recordingState == RecordingState.Play)
//		{
//			this.recordingState = RecordingState.Idle;
//		}
//	}
//
//	void LateUpdate()
//	{
//		if (Input.GetKeyDown (KeyCode.R))
//		{
//			SwitchState ();
//		}
//
//		switch (this.recordingState)
//		{
//
//		case RecordingState.Play:
//			// If the playback head is not at the end, send our input, otherwise, loop!
//			if (playbackHead.MoveNext() != false)
//			{
//				SendInput(playbackHead.Current);
//			}
//			else
//			{
//				playbackHead.Reset();
//			}
//			break;
//
//		case RecordingState.Record:
//			RecordedInput inputFrame = new RecordedInput();
//			PlayerColor playerColor = this.GetCurrentPlayer();
//
//			// Capture our input
//			inputFrame.inputHorizontal = Input.GetAxis ("Horizontal");
//			inputFrame.jumpInput = Input.GetButtonDown ("Jump");
//			inputFrame.activateInput = Input.GetButtonDown ("Action");
//
//			CombinedInput combinedInput = new CombinedInput();
//			combinedInput.AddPlayerInput(playerColor, inputFrame);
//			recordedFrames.AddLast(combinedInput);
//			break;
//
//		case RecordingState.Idle:
//			break;
//		}
//	}
//
//	void OnGUI()
//	{
//		switch (recordingState)
//		{
//		case RecordingState.Idle:
//			GUI.Box (new Rect (Screen.width - 100,0,100,50), "IDLE");
//			break;
//		case RecordingState.Record:
//			GUI.Box (new Rect (Screen.width - 100,0,100,50), recordSymbol);
//			break;
//		case RecordingState.Play:
//			GUI.Box (new Rect (Screen.width - 100,0,100,50), playSymbol);
//			break;
//		}
//
//
//	}
//
//	void SendInput(CombinedInput combinedInput)
//	{
//		PlayerController currentPlayer = null;
//		PlayerColor[] colorList = {PlayerColor.Red, PlayerColor.Green, PlayerColor.Blue}; // HACK CITY
//		foreach (PlayerColor currentColor in colorList)
//		{
//			if (currentColor == PlayerColor.Red)
//				currentPlayer = gameController.redPlayer;
//			else if (currentColor == PlayerColor.Green)
//				currentPlayer = gameController.greenPlayer;
//			else if (currentColor == PlayerColor.Blue)
//				currentPlayer = gameController.bluePlayer;
//
//			RecordedInput currentInput = combinedInput.GetPlayerInput(currentColor);
//			// Send our horizontal motion
//			currentPlayer.MoveHorizontal(currentInput.inputHorizontal);
//			// Jump if we're jumping
//			if (currentInput.jumpInput)
//				currentPlayer.Jump ();
//			// Activate if we're activating
//			if (currentInput.activateInput)
//				currentPlayer.Activate();
//		}
//	}
//
//
//	
//}
