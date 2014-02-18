using UnityEngine;
using System.Collections;

public class MoveCameraToPlayer : MonoBehaviour {

	public float zoomSpeed; //How fast the camera should zoom in and out on the player

	public GameObject player;
	
	void Update () {
		this.transform.position = new Vector3(player.transform.position.x,player.transform.position.y,transform.position.z);
		camera.orthographicSize = Mathf.Lerp (camera.orthographicSize,player.rigidbody2D.velocity.magnitude,zoomSpeed);
	}
}
