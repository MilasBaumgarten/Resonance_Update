using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using System;

public class ArmTool : MonoBehaviourPun {
	[SerializeField]
	private BoneOverride[] boneOverridesCatriona;
	[SerializeField]
	private BoneOverride[] boneOverridesRobert;
	private BoneOverride[] boneOverrides;

	[SerializeField]
	private StateMachine stateMachine;
	
	[HideInInspector]
	public bool armUp = false;

	[SerializeField]
	private ArmToolModule[] equipped;
	[SerializeField]
	private int selected = 0;
	[SerializeField]
	private Image toolIcon;
	[SerializeField]
	private Transform modules;

	[SerializeField]
	private InputSettings input;
	[SerializeField]
	private Settings settings;

	private int layerMask = ~(1 << 9);  // TODO: DAFUQ
	[SerializeField]
	private Transform cam;

	void Start() {
		if (photonView.IsMine == false && PhotonNetwork.IsConnected == true) {
			enabled = false;
		}

		ChangeIconColor();

		// setup Bone Overrides
		string nickname = CharacterEnum.CATRIONA.ToString();
		try {
			nickname = photonView.Owner.NickName;
		} catch (Exception e) {
			Debug.Log("Playing in Offline Mode and owner was not found fast enough.\n" + e.Message);
		}

		if (nickname.Equals(CharacterEnum.CATRIONA.ToString())) {
			boneOverrides = boneOverridesCatriona;
		} else if (nickname.Equals(CharacterEnum.ROBERT.ToString())) {
			boneOverrides = boneOverridesRobert;
		} else {
			Debug.Log("<Color=Red><a>Player</a></Color> nickname: " + nickname + " is unknown.");
		}
	}

	void Update() {
		if (!photonView.IsMine) {
			return;
		}

		if (Input.GetKeyDown(input.armTool)) {

			GameObject interactTarget;
			if (Physics.Raycast(cam.position, cam.forward, out RaycastHit hit, settings.forceToolMaxDist, layerMask)) {
				interactTarget = hit.transform.gameObject;
				if (interactTarget.GetComponent<Interactable>()) {
					photonView.RPC("InteractRpc", RpcTarget.All, interactTarget.GetPhotonView().ViewID);
				}
				if (equipped[selected]) {
					equipped[selected].Function(interactTarget);
				}
			} else {
				if (equipped[selected]) {
					equipped[selected].Function(null);
				}
			}
		}

		if (Input.GetKeyDown(KeyCode.Alpha1)) {
			selected = 0;
			ChangeIconColor();
		}
		if (Input.GetKeyDown(KeyCode.Alpha2)) {
			selected = 1;
			ChangeIconColor();
		}
		if (Input.GetKeyDown(KeyCode.Alpha3)) {
			selected = 2;
			ChangeIconColor();
		}
		if (Input.GetKeyDown(KeyCode.Alpha4)) {
			selected = 3;
			ChangeIconColor();
		}

		if (selected == 0) {
			armUp = false;
			boneOverrides[0].enabled = false;
			boneOverrides[1].enabled = false;
		} else {
			armUp = true;
			boneOverrides[0].enabled = true;
			boneOverrides[1].enabled = true;
		}
	}

	public void DeselectTool() {
		selected = 0;
		ChangeIconColor();
		armUp = false;
	}

	private void ChangeIconColor() {
		if (equipped[selected]) {
			toolIcon.color = equipped[selected].color;
		} else {
			toolIcon.color = Color.white;
		}
	}

	[PunRPC]
	public void InteractRpc(int targetId) {
		PhotonView target = PhotonNetwork.GetPhotonView(targetId);
		target.GetComponent<Interactable>().Interact(this);
	}

	[PunRPC]
	public void InteractModuleRpc(int targetId) {
		// send the server a command telling it the player is intending to interact with something
		PhotonView target = PhotonNetwork.GetPhotonView(targetId);
		target.GetComponent<ArmToolModuleBehaviour>().Interact(equipped[selected]);
	}
}
