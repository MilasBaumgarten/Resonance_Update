using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

//author manuel fischer
// put on item with collider or rigidbody
// optional: put outline script on and activate on mouseenter to have hover effekt, disable on exit
public class OnHoverActive : MonoBehaviour {
	[SerializeField]
	[FormerlySerializedAs("OnMouseEnterResonse")]
	private UnityEvent OnMouseEnterEvent;

	[SerializeField]
	[FormerlySerializedAs("OnMouseExitResonce")]
	[FormerlySerializedAs("OnMouseExitResponse")]
	private UnityEvent OnMouseExitEvent;
	private void OnMouseEnter() {
		if (!EventSystem.current.IsPointerOverGameObject()){
			OnMouseEnterEvent.Invoke();
		}
	}
	private void OnMouseExit() {
		OnMouseExitEvent.Invoke();
	}
}
