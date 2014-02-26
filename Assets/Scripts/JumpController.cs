using UnityEngine;
using System.Collections;

public class JumpController : MonoBehaviour {
	private int groundCount;

	void OnCollisionEnter2D(Collision2D collision)
	{
		int layerNumber = collision.gameObject.layer;
		if (LayerMask.LayerToName(layerNumber) == "Ground")
		{
			this.groundCount += 1;
		}
	}
	void OnCollisionExit2D(Collision2D collision)
	{
		int layerNumber = collision.gameObject.layer;
		if (LayerMask.LayerToName(layerNumber) == "Ground")
		{
			this.groundCount -= 1;
		}
	}

	public bool isGrounded()
	{
		return this.groundCount > 0;
	}
}
