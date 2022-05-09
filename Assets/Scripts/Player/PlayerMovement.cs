// Author: Noah Stolz
// changes: Felix Werner (11.10.2018)
// added gravity: Leon Ullrich (31.10.2018)
// Allows the player to move
// Should be attached to the PlayerPrefab
// For syncing a NetworkManager is required

using Photon.Pun;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]

public class PlayerMovement : MonoBehaviourPun {

	//private Settings playerSettings;

	private float vSpeed = 0;

	[SerializeField]
	private float playerSpeed;
	private Vector3 moveDirection;

	[SerializeField]
	private Animator catrionaAnimator;
	[SerializeField]
	private Animator robertAnimator;
	private Animator anim;

	[Tooltip("CharacterController attached to the prefab")]
	private CharacterController characterController;

	private void Start() {
		// check if is local player
		if (photonView.IsMine == false && PhotonNetwork.IsConnected == true) {
			enabled = false;
		}

		characterController = GetComponent<CharacterController>();

		string nickname = photonView.Owner.NickName;
		if (nickname.Equals(CharacterEnum.CATRIONA.ToString())) {
			anim = catrionaAnimator;
		} else if (nickname.Equals(CharacterEnum.ROBERT.ToString())) {
			anim = robertAnimator;
		} else {
			Debug.LogWarning("<Color=Red><a>Player</a></Color> nickname: " + nickname + " is unknown.");
		}
	}

	private void OnDisable() {
		UpdateAnimation(Vector3.zero);
	}

	void Update() {
		moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical")); // Get input as a Vector3
																								 // Input used to move the player in different directions

		UpdateAnimation(moveDirection);
		if (moveDirection.magnitude > 0f) {
			moveDirection *= playerSpeed * Time.deltaTime;
			moveDirection = transform.TransformDirection(moveDirection); // Transform the Vector to Worldspace so it depends on the local Z-Axis
			characterController.Move(moveDirection);   // Move the Player
		}
	}

	void UpdateAnimation(Vector3 inputDir) {
		// Animator stuff
		if (anim) {
			anim.SetFloat("horizontalSpeed", inputDir.x);
			anim.SetFloat("verticalSpeed", inputDir.z);
		}
	}

	void FixedUpdate() {
		if (!characterController.isGrounded) {
			vSpeed += Physics.gravity.y * Time.deltaTime;
			moveDirection.y = vSpeed;
			characterController.Move(moveDirection * Time.deltaTime);
		} else {
			if (vSpeed != 0)
				vSpeed = 0;
		}
	}
}
