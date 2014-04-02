using UnityEngine;
using BreakGlobals;

public class CombinedInput
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