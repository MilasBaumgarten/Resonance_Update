// Author: Noah Stolz
// Used to set the position of the Players

using Photon.Pun;
using UnityEngine;

// TODO: reenable
public class SetPlayerPositions : MonoBehaviourPun {

	[SerializeField]
	[Tooltip("The position player1 should be set to")]
	private Transform player1Pos;

	[SerializeField]
	[Tooltip("The position player2 should be set to")]
	private Transform player2Pos;

	public void triggerEvent() {
		//EventManager.instance.TriggerEvent("setPlayerPosition");
		//SetPlayerPositionServerRpc();
	}

	/// <summary>
	/// Used to set the player positions with the BothPlayersEnter script
	/// </summary>
	public void SetPlayerPositionByTrigger() {
		BothPlayersEnter.player1.transform.position = player1Pos.position;
		BothPlayersEnter.player2.transform.position = player2Pos.position;
	}

	//[ServerRpc]
	//void SetPlayerPositionServerRpc() {
	//	SetPlayerPositionClientRpc();
	//}

	//[ClientRpc]
	//void SetPlayerPositionClientRpc() {
	//	BothPlayersEnter.player1.transform.position = player1Pos.position;
	//	BothPlayersEnter.player2.transform.position = player2Pos.position;
	//}
}
