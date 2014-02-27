using UnityEngine;
using System.Collections;

public class LevelLoader : MonoBehaviour,IActivator {


	public void Activate()
	{
		int loadedLevel = Application.loadedLevel;
		if (loadedLevel < Application.levelCount - 1)
		{
			Application.LoadLevel (loadedLevel + 1);
		}
		else
		{
			Debug.Log ("No more levels to load!");
		}
	}
}
