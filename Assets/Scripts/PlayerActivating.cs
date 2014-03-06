//using UnityEngine;
//using System.Collections;
//
//public class PlayerActivating : MonoBehaviour {
//
//	
//
//	private Hashtable activatorsList; // Mechanical things you are touching. Key: Instance ID of the thing. Value: The Activator
//
//	
//	
//	void Awake()
//	{
//
//		activatorsList = new Hashtable();
//	
//		
//	}
//
//	void OnLevelWasLoaded()
//	{
//		activatorsList = new Hashtable(); // Clear the activators list
//	}
//	
//
//
//	void OnTriggerEnter2D(Collider2D collider)
//	{
//		if (collider.gameObject.tag.Equals ( "Mechanical"))
//		{
//			// Check whether this contains an activator component
//			IActivator activator = (IActivator) collider.gameObject.GetComponent(typeof(IActivator));
//			if (activator != null)
//			{
//				int activatorID = collider.gameObject.GetInstanceID();
//				// If it isn't in the table
//				if (!activatorsList.ContainsKey(activatorID))
//				{
//					// Add it to the table
//					activatorsList.Add(activatorID,activator);
//				}
//
//			}
//
//		}
//	}
//
//	void OnTriggerExit2D(Collider2D collider)
//	{
//		int activatorID = collider.gameObject.GetInstanceID();
//		if (activatorsList.ContainsKey (activatorID))
//		{
//			activatorsList.Remove(activatorID);
//		}
//	}
//	
//	
//	void Update ()
//	{
//
//		if (Input.GetButtonDown ("Action"))
//		{
//			// Activate anything we're currently touching
//			foreach (IActivator activator in activatorsList.Values)
//			{
//				activator.Activate();
//			}
//		}
//	}
//
//
//}
