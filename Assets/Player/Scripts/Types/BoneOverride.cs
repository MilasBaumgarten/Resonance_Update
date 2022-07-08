using Photon.Pun;
using UnityEngine;

public class BoneOverride : MonoBehaviourPun {
	[SerializeField]
	private bool aimAtTarget;
	public Transform LookAtTarget;
	public Vector3 Offset;

	private bool isActive = false;

	[SerializeField]
	private Animator anim;

	void LateUpdate() {
		if (isActive) {
			if (aimAtTarget) {
				transform.LookAt(LookAtTarget.position);
			}

			transform.rotation = transform.rotation * Quaternion.Euler(Offset);
		}
	}

	[PunRPC]
	public void SetActivationStateRPC(bool state) {
		isActive = state;
	}
}
