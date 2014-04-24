//using UnityEngine;
//using System.Collections;
//
//public class CameraController : MonoBehaviour {
//
//	public Camera theCamera;
//	public GameControllerScript gameController;
//
//	public float zoomSpeed; //How fast the camera should zoom in and out on the player
//	public float closeFOV = 4;
//	public float farFOV = 20;
//	public float trackXSpeed = 1; //Should we track the horizontal
//	public float trackYSpeed = 1; //Should we track the vertical
//	
//	public PlayerController currentPlayer;
//
//	void Awake()
//	{
//		gameController = gameObject.GetComponent<GameControllerScript>();
//	}
//
//
//	void Update () {
//		this.transform.position = new Vector3(player.transform.position.x * trackXSpeed,player.transform.position.y * trackYSpeed,transform.position.z);
//		camera.orthographicSize = Mathf.Lerp (camera.orthographicSize,player.rigidbody2D.velocity.magnitude,zoomSpeed);
//	}
//}
