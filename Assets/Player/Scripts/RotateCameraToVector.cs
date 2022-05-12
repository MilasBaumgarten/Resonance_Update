// Author: Noah Stolz
// Used to rotate the camera towards a specified transform
// Has to be attached to the player camera
using System.Collections;
using UnityEngine;

enum CameraState {
	IDLE,
	MOVE_IN,
	BACK
}

public class RotateCameraToVector : MonoBehaviour {

	[SerializeField]
	[Tooltip("The transfrom the cam is rotated to")]
	private Transform targetTransform;

	[SerializeField]
	private PlayerManager playerManager;

	[SerializeField]
	[Tooltip("How fast the cam should rotate")]
	private float speed;

	[SerializeField]
	[Tooltip("How long the rotation should last")]
	private int duration;

	private Quaternion lookRotation;
	private Quaternion startRotation;

	private CameraState state;

	void Start() {
		speed /= duration;
		targetTransform = playerManager.logbook.transform;
	}

	public void RotateCam(bool back) {
		if (!back) {
			startRotation = transform.rotation;
			state = CameraState.MOVE_IN;
		} else {
			state = CameraState.BACK;
			lookRotation = startRotation;

			StartCoroutine("SetCameraToIdle");
		}
	}

	private void Update() {
		if (state == CameraState.IDLE) {
			return;
		}

		if (state == CameraState.MOVE_IN) {
			//find the vector pointing to the target
			Vector3 direction = (targetTransform.position - transform.position).normalized;

			//create the rotation
			lookRotation = Quaternion.LookRotation(direction);
		}

		transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * speed);
	}

	IEnumerator SetCameraToIdle() {
		yield return new WaitForSeconds(duration * Time.fixedDeltaTime);

		state = CameraState.IDLE;
	}
}
