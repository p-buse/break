using UnityEngine;
using System.Collections;

public class LeverController : MonoBehaviour,IActivator
{
	public Animator animationController;
	public GameObject activatedObject;
	private IActivator activateScript;

	void Awake()
	{
		activateScript = (IActivator) activatedObject.GetComponent (typeof(IActivator));
		if (activateScript == null)
		{
			Debug.Log ("Warning: Couldn't find IActivator in object " + activatedObject);
		}
	}

	/// <summary>
	/// Toggles the lever, reverses the animation state of the lever and activates the target script.
	/// </summary>
	public void Activate()
	{
		bool leverIsLeft = animationController.GetBool("isLeft");
		leverIsLeft = !leverIsLeft;
		animationController.SetBool("isLeft",leverIsLeft);
		activateScript.Activate();
	}

}
