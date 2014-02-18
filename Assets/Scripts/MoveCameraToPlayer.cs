using UnityEngine;
using System.Collections;

public class MoveCameraToPlayer : MonoBehaviour {

	public float zoomSpeed; //How fast the camera should zoom in and out on the player
	public float trackXSpeed = 1; //Should we track the horizontal
	public float trackYSpeed = 1; //Should we track the vertical
	
	public GameObject player;
	
	void Update () {
		this.transform.position = new Vector3(player.transform.position.x * trackXSpeed,player.transform.position.y * trackYSpeed,transform.position.z);
		camera.orthographicSize = Mathf.Lerp (camera.orthographicSize,player.rigidbody2D.velocity.magnitude,zoomSpeed);
	}
}
