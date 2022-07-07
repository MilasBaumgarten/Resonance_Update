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
	private int selected = 0;
	[SerializeField]
	private Image toolIcon;
	[SerializeField]
	private Transform modules;

	[SerializeField]
	private InputSettings input;
	[SerializeField]
	private Settings settings;

	// use every layer except layer number 9 aka player layer
	private int layerMask = ~(1 << 9);
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
		bool armtoolInUse = false;
		if (equipped[selected] && equipped[selected].type == ToolType.EXTINGUISHER) {
			armtoolInUse = Input.GetKey(input.armTool);
		} else {
			armtoolInUse = Input.GetKeyDown(input.armTool);
		}

		if (armtoolInUse) {
			GameObject interactTarget;
			if (Physics.Raycast(cam.position, cam.forward, out RaycastHit hit, settings.forceToolMaxDist, layerMask)) {
				interactTarget = hit.transform.gameObject;
				if (interactTarget.GetComponent<Interactable>()) {
					photonView.RPC("InteractRpc", RpcTarget.All, PlayerManager.localPlayerInstance.GetPhotonView().ViewID, interactTarget.GetPhotonView().ViewID);
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

	public int GetSelected() {
		return selected;
	}

	[PunRPC]
	public void InteractRpc(int sourceId, int targetId) {
		PhotonView source = PhotonNetwork.GetPhotonView(sourceId);
		PhotonView target = PhotonNetwork.GetPhotonView(targetId);

		target.GetComponent<Interactable>().Interact(source.GetComponent<ArmTool>());
	}

	[PunRPC]
	public void InteractModuleRpc(int sourceId, int selectedModule, int targetId) {
		// send the server a command telling it the player is intending to interact with something
		PhotonView source = PhotonNetwork.GetPhotonView(sourceId);
		PhotonView target = PhotonNetwork.GetPhotonView(targetId);

		ArmToolModuleBehaviour targetBehaviour = target.GetComponent<ArmToolModuleBehaviour>();
		ArmTool sourceArmTool = source.GetComponent<ArmTool>();
		targetBehaviour.Interact(sourceArmTool.equipped[selectedModule]);
	}
}
