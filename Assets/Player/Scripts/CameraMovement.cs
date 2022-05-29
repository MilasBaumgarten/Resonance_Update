// Author: Nico Mahler
// changes: Milas Baumgarten (07.10.2018), Felix Werner (11.10.2018)
// Clamps the Mouse Input-Axis
// Should be attached to the PlayerPrefab
// Changes: Noah Stolz 
// Slow mouse when using forcetool

using Photon.Pun;
using UnityEngine;

public class CameraMovement : MonoBehaviourPun {
	[SerializeField]
	private Transform playerCamera;

	[SerializeField]
	private Settings playerSettings;

	float hSens;
	float vSens;
	float hClamp;
	float vClamp;

	float minAngle;
	float maxAngle;

	private float rotX;
	private float rotY;

	private bool cameraIsInPlayMode = true;
	private bool clampedCameraMotion;

	private void Awake() {
		// check if is local player
		if (photonView.IsMine == false && PhotonNetwork.IsConnected == true) {
			enabled = false;
			playerCamera.gameObject.SetActive(false);
		}

		Cursor.lockState = CursorLockMode.Locked;

		UpdateValues();
	}

	void Update() {
		UpdateCameraMovement();
	}

	private void UpdateCameraMovement() {
		// Clamp the Movement with the public Variables of the maximum movement across the horizontal / vertical axis
		if (clampedCameraMotion) {
			rotX += Mathf.Clamp(Input.GetAxis("Mouse X") * hSens * Time.deltaTime, -hClamp, hClamp);
			rotY = Mathf.Clamp(rotY - Mathf.Clamp(Input.GetAxis("Mouse Y") * vSens * Time.deltaTime, -vClamp, vClamp), minAngle, maxAngle);
		} else {
			rotX += Input.GetAxis("Mouse X") * hSens * Time.deltaTime;
			rotY = Mathf.Clamp(rotY - Input.GetAxis("Mouse Y") * vSens * Time.deltaTime, minAngle, maxAngle);
		}

		// Rotate the player
		transform.localEulerAngles = new Vector3(0, rotX, 0);
		// Tilt the Camera
		playerCamera.transform.localEulerAngles = new Vector3(rotY, 0, 0);
	}

	/// <summary>
	/// Updates the settings for the camera controls.
	/// Should be called after changes to the camera controls are wanted (e.g. in the options menu)
	/// </summary>
	public void UpdateValues() {
		// get values
		if (cameraIsInPlayMode) {
			hSens = playerSettings.horizontalSensitivity;
			vSens = playerSettings.verticalSensitivity;
			hClamp = playerSettings.horizontalClamp;
			vClamp = playerSettings.verticalClamp;

			clampedCameraMotion = false;
		} else {
			hSens = playerSettings.horizontalForceSensitivity;
			vSens = playerSettings.verticalForceSensitivity;
			hClamp = playerSettings.horizontalForceClamp;
			vClamp = playerSettings.verticalForceClamp;

			clampedCameraMotion = true;
		}

		minAngle = playerSettings.minAngle;
		maxAngle = playerSettings.maxAngle;
	}

	/// <summary>
	/// switch between motion modes (i.e. switch camera max speed)
	/// </summary>
	/// <param name="state"></param> true: camera is in play mode, false: camera is in cinematic mode
	public void SetCameraFree(bool state) {
		if (state) {
			cameraIsInPlayMode = true;
		} else {
			cameraIsInPlayMode = false;
		}

		UpdateValues();
	}
}
