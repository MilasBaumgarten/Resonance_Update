// Author: Noah Stolz
// Used to start an InvokeWithDelay event with networking

using UnityEngine;
using Unity.Netcode;

public class StartEvent : NetworkBehaviour {
	[SerializeField]
	private GameObject obj;

	public void CallEvent() {
		//CallEventServerRpc(obj);
	}

	//[ServerRpc]
	//void CallEventServerRpc(GameObject obj) {
	//	CallEventClientRpc(obj);
	//}

	//[ClientRpc]
	//void CallEventClientRpc(GameObject obj) {
	//	obj.GetComponent<InvokeWithDelay>().OnButtonClicked();
	//}
}
