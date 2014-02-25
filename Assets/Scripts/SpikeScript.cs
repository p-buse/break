using UnityEngine;
using System.Collections;

public class SpikeScript : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D collider)
	{
		if (collider.gameObject.tag == "Player")
		{
			Destroy(collider.gameObject);
		}
	}
}
