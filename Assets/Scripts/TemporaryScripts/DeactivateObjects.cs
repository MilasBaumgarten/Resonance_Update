// Author: Noah Stolz
// Workaround for Objects with the ForceModuleBehaviour, that should be disabled at the start of the game

using Photon.Pun;
using UnityEngine;

// TODO: reenable
public class DeactivateObjects : MonoBehaviourPun {
	[SerializeField]
	[Tooltip("The Objects to be disabled")]
	private GameObject[] objects = new GameObject[0];

	public void Disable() {
		//EventManager.instance.TriggerEvent("disableWorkaround");
		//DisableClientRpc();
	}

	public void DisableObjects() {
		for (int i = 0; i < objects.Length; i++) {
			objects[i].SetActive(false);
		}
	}


	//[ServerRpc]
	//void DisableServerRpc() {
	//	DisableClientRpc();
	//}

	//[ClientRpc]
	//void DisableClientRpc() {
	//	for (int i = 0; i < objects.Length; i++) {
	//		objects[i].SetActive(false);
	//	}
	//}
}