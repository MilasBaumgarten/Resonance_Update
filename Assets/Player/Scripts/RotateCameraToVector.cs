// Author: Noah Stolz
// Used to rotate the camera towards a specified transform
// Has to be attached to the player camera
using System.Collections;
using UnityEngine;

public enum CameraState {
	IDLE,
	MOVE_IN,
	BACK
}

public class RotateCameraToVector : MonoBehaviour {
	[SerializeField]
	private Transform playerHead;
	[SerializeField]
	private CameraMovement cameraMovement;

	[SerializeField]
	[Tooltip("The transfrom the cam is rotated to")]
	private Transform targetTransform;

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

	public CameraState state { get; private set; }

	private Vector3 direction;

	public void Setup(Transform playerHead,
					  CameraMovement cameraMovement) {
		this.playerHead = playerHead;
		this.cameraMovement = cameraMovement;
	}

	void Start() {
		move_in_speed = speed / move_in_duration;
		move_out_speed = speed / move_out_duration;
	}

	public void RotateCam(bool back) {
		if (!back) {
			startRotation = playerHead.transform.rotation;
			state = CameraState.MOVE_IN;
			cameraMovement.SetCameraFree(false);

			//find the vector pointing to the target
			direction = (targetTransform.position - playerHead.transform.position).normalized;

			//create the rotation
			lookRotation = Quaternion.LookRotation(direction);
		} else {
			state = CameraState.BACK;
			lookRotation = startRotation;

			StartCoroutine(SetCameraToIdle());
		}
	}

	private void FixedUpdate() {
		if (state == CameraState.IDLE) {
			return;
		}

		if (state == CameraState.MOVE_IN) {
			playerHead.transform.rotation = Quaternion.Slerp(playerHead.transform.rotation, lookRotation, Time.fixedDeltaTime * move_in_speed);
		} 
		else if (state == CameraState.BACK) {
			playerHead.transform.rotation = Quaternion.Slerp(playerHead.transform.rotation, lookRotation, Time.fixedDeltaTime * move_out_speed);
		}
	}

	IEnumerator SetCameraToIdle() {
		yield return new WaitUntil(() => Quaternion.Angle(playerHead.transform.rotation, lookRotation) <= 0.1f);

		state = CameraState.IDLE;
		cameraMovement.SetCameraFree(true);
	}
}
