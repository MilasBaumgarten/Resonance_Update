using UnityEngine;
using Unity.Netcode;

public class SpawnSetup : NetworkBehaviour {

    [SerializeField]
    Behaviour[] ToDisable;

    void Start() {
        if (!IsLocalPlayer) {
            foreach (Behaviour component in ToDisable) {
                component.enabled = false;
            }
        }
    }
}
