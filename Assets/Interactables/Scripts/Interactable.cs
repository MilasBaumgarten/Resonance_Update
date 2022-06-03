using Photon.Pun;
using UnityEngine;

[RequireComponent(typeof(PhotonView))]
public abstract class Interactable : MonoBehaviour {
	[SerializeField]
	protected bool onlyExecuteLocally = false;

	public abstract void Interact(ArmTool armTool);
}
