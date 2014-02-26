using UnityEngine;
using System.Collections;

public class MovingPlatform : MonoBehaviour,IActivator,IMover {

	public Transform leftOrUpperLimit; //The left or upper limit
	public Transform rightOrLowerLimit; //The right or lower limit
	public float moveSpeed = 1;
	public enum Direction {Horizontal, Vertical};
	public Direction direction;
	public bool isActive = true; // Whether this platform moves or not

	private int currentDirection = 1; //Which way are we currently moving?
	private float leftLimit;
	private float rightLimit;
	private float upLimit;
	private float downLimit;



	void Awake()
	{
		leftLimit = leftOrUpperLimit.position.x;
		rightLimit = rightOrLowerLimit.position.x;
		upLimit = leftOrUpperLimit.position.y;
		downLimit = rightOrLowerLimit.position.y;
	}

	public void Activate()
	{
		isActive = !isActive;
	}

	public Vector3 Movement()
	{
		if (isActive) {
			//Horizontal movement
			if (direction == Direction.Horizontal) {
				//Too far to the right
				if (transform.position.x >= rightLimit)
					currentDirection = -1;
				//Too far to the left
				if (transform.position.x <= leftLimit)
					currentDirection = 1;
				
				return new Vector3 (moveSpeed * currentDirection, 0f, 0f);
			}
			if (direction == Direction.Vertical) {
				//Too far up
				if (transform.position.y >= upLimit)
					currentDirection = -1;
				//Too far down
				if (transform.position.y <= downLimit)
					currentDirection = 1;
				
				return new Vector3 (0f, moveSpeed * currentDirection, 0f);
			}
			Debug.LogError("Error: " + this.gameObject + " moving platform has no direction set!");
			return Vector3.zero;
		}
		else
		{
			return Vector3.zero;
		}
	}


	void FixedUpdate()
	{
		transform.Translate (this.Movement());
	}
}
