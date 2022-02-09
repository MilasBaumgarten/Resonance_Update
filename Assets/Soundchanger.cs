using UnityEngine;
using Unity.Netcode;

public class Soundchanger : MonoBehaviour {
	public AudioClip backGround;
	public AudioSource source;
	public bool repeat;
	private bool trigger = true;

	private void OnTriggerEnter(Collider michi) {
		if (trigger) {
			if (michi.tag.Equals("Player")) {
				NetworkObject netID = michi.GetComponent<NetworkObject>();
				if (netID != null) {
					if (netID.IsLocalPlayer) {
						source.clip = backGround;
						source.Play();
					}

					if (!repeat) trigger = false;
				}
			}
		}
	}
}
