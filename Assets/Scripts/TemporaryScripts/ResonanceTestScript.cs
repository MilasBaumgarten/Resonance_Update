using UnityEngine;
using Photon.Pun;

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
public class ResonanceTestScript : MonoBehaviourPun {

    // This bool only gets true, if the player collided with the resonance. The code is in the ResonanceScript.
    [HideInInspector] public bool activated;

	[HideInInspector] public bool pressedJ;
	[HideInInspector] public bool pressedN;

    // This update checks for the player input, and changes the bools (the decisions).
    private void Update() {
        if (activated && photonView.IsMine) {
            if (!PhotonNetwork.IsMasterClient) {
                if (Input.GetKeyDown(KeyCode.J)) {
                    Debug.Log("Client pressed J");
                    //PressedJServerRpc();
                }
                if (Input.GetKeyDown(KeyCode.N)) {
                    Debug.Log("Client pressed N");
                    //PressedNServerRpc();
                }
            }
            else if (PhotonNetwork.IsMasterClient) {
                if (Input.GetKeyDown(KeyCode.J)) {
                    Debug.Log("Server pressed J");
                    //PressedJClientRpc();
                }

                if (Input.GetKeyDown(KeyCode.N)) {
                    Debug.Log("Server pressed N");
                    //PressedNClientRpc();
                }
            }
        }
    }

    //[ServerRpc]
    //private void PressedJServerRpc() {
    //    PressedJClientRpc();
    //}

    //[ServerRpc]
    //private void PressedNServerRpc() {
    //    PressedNClientRpc();
    //}

    //[ClientRpc]
    //private void PressedJClientRpc() {
    //    pressedJ = true;
    //}
    
    //[ClientRpc]
    //private void PressedNClientRpc() {
    //    pressedN = true;
    //}
}
