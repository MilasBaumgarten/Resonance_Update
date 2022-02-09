using UnityEngine;
using UnityEngine.Networking;

/**
 * Author: Leon Ullrich
 * - Allows the player to interact with any object which has the Interactable-Component attached to it
 * - handles (optional) synchronization
 * - put this on the player
 */

public class Interact : NetworkBehaviour {

    public InputSettings input;
    public Settings settings;

    // ignore player-layer (the 9th layer)
    private int layerMask = ~(1 << 9);

    [SerializeField]
    private Transform cam;

    private NetworkIdentity objectNetID;

    // Use this for initialization
    void Start () {
        cam = transform.GetComponentInChildren<Camera>().transform;
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(input.interact)) {
            RaycastHit hit;
            if (Physics.Raycast(cam.position, cam.forward, out hit, settings.maxDist, layerMask)) {

                hit.transform.SendMessage("InteractedWith", gameObject);

            }
        }
	}

    public void SyncEvent(GameObject connection) {
        // call the command
        CmdInvokeEvent(connection);

    }

    public void SyncTriggerEvent(GameObject connection) {
        CmdInvokeTriggerEvent(connection);
    }

    [Command]
    void CmdInvokeEvent(GameObject objectID) {
        // Get the NetworkIdentity of the calling player
        objectNetID = objectID.GetComponent<NetworkIdentity>();
        // assign authority of this object to calling player
        objectNetID.AssignClientAuthority(connectionToClient);
        // Client can now call the RPC with his authority
        RpcInvokeEvent(objectID);
        // Remove authority of this object from the calling player
        objectNetID.RemoveClientAuthority(connectionToClient);
    }

    [Command]
    void CmdInvokeTriggerEvent(GameObject objectID) {
        objectNetID = objectID.GetComponent<NetworkIdentity>();
        objectNetID.AssignClientAuthority(connectionToClient);
        RpcInvokeTriggerEvent(objectID);
        objectNetID.RemoveClientAuthority(connectionToClient);
    }

    [ClientRpc]
    void RpcInvokeEvent(GameObject objectID) {
        // call InvokeEvent-Method on object
        objectID.GetComponent<InteractableObject>().InvokeEvent();
    }

    [ClientRpc]
    void RpcInvokeTriggerEvent(GameObject objectID) {
        objectID.GetComponent<ColliderTrigger>().Trigger();
    }
}
