using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using Photon.Pun;
using System.Collections;

//author manuel fischer
// put on item with collider or rigidbody
// optional: put outline script on and activate on mouseenter to have hover effekt, disable on exit
public class OnHoverActive : MonoBehaviour {

	public delegate void ObjectInteractibleDelegate(bool mouseOver);
	public static event ObjectInteractibleDelegate OnObjectInteractible;

	[SerializeField]
	[FormerlySerializedAs("OnMouseEnterResonse")]
	private UnityEvent OnMouseEnterEvent;

	[SerializeField]
	[FormerlySerializedAs("OnMouseExitResonce")]
	[FormerlySerializedAs("OnMouseExitResponse")]
	private UnityEvent OnMouseExitEvent;

	private bool isMouseOver = false;
	private bool isInInteractibleDistance = false;
	private Transform localPlayer;



	private IEnumerator Start() {
		// while(PhotonNetwork.LocalPlayer.TagObject == null){
		// 	yield return null;
		// }
		// localPlayer = ((GameObject) PhotonNetwork.LocalPlayer.TagObject).transform; 
		Destroy(this);
		yield return null;
	}


	

	private void OnMouseEnter() {
		isMouseOver = true;
		if (!EventSystem.current.IsPointerOverGameObject() && Vector3.Distance(transform.position, localPlayer.position) < 1.5f){
			OnMouseEnterEvent.Invoke();
		}
	}

	private void OnMouseOver() {

	}

	private void OnMouseExit() {
		isMouseOver = false;
		OnMouseExitEvent.Invoke();
	}
}
