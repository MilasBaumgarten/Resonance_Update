using UnityEngine;
using UnityEngine.Networking;

/*
 * by Andre Spittel 15.10.2018
 * ---------------------------------------------------------------------------------------------------------------------
 * A script for the testscene, to make a decision in the resonance.
 * ---------------------------------------------------------------------------------------------------------------------
 * Place this script onto your player prefab.
 *
 * This script is temporary, to test the functionality of the resonances. It tracks if a player made a decision (pressed
 * J or N) and sends these to the server, if its the client. If its the server, It sends the decision to all clients.
 */
public class ResonanceTestScript : NetworkBehaviour {

    // This bool only gets true, if the player collided with the resonance. The code is in the ResonanceScript.
    [HideInInspector] public bool activated;

	[HideInInspector] public bool pressedJ;
	[HideInInspector] public bool pressedN;

    // This update checks for the player input, and changes the bools (the decisions).
    private void Update() {
        if (activated && isLocalPlayer) {
            if (!isServer) {
                if (Input.GetKeyDown(KeyCode.J)) {
                    Debug.Log("Client pressed J");
                    CmdPressedJ();
                }
                if (Input.GetKeyDown(KeyCode.N)) {
                    Debug.Log("Client pressed N");
                    CmdPressedN();
                }
            }
            else if (isServer) {
                if (Input.GetKeyDown(KeyCode.J)) {
                    Debug.Log("Server pressed J");
                    RpcPressedJ();
                }

                if (Input.GetKeyDown(KeyCode.N)) {
                    Debug.Log("Server pressed N");
                    RpcPressedN();
                }
            }
        }
    }

    [Command]
    private void CmdPressedJ() {
        RpcPressedJ();
    }

    [Command]
    private void CmdPressedN() {
        RpcPressedN();
    }

    [ClientRpc]
    private void RpcPressedJ() {
        pressedJ = true;
    }
    
    [ClientRpc]
    private void RpcPressedN() {
        pressedN = true;
    }
}
