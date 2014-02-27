using UnityEngine;
using System.Collections;

public class KeepPersistentAcrossScenes : MonoBehaviour {

	void Awake()
	{
		DontDestroyOnLoad(transform.gameObject);
	}
}
