using UnityEngine;
using System.Collections;

public class LeverController : MonoBehaviour,IActivator,IReset
{
	public Animator animationController;
	public GameObject[] activatedObjects;
	public bool isActive;
	private IActivator[] activateScripts;
	private bool originalState;

	void Awake()
	{
		this.originalState = this.isActive;
		animationController.SetBool ("isOn", this.isActive);
		activateScripts = new IActivator[activatedObjects.Length];

		for (int i = 0; i < activatedObjects.Length; i++)
		{
			activateScripts[i] = (IActivator) activatedObjects[i].GetComponent (typeof(IActivator));
			if (activateScripts[i] == null)
			{
				Debug.LogWarning ("Warning: Couldn't find IActivator in object " + activatedObjects);
			}
		}
	}

	public void Reset()
	{
			this.isActive = originalState;
			animationController.SetBool("isOn",this.isActive);
	}

	public void Resetting(float r){}

	private void ActivateObjects()
	{
		foreach (IActivator activateScript in activateScripts)
		{
			activateScript.Activate();
		}
	}

	/// <summary>
	/// Toggles the lever, reverses the animation state of the lever and activates the target script.
	/// </summary>
	public void Activate()
	{
		this.isActive = !this.isActive;
		animationController.SetBool("isOn", this.isActive);
		ActivateObjects();

	}

	public void Activate(bool isActive)
	{
		animationController.SetBool("isOn", this.isActive);
		ActivateObjects();
	}

}
