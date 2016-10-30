using UnityEngine;
using System.Collections;

public class LeverController : MonoBehaviour,IActivator,IReset
{
	public Animator animationController;
	public GameObject[] activatedObjects;
	public bool startActive;
	private IActivator[] activateScripts;
	private bool originalState;
	private RaycastHit2D[] thePlayer = new RaycastHit2D[1];
	private int defaultLayer;
	private float leftOfBox;
	private float rightOfBox;

	void Awake()
	{
		this.defaultLayer = 1 << LayerMask.NameToLayer("Default");
		BoxCollider2D boxCol = GetComponent<BoxCollider2D>();
		this.leftOfBox = boxCol.offset.x - (boxCol.size.x / 2);
		this.rightOfBox = boxCol.offset.x + (boxCol.size.x / 2);
		this.originalState = this.startActive;
		animationController.SetBool ("isOn", this.startActive);
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

	void FixedUpdate()
	{
		Vector3 leftPos = transform.position + new Vector3(leftOfBox, 0f, 0f);
		Vector3 rightPos = transform.position + new Vector3(rightOfBox, 0f, 0f);
		// Cast a line and check if it collides with ground
		int numPlayers = Physics2D.LinecastNonAlloc (leftPos, rightPos, thePlayer, defaultLayer);
		// If we collided with > 0 "Ground" objects, then we're grounded!
		if (numPlayers > 0)
			this.Activate(true);
		else
			this.Activate(false);
	}

	public void Reset()
	{
			this.startActive = originalState;
			animationController.SetBool("isOn",this.startActive);
	}

	public void Resetting(float r){}

	
	private void ActivateObjects(bool isActive)
	{
		foreach (IActivator activateScript in activateScripts)
		{
			activateScript.Activate(isActive);
		}
	}


	public void Activate(bool isActive)
	{
		animationController.SetBool("isOn", isActive);
		ActivateObjects(isActive);
	}

}
