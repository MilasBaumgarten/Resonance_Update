using UnityEngine;
using Photon.Pun;

public class OpenCanvas : MonoBehaviourPun {
	[SerializeField]
	private InputSettings input;

	private LogbookManager logbook;

	void Start() {
		// check if is local player
		if (photonView.IsMine == false && PhotonNetwork.IsConnected == true) {
			enabled = false;
		}

		logbook = GetComponent<PlayerManager>().logbook;
	}

	void Update() {
		if (Input.GetKeyDown(input.logbook)) {
			if (logbook.isActive) {
				logbook.CloseLogbook();
			} else {
				logbook.OpenLogbook();
			}
		}

		if (Input.GetKeyDown(input.closeLogbook)) {
			if (logbook.isActive) {
				logbook.CloseLogbook();
			}
		}
	}
}

