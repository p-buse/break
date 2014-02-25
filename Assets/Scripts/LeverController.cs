using UnityEngine;
using System.Collections;

public class LeverController : MonoBehaviour,IActivator
{
	public Animator animationController;
	

	/// <summary>
	/// Toggles the lever
	/// </summary>
	public void Activate()
	{
		bool leverIsLeft = animationController.GetBool("isLeft");
		leverIsLeft = !leverIsLeft;
		animationController.SetBool("isLeft",leverIsLeft);
	}
}
