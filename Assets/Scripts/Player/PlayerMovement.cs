// Author: Noah Stolz
// changes: Felix Werner (11.10.2018)
// added gravity: Leon Ullrich (31.10.2018)
// Allows the player to move
// Should be attached to the PlayerPrefab
// For syncing a NetworkManager is required

using UnityEngine;
using Unity.Netcode.Samples;

[RequireComponent(typeof(ClientNetworkTransform), typeof(CharacterController))]

public class PlayerMovement : MonoBehaviour {

	private Settings playerSettings;

	private float vSpeed = 0;

	private float playerSpeed;
	private Vector3 moveDirection;

	//private Vector3 moveDirection;
	[SerializeField]
	private Animator anim;

	[Tooltip("CharacterController attached to the prefab")]
	private CharacterController characterController;

	private void Start() {
		characterController = GetComponent<CharacterController>();
		if (!anim) anim = transform.GetComponentInChildren<Animator>();

		playerSettings = GameManager.instance.settings;

		// use move speed from settings
		playerSpeed = playerSettings.moveSpeed;
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
