/*
 * Author: Noah Stolz
 * 
 * changes: 
 * Felix Werner (11.10.2018)
 * added gravity: Leon Ullrich (31.10.2018)
 * Allows the player to move
 * 
 * Should be attached to the PlayerPrefab
 * For syncing a NetworkManager is required

 * Changes: Milas Baumgarten
 * - update to Unity 2020.3
 * - implement new Network Code
 */

using Photon.Pun;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]

public class PlayerMovement : MonoBehaviourPun {

	//private Settings playerSettings;

	private float vSpeed = 0;

	[SerializeField]
	private float playerSpeed;
	private Vector3 moveDirection;

	private CharacterController characterController;
	private Animator animator;

	private void Start() {
		// check if is local player
		if (photonView.IsMine == false && PhotonNetwork.IsConnected == true) {
			enabled = false;
		}

		characterController = GetComponent<CharacterController>();
		animator = GetComponent<PlayerManager>().animator;
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
		if (animator) {
			animator.SetFloat("horizontalSpeed", inputDir.x);
			animator.SetFloat("verticalSpeed", inputDir.z);
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
