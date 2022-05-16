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
	private Transform playerCamera;
	[SerializeField]
	private CameraMovement cameraMovement;

	[SerializeField]
	[Tooltip("The transfrom the cam is rotated to")]
	private Transform targetTransform;

	[SerializeField]
	private PlayerManager playerManager;

	[SerializeField]
	[Tooltip("How fast the cam should rotate")]
	private float speed;
	private float move_in_speed;
	private float move_out_speed;

	[SerializeField]
	[Tooltip("How long the rotation should last")]
	private int move_in_duration;
	[SerializeField]
	[Tooltip("How long the rotation should last")]
	private int move_out_duration;

	private Quaternion lookRotation;
	private Quaternion startRotation;

	private CameraState state;

	void Start() {
		move_in_speed = speed / move_in_duration;
		move_out_speed = speed / move_out_duration;
		targetTransform = playerManager.logbook.transform;
	}

	public void RotateCam(bool back) {
		if (!back) {
			startRotation = transform.rotation;
			state = CameraState.MOVE_IN;
			cameraMovement.SetCameraFree(false);
		} else {
			state = CameraState.BACK;
			lookRotation = startRotation;

			StartCoroutine("SetCameraToIdle");
		}
	}

	// TODO: improve lookRotation accuracy
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

		lookRotation = Quaternion.Euler(lookRotation.eulerAngles.x, transform.rotation.eulerAngles.y, 0);

		if (state == CameraState.MOVE_IN) {
			transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * move_in_speed);
		} 
		else if (state == CameraState.BACK) {
			transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * move_out_speed);
		}
		
	}

	IEnumerator SetCameraToIdle() {
		yield return new WaitForSeconds(move_out_duration * Time.fixedDeltaTime);

		state = CameraState.IDLE;
		cameraMovement.SetCameraFree(true);
	}
}
