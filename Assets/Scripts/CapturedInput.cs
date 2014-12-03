using UnityEngine;
using System.Collections;

/// <summary>
/// Class for holding a frame of captured input.
/// </summary>
public class CapturedInput {

	private BitArray inputArray;
	private bool empty;

	/// <summary>
	/// Initializes an CapturedInput with left, right, jump, and action keys.
	/// </summary>
	/// <param name="leftKey">If set to <c>true</c> left key is pressed.</param>
	/// <param name="rightKey">If set to <c>true</c> right key is pressed.</param>
	/// <param name="jumpKey">If set to <c>true</c> jump key is pressed.</param>
	/// <param name="actionKey">If set to <c>true</c> action key is pressed.</param>
	public CapturedInput(bool leftKey, bool rightKey, bool jumpKey)
	{
		inputArray = new BitArray(3);
		inputArray.Set (0,leftKey);
		inputArray.Set (1,rightKey);
		inputArray.Set(2,jumpKey);
		if ((leftKey || rightKey || jumpKey) == false)
		{
			this.empty = true;
		}
		else
			this.empty = false;
	}

	/// <summary>
	/// Initializes a new empty CapturedInput.
	/// </summary>
	public CapturedInput()
	{
		inputArray = new BitArray(3);
		this.empty = true;
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

	public bool isEmpty()
	{
		return this.empty;
	}

	public override string ToString()
	{
		return string.Format("Left: {0}\nRight: {1}\nJump: {2}\nisEmpty? {3}",
		                     getLeft(),getRight(),getJump(),isEmpty());
	}

}
