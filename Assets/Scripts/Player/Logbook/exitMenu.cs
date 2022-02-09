using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

/**
 * Author: Leon Ullrich
 * - Disconnects the player from the server/stops the server and loads the MainMenu-Scene
 */

public class exitMenu : NetworkBehaviour {

    private List<PlayerController> playerControllers;
    private NetworkIdentity ownPlayerID;

	public void onExitMenuButton()
    {
        
        //find own player

        playerControllers = NetworkManager.singleton.client.connection.playerControllers;
        foreach (PlayerController controller in playerControllers) {
            if (controller.IsValid) {
                if (controller.gameObject.GetComponent<NetworkIdentity>().isLocalPlayer) {
                    ownPlayerID = controller.gameObject.GetComponent<NetworkIdentity>();
                    break;
                }
            }
        }

        // check if player is host
        if (ownPlayerID.isServer) {
            NetworkManager.singleton.StopHost();
        } else {
            NetworkManager.singleton.StopClient();
        }

        // load new scene
        SceneManager.LoadScene("MainMenu");

    }
}
