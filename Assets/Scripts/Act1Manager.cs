// Author: Noah Stolz
// Provides some functionality that is only needed in act1

using UnityEngine;
using UnityEngine.Networking;

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

			CmdSetPlayerReady();

		}

	}

	[Command]
	public void CmdSetPlayerReady() {

		RpcSetPlayerReady();

	}

	[ClientRpc]
	void RpcSetPlayerReady() {

		doorColl1.enabled = true;
		doorColl2.enabled = true;

	}
}
