using UnityEngine;
using System.Collections;

public class ButtonTrigger : MonoBehaviour
{
	public Animator buttonAnimationController;

	void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.tag == "Player")
		{
			buttonAnimationController.SetBool ("isDown",true);
		}
	}

	void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.tag == "Player")
		{
			buttonAnimationController.SetBool ("isDown",false);
		}
	}
}
