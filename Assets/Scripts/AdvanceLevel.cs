
using UnityEngine;
using System.Collections;

public class AdvanceLevel : MonoBehaviour, IReset {

	private bool redComplete;
	private bool greenComplete;
	private bool blueComplete;

	// Use this for initialization
	void Awake () {
		redComplete = false;
		greenComplete = false;
		blueComplete = false;
	
	}
	
	// Update is called once per frame
	void Update () {

		if (redComplete && greenComplete && blueComplete)
		{
			redComplete = false;
			greenComplete = false;
			blueComplete = false;
			this.LoadNextLevel();
		}
	
	}

	private void LoadNextLevel()
	{
		int loadedLevel = Application.loadedLevel;
		if (loadedLevel < Application.levelCount - 1)
		{
			Application.LoadLevel (loadedLevel + 1);
		}
		else
		{
			Application.LoadLevel(0);
		}
	}

	/// <summary>
	/// Tells the level controller that this player has completed the level.
	/// If all 3 players have completed the level, the level advances.
	/// </summary>
	/// <param name="name">The player name. Can be "Red", "Green", or "Blue".</param>
	/// <param name="complete">Whether the player has completed the level.</param>
	public void SetComplete(string name, bool complete)
	{
		if (name.Equals ("Red"))
			redComplete = complete;
		else if (name.Equals("Green"))
			greenComplete = complete;
		else if (name.Equals ("Blue"))
			blueComplete = complete;
		else
			Debug.LogError("Passed a name that wasn't Red, Green, or Blue!");
	}

	public void Reset()
	{
		redComplete = false;
		greenComplete = false;
		blueComplete = false;
	}

	public void Resetting(float resetTime){}
	
}
