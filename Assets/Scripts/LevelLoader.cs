using UnityEngine;
using System.Collections;

public class LevelLoader : MonoBehaviour,IActivator {
	public string[] Scenes;
	private int sceneIndex = 0;


	public void Activate()
	{
		if (sceneIndex < Scenes.Length - 1)
		{
			sceneIndex += 1;
			Application.LoadLevel(Scenes[sceneIndex]);
		}
		else
		{
			Debug.Log ("No more levels to load!");
		}
	}
}
