using Photon.Pun;
using UnityEngine;

[RequireComponent(typeof(PhotonView))]
public abstract class Interactable : MonoBehaviour {
	[SerializeField]
	protected bool onlyExecuteLocally = false;

	[SerializeField]
	protected GameObject outlineObject;

	protected bool collected = false;

	public abstract void Interact(ArmTool armTool);

	public void SetOutlineVisibility(bool state) {
		if (outlineObject) {
			outlineObject.SetActive(state);
		}
	}

	public bool IsCollected() {
		return collected;
	}
}
