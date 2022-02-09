using System.Collections;
using System.Collections.Generic;
//using AuraAPI;
using UnityEngine;
using UnityEngine.Networking;

public class AuraLightActivator : NetworkBehaviour {
	/* commented until auralight is reimported again - by manuel
	private AuraLight light;
	private AuraVolume volume;

	void Start () {
		StartCoroutine(ActivateOnConnect());
	}

	IEnumerator ActivateOnConnect() {
		yield return new WaitUntil(() => NetworkManager.singleton.client.isConnected);
		light = GetComponent<AuraLight>();
		
		if(light)
			light.enabled = true;
		
		volume = GetComponent<AuraVolume>();
		
		if(volume)
			volume.enabled = true;
	}
	*/
}
