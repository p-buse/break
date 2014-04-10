using UnityEngine;
using System.Collections;

public class LeverController : MonoBehaviour,IActivator,IReset
{
	public Animator animationController;
	public GameObject activatedObject;
	private IActivator activateScript;
	private bool originalState;

	void Awake()
	{
		this.originalState = animationController.GetBool ("isOn");
		activateScript = (IActivator) activatedObject.GetComponent (typeof(IActivator));
		if (activateScript == null)
		{
			Debug.Log ("Warning: Couldn't find IActivator in object " + activatedObject);
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
		activateScript.Activate();
	}

}
