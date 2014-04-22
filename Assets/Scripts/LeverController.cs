using UnityEngine;
using System.Collections;

public class LeverController : MonoBehaviour,IActivator,IReset
{
	public Animator animationController;
	public GameObject[] activatedObjects;
	private IActivator[] activateScripts;
	private bool originalState;

	void Awake()
	{
		this.originalState = animationController.GetBool ("isOn");
		activateScripts = new IActivator[activatedObjects.Length];

		for (int i = 0; i < activatedObjects.Length; i++)
		{
			activateScripts[i] = (IActivator) activatedObjects[i].GetComponent (typeof(IActivator));
			if (activateScripts[i] == null)
			{
				Debug.Log ("Warning: Couldn't find IActivator in object " + activatedObjects);
			}
		}
	}

	public void Reset()
	{
		animationController.SetBool("isOn",originalState);
	}

	/// <summary>
	/// Toggles the lever, reverses the animation state of the lever and activates the target script.
	/// </summary>
	public void Activate()
	{
		bool leverIsOn = animationController.GetBool("isOn");
		leverIsOn = !leverIsOn;
		animationController.SetBool("isOn",leverIsOn);
		foreach (IActivator activateScript in activateScripts)
		{
			activateScript.Activate();
		}

	}

}
