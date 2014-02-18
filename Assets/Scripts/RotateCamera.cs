using UnityEngine;
using System.Collections;

public class RotateCamera : MonoBehaviour {

	public Rigidbody2D playerBody;
	public float rotateHorizontal = 0.01f;
	public float rotateVertical = 0.1f;
	public float resetTime = 0.1f;

	

	void Update ()
	{
		transform.Rotate (Mathf.Abs (playerBody.velocity.normalized.y * rotateVertical),0f,Mathf.Abs (playerBody.velocity.normalized.x * rotateHorizontal));
		transform.rotation = Quaternion.Lerp (Quaternion.identity,transform.rotation,resetTime);
	}
}
