using UnityEngine;
using System.Collections;

public class DoorController : MonoBehaviour,IActivator {
	private LevelLoader gameController;

	void Awake()
	{
		gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<LevelLoader>();
	}

	public void Activate()
	{
		gameController.Activate();
	}

}
