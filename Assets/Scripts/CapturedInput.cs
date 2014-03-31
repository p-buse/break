using UnityEngine;
using System.Collections;

/// <summary>
/// Class for holding a frame of captured input.
/// </summary>
public class CapturedInput {

	private BitArray inputArray;
	enum Buttons {Left=0, Right=1, Jump=2, Action=3};

	/// <summary>
	/// Initializes an CapturedInput with left, right, jump, and action keys.
	/// </summary>
	/// <param name="leftKey">If set to <c>true</c> left key is pressed.</param>
	/// <param name="rightKey">If set to <c>true</c> right key is pressed.</param>
	/// <param name="jumpKey">If set to <c>true</c> jump key is pressed.</param>
	/// <param name="actionKey">If set to <c>true</c> action key is pressed.</param>
	public CapturedInput(bool leftKey, bool rightKey, bool jumpKey, bool actionKey)
	{
		inputArray = new BitArray(4);
		inputArray.Set (0,leftKey);
		inputArray.Set (1,rightKey);
		inputArray.Set(2,jumpKey);
		inputArray.Set(3,actionKey);
	}

	/// <summary>
	/// Initializes a new empty CapturedInput.
	/// </summary>
	public CapturedInput()
	{
		inputArray = new BitArray(4);
	}

	public bool getLeft()
	{
		return inputArray.Get(0);
	}

	public bool getRight()
	{
		return inputArray.Get (1);
	}

	public bool getJump()
	{
		return inputArray.Get(2);
	}

	public bool getAction()
	{
		return inputArray.Get(3);
	}

}
