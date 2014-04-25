using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	public Camera theCamera;
	public GameControllerScript gameController;

	public float zoomSpeed; //How fast the camera should zoom in and out on the player
	public float closeFOV = 4;
	public float farFOV = 20;
	
	private PlayerController currentPlayer;

	void Awake()
	{
		gameController = gameObject.GetComponent<GameControllerScript>();
		currentPlayer = gameController.CurrentPlayer();
	}


	void Update () {
		currentPlayer = gameController.CurrentPlayer();
		if (currentPlayer == null)
		{
			ZoomFar();
		}
		else
		{
			ZoomClose(currentPlayer);
		}
	}

	void ZoomFar()
	{
		theCamera.transform.position = new Vector3(Mathf.Lerp (theCamera.transform.position.x,0,zoomSpeed),
		                                 Mathf.Lerp(theCamera.transform.position.y,0,zoomSpeed), 
		                                           theCamera.transform.position.z);
		theCamera.orthographicSize = Mathf.Lerp(theCamera.orthographicSize, farFOV, zoomSpeed);
	}

	void ZoomClose(PlayerController player)
	{
		theCamera.transform.position = new Vector3(Mathf.Lerp (theCamera.transform.position.x, player.transform.position.x,zoomSpeed),
		                                           Mathf.Lerp (theCamera.transform.position.y, player.transform.position.y,zoomSpeed),
		                                           theCamera.transform.position.z);
		theCamera.orthographicSize = Mathf.Lerp (theCamera.orthographicSize, closeFOV, zoomSpeed);
	}
}
