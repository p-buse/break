using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using BreakGlobals;

public class PlayerRecordAndPlayback : MonoBehaviour {

	public Texture2D recordSymbol;
	public Texture2D playSymbol;

	public GameControllerScript gameController;
	LinkedList<IReset> resetList;
	enum RecordingState {Idle, Record, Play};
	RecordingState recordingState;
	ArrayList recordedStuff;
	int playbackPosition;

	void Awake()
	{
		this.recordingState = RecordingState.Idle;
		this.playbackPosition = 0;
		this.recordedStuff = new ArrayList();
		this.resetList = new LinkedList<IReset>();
		this.FindThingsToReset();
	}
	

	void Update()
	{

		switch (recordingState)
		{
		case RecordingState.Idle:
			break;
		case RecordingState.Play:
			this.PlaybackFrame ();
			this.playbackPosition++;
			if (playbackPosition >= recordedStuff.Count)
			{
				this.Reset ();
				this.playbackPosition = 0;
			}
			break;
		case RecordingState.Record:
			this.PlaybackFrame ();
			this.RecordFrame ();
			this.playbackPosition++;
			break;
		}

	}
	
	public void SwitchState()
	{
		switch(recordingState)
		{
		case RecordingState.Idle:
			recordingState = RecordingState.Record;
			break;
		case RecordingState.Record:
			recordingState = RecordingState.Play;
			break;
		case RecordingState.Play:
			recordingState = RecordingState.Idle;
			break;
		}
	}

	public void Reset()
	{
		print ("resetting!");
		foreach (IReset resetScript in this.resetList)
		{
			resetScript.Reset ();
		}
	}

	private void PlaybackFrame()
	{
		if (playbackPosition < recordedStuff.Count && recordedStuff[playbackPosition] != null)
		{
			CombinedInput currentInputFrame = (CombinedInput) recordedStuff[playbackPosition];
			gameController.ReceiveInputFromLooper(currentInputFrame);
		}
	}
	
	private void RecordFrame()
	{
		PlayerColor CurrentPlayerColor = gameController.CurrentPlayerColor();
		CapturedInput capturedInput = gameController.CaptureInput();
		// If we're at the end of the recordedstuff array, append rather than replace the input
		if (playbackPosition >= recordedStuff.Count - 1)
		{
			CombinedInput newFrame = new CombinedInput(CurrentPlayerColor, capturedInput);
			recordedStuff.Add (newFrame);
		}
		else
		{
			((CombinedInput) recordedStuff[playbackPosition]).AddPlayerInput(CurrentPlayerColor,capturedInput);
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