using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//author manuel fischer
// put on item with collider or rigidbody
// optional: put outline script on and activate on mouseenter to have hover effekt, disable on exit
public class OnHoverActive : MonoBehaviour {

	public UnityEvent OnMouseEnterResonse,OnMouseExitResonce;
	private void OnMouseEnter()
	{
		OnMouseEnterResonse.Invoke();
	}
		private void OnMouseExit()
	{
		OnMouseExitResonce.Invoke();
	}
}
