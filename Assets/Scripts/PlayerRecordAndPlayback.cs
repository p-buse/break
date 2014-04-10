using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using BreakGlobals;

public class PlayerRecordAndPlayback : MonoBehaviour {

	LinkedList<IReset> resetList;

	void Awake()
	{
		resetList = new LinkedList<IReset>();
		FindThingsToReset();
	}

	void Update()
	{
		if (Input.GetButtonDown("Action"))
		{
			this.Reset ();
		}
	}

	void Reset()
	{
		print ("resetting!");
		foreach (IReset resetScript in resetList)
		{
			resetScript.Reset ();
		}

	}

	private LinkedList<GameObject> getActiveObjects()
	{
		LinkedList<GameObject> returnList = new LinkedList<GameObject>();
		object[] allObjects = FindObjectsOfType(typeof(GameObject));
		foreach(object currentObject in allObjects) {
			if (((GameObject) currentObject).activeInHierarchy) {
				returnList.AddLast((GameObject) currentObject);
			}
		}
		return returnList;
	}
	
	private void FindThingsToReset()
	{
		LinkedList<GameObject> sceneObjects = this.getActiveObjects();
		foreach (GameObject currentObject in sceneObjects)
		{
			MonoBehaviour[] scriptList = currentObject.GetComponents<MonoBehaviour>();
			foreach (MonoBehaviour currentScript in scriptList)
			{
				if (currentScript is IReset)
				{
					this.resetList.AddLast((IReset) currentScript);
				}
			}
		}
	}

}