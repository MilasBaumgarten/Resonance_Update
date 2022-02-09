using UnityEngine;
using UnityEngine.Networking;

public class SpawnSetup : NetworkBehaviour {

    [SerializeField]
    Behaviour[] ToDisable;
    GameObject sceneCam;

    void Start() {
        if (!isLocalPlayer) {
            foreach (Behaviour component in ToDisable) {
                component.enabled = false;
            }
        }
    }
}
