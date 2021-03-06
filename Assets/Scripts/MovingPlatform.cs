﻿using UnityEngine;
using System.Collections;

public class MovingPlatform : MonoBehaviour,IActivator,IMover,IReset {
	public Transform leftOrUpperLimit; //The left or upper limit of movement
	public Transform rightOrLowerLimit; //The right or lower limit of movement
	public float moveSpeed = 1;
	public enum Direction {Horizontal, Vertical};
	public Direction direction; // Vertical or Horizontal
	public bool isActive = true; // Whether this platform starts out moving or not

	private int currentDirection = 1; //Which way are we currently moving?

	// Used to store how far we go
	private float leftLimit;
	private float rightLimit;
	private float upLimit;
	private float downLimit;

	// Used for resetting to original position and movement
	private Vector3 originalPosition;
	private int originalDirection;
	private bool originalIsActive;

	private Animator animController;


	void Awake()
	{
		// Find our limits of movement
		leftLimit = leftOrUpperLimit.position.x;
		rightLimit = rightOrLowerLimit.position.x;
		upLimit = leftOrUpperLimit.position.y;
		downLimit = rightOrLowerLimit.position.y;

		// Save originals so we can reset
		originalPosition = transform.position;
		originalIsActive = isActive;
		originalDirection = currentDirection;

		this.animController = GetComponent<Animator>();
		if (this.animController == null)
			Debug.LogWarning("Warning: Moving platform couldn't find animator!");
	}



	void FixedUpdate()
	{
		GetComponent<Rigidbody2D>().velocity = (this.Movement());
		this.animController.SetBool("isMoving",this.isActive);
	}



	public void Activate(bool isActive)
	{
		this.isActive = isActive;
	}

	public void Reset()
	{
		this.transform.position = this.originalPosition;
		this.isActive = this.originalIsActive;
		this.currentDirection = this.originalDirection;
	}

	public void Resetting(float resetTime)
	{
		this.transform.position = Vector3.Lerp (this.transform.position, originalPosition, resetTime);
	}


	public Vector2 Movement()
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
				
				return new Vector2 (moveSpeed * currentDirection, 0f);
			}
			// Vertical movement
			if (direction == Direction.Vertical) {
				//Too far up
				if (transform.position.y >= upLimit)
					currentDirection = -1;
				//Too far down
				if (transform.position.y <= downLimit)
					currentDirection = 1;
				
				return new Vector2 (0f, moveSpeed * currentDirection);
			}
			Debug.LogError("Error: " + this.gameObject + " moving platform has no direction set!");
			return Vector2.zero;
		}
		else
			// We're inactive
		{
			return Vector2.zero;
		}
	}



}
