using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;

/**
 * Author: Leon Ullrich
 * - Let this script trigger by the ColliderTrigger-Component on the scene-loader at the end of the game
 * - check "Sync Event" variable in inspector for the ColliderTrigger-component
 * 
 * !*********************************************************************************!
 * 
 * - player NEEDS Interact-component for this to work
 * - SceneLoader NEEDS localPlayerAuthority for this to work
 */

public class LoadCreditSceneForBothPlayers : NetworkBehaviour {

    private List<GameObject> playerControllers;
    //private List<PlayerController> playerControllers;
    private NetworkObject ownPlayerID;
    private NetworkObject otherPlayerID;

    //public void LoadCreditScene() {
    //    StartCoroutine("WaitAndThenLoad");
    //}

    //IEnumerator WaitForClientDisconnect() {

    //    while(NetworkManager.Singleton.numPlayers > 1) {
    //        yield return new WaitForEndOfFrame();
    //    }

    //    NetworkManager.Singleton.StopHost();
    //    // load new scene
    //    SceneManager.LoadScene("CreditsEndgame");
    //}

    //IEnumerator WaitAndThenLoad()
    //{
    //    yield return new WaitForSecondsRealtime(2);
    //    //find own player

    //    playerControllers = NetworkManager.Singleton.client.connection.playerControllers;
    //    foreach (PlayerController controller in playerControllers)
    //    {
    //        if (controller.IsValid)
    //        {
    //            if (controller.gameObject.GetComponent<NetworkIdentity>().isLocalPlayer)
    //            {
    //                ownPlayerID = controller.gameObject.GetComponent<NetworkIdentity>();
    //                break;
    //            }
    //        }
    //    }

    //    // check if player is host
    //    if (!ownPlayerID.isServer)
    //    {
    //        NetworkManager.singleton.StopClient();
    //        // load new scene
    //        SceneManager.LoadScene("CreditsEndgame");
    //    }
    //    else
    //    {
    //        StartCoroutine(WaitForClientDisconnect());
    //    }
    //}
}
