using Photon.Pun;
using UnityEngine;

/**
 * Author: Leon Ullrich
 * - Quits the game
 */

public class ExitGame : MonoBehaviour {

    public void CloseGame() {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            PhotonNetwork.Disconnect();
            Application.Quit();
        #endif
    }

    public void Exit() {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}
