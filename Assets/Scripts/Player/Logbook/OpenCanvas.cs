using UnityEngine;
using Unity.Netcode;

public class OpenCanvas : NetworkBehaviour {
	public LogbookManager logbook;
	public InputSettings input;

	void Start() {
		if (!IsLocalPlayer)
			this.enabled = false;
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

