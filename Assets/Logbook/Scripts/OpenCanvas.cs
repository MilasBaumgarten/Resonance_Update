using UnityEngine;
using Photon.Pun;

public class OpenCanvas : MonoBehaviourPun {
	[SerializeField]
	private InputSettings input;

	[SerializeField]
	private LogbookManager logbookCatriona;
	[SerializeField]
	private LogbookManager logbookRobert;

	private LogbookManager logbook;

	void Start() {
		// check if is local player
		if (photonView.IsMine == false && PhotonNetwork.IsConnected == true) {
			enabled = false;
		}
		logbook = GetComponent<PlayerManager>().logbook;
	}

	// Update is called once per frame
	void Update() {
		if (Input.GetKeyDown(input.logbook)) {
			if (logbook.isActive) {
				logbook.DisablePanel();
			} else {
				logbook.openLogbook();
			}
		}

		if (Input.GetKeyDown(input.closeLogbook)) {
			if (logbook.isActive) {
				logbook.DisablePanel();
			}
		}
	}
}

