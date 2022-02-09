// Author: Noah Stolz
// Provides some functionality that is only needed in act1

using UnityEngine;
using Unity.Netcode;

public class Act1Manager : NetworkBehaviour {

	[SerializeField]
	[Tooltip("How many times should the player interact with something before they can move on")]
	private int targetNummberOfInteractions;

	[SerializeField]
	private Collider doorColl1;
	[SerializeField]
	private Collider doorColl2;

	private int nummberOfInteractions = 0;

	// Increase the nummber of interactions whenever one of the players collect an Object
	public void increaseNummberOfInteractions() {
		nummberOfInteractions++;

		if (nummberOfInteractions >= targetNummberOfInteractions) {
			SetPlayerReadyServerRpc();
		}
	}

	[ServerRpc]
	public void SetPlayerReadyServerRpc() {
		SetPlayerReadyClientRpc();
	}

	[ClientRpc]
	void SetPlayerReadyClientRpc() {
		doorColl1.enabled = true;
		doorColl2.enabled = true;
	}
}
