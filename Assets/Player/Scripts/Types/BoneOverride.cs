using UnityEngine;

public class BoneOverride : MonoBehaviour {
	[SerializeField]
	private bool aimAtTarget;
	public Transform LookAtTarget;
	public Vector3 Offset;

	[HideInInspector]
	public bool isActive;

	[SerializeField]
	private Animator anim;

	void LateUpdate() {
		if (aimAtTarget) {
			transform.LookAt(LookAtTarget.position);
		}

		transform.rotation = transform.rotation * Quaternion.Euler(Offset);
	}
}
