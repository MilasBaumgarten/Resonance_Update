using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class ArmTool : MonoBehaviourPun {
	[HideInInspector]
	public bool armUp = false;

	[SerializeField]
	private StateMachine stateMachine;
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
	private Transform cam;

	void Start() {
		if (photonView.IsMine == false && PhotonNetwork.IsConnected == true) {
			enabled = false;
		}

		cam = transform.GetComponentInChildren<Camera>().transform;
		equipped = new ArmToolModule[4];
		foreach (Transform child in modules) {
			equipped[child.GetSiblingIndex()] = child.GetComponent<ArmToolModule>();
		}
		ChangeIconColor();
	}

	void Update() {
		if (!photonView.IsMine) {
			return;
		}

		if (Input.GetKeyDown(input.armTool)) {

			GameObject interactTarget;
			if (Physics.Raycast(cam.position, cam.forward, out RaycastHit hit, settings.maxDist, layerMask)) {
				interactTarget = hit.transform.gameObject;
				if (interactTarget.GetComponent<Interactable>()) {
					photonView.RPC("InteractRpc", RpcTarget.All, interactTarget);
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

		if (selected < 3) {
			armUp = true;
		} else {
			armUp = false;
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
	public void InteractRpc(GameObject target) {
		Debug.Log(string.Format("RPC: 'InteractRpc' Parameter: {0} PhotonView: {1}", target.name, photonView));
		target.GetComponent<Interactable>().Interact(this);
	}

	[PunRPC]
	public void InteractModuleRpc(GameObject target) {
		Debug.Log(target);
		Debug.Log(string.Format("RPC: 'InteractModuleRpc' Parameter: {0} PhotonView: {1}", target.name, photonView));
		// send the server a command telling it the player is intending to interact with something
		target.GetComponent<ArmToolModuleBehaviour>().Interact(equipped[selected]);
	}
}
