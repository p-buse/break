using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ResetLevel : MonoBehaviour {

	public int timeToReset;
	private int currentResetTime;
	private enum State {Idle, Resetting};
	private State state;
	LinkedList<IReset> resetList;


	void Awake()
	{
		resetList = new LinkedList<IReset>();
		state = State.Idle;
		currentResetTime = 0;
	}

	void ResettingAll(float resetTime)
	{
		foreach (IReset resetScript in this.resetList)
		{
			resetScript.Resetting (resetTime);
		}
	}

	void ResetAll()
	{
		foreach (IReset resetScript in this.resetList)
		{
			resetScript.Reset ();
		}
	}

	public void StartReset(LinkedList<IReset> resetList)
	{
		this.resetList = resetList;
		this.state = State.Resetting;
	}

	void FixedUpdate()
	{
		switch (state)
		{
		case State.Idle:
			break;
		case State.Resetting:
			if (currentResetTime >= timeToReset)
			{
				ResetAll();
				state = State.Idle;
				currentResetTime = 0;
				break;
			}
			else
			{
				float normalizedResetTime;
				if (currentResetTime == timeToReset)
				{
					normalizedResetTime = 0f;
				}
				else if (currentResetTime == 0)
				{
					normalizedResetTime = 1f;
				}
				else
				{
					normalizedResetTime = (float)(timeToReset - currentResetTime) / (float)(timeToReset);
				}
				ResettingAll(normalizedResetTime);
				currentResetTime += 1;
				break;
			}


		}
	}
}
