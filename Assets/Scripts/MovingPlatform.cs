﻿using UnityEngine;
using System.Collections;

public class MovingPlatform : MonoBehaviour {

	public Transform leftOrUpperLimit; //The left or upper limit
	public Transform rightOrLowerLimit; //The right or lower limit
	public float moveSpeed = 1;
	public enum Direction {Horizontal, Vertical};
	public Direction direction;

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


	void FixedUpdate()
	{
		//Horizontal movement
		if (direction == Direction.Horizontal)
		{
			//Too far to the right
			if (transform.position.x >= rightLimit)
				currentDirection = -1;
			//Too far to the left
			if (transform.position.x <= leftLimit)
				currentDirection = 1;

//			rigidbody2D.transform.position. = new Vector2(moveSpeed * currentDirection,0f);
//			rigidbody2D.transform.position.x += moveSpeed * currentDirection;
			transform.Translate (new Vector3(moveSpeed * currentDirection,0f,0f));
		}
		if (direction == Direction.Vertical)
		{
			//Too far up
			if (transform.position.y >= upLimit)
				currentDirection = -1;
			//Too far down
			if (transform.position.y <= downLimit)
				currentDirection = 1;

//			rigidbody2D.velocity = new Vector2(0f,moveSpeed * currentDirection);
//			rigidbody2D.transform.position.y += moveSpeed * currentDirection;
			transform.Translate (new Vector3(0f,moveSpeed * currentDirection, 0f));
		}
	}
}