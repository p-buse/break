using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour, IReset {

	public Camera theCamera;
	public GameControllerScript gameController;
	private bool resetting;

	public float zoomSpeed; //How fast the camera should zoom in and out on the player
	public float closeFOV;
	private float farFOV;
	
	private PlayerController currentPlayer;

	void Awake()
	{
		farFOV = theCamera.orthographicSize;
		resetting = false;
		gameController = gameObject.GetComponent<GameControllerScript>();
		currentPlayer = gameController.CurrentPlayer();
	}


	void Update () {
		currentPlayer = gameController.CurrentPlayer();
		if (currentPlayer == null)
		{
			ZoomFar(this.zoomSpeed);
		}
		else if (resetting == true)
		{

		}
		else
		{
			ZoomClose(currentPlayer);
		}
	}

	public void Resetting(float resetTime)
	{
		ZoomFar(1f - resetTime);
		this.resetting = true;
	}

	public void Reset()
	{
		this.resetting = false;
	}

	void ZoomFar(float zoomSpeed)
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
