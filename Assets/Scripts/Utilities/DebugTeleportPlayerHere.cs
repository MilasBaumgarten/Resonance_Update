using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.Utilities;

public class DebugTeleportPlayerHere : MonoBehaviour
{
    // private static bool isInitialized = false;
    // private static int currentTeleportPoint = 0;
    // private static DebugTeleportPlayerHere[] debugTeleportPlayers;

    private static PlayerManager playerObject;

    // public int myIndex = -1;

    /// <summary>
    /// This function is called when the object becomes enabled and active.
    /// </summary>
    void OnEnable()
    {
        if(!Application.isEditor){
            Destroy(gameObject);
        }

        // if(!isInitialized){
        //     isInitialized = true;
        //     debugTeleportPlayers = FindObjectsOfType<DebugTeleportPlayerHere>();
        //     playerObject = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>();
        //     for(int i = 0; i < debugTeleportPlayers.Length; i++){
        //         debugTeleportPlayers[i].myIndex = i;
        //     }
        // }
    }

    void Start()
    {
        playerObject = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>();
        if(!playerObject/*  || myIndex < 0 */){
            Destroy(gameObject);
        }
    }

    // /// <summary>
    // /// LateUpdate is called every frame, if the Behaviour is enabled.
    // /// It is called after all Update functions have been called.
    // /// </summary>
    // void LateUpdate()
    // {
    //     if(currentTeleportPoint == myIndex){
    //         if(Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.LeftShift)){
    //             // Debug.Log(currentTeleportPoint + " == " + myIndex);
    //             if(Input.GetKeyUp(KeyCode.LeftArrow)){
    //                 Debug.Log(Time.frameCount);
    //                 Debug.Log(myIndex + " | Debug teleport player next index " + currentTeleportPoint + ": " + debugTeleportPlayers[currentTeleportPoint].name, debugTeleportPlayers[currentTeleportPoint]);
    //                 currentTeleportPoint = (currentTeleportPoint - 1 + debugTeleportPlayers.Length) % debugTeleportPlayers.Length;
    //                 // (0 - 1 + 12) % 12 = 11
    //                 // ((1 - 1) + 12) % 12 = 0 
    //             } else 
    //             if(Input.GetKeyUp(KeyCode.RightArrow)){
    //                 Debug.Log(Time.frameCount);
    //                 Debug.Log(myIndex + " | Debug teleport player previous index " + currentTeleportPoint + ": " + debugTeleportPlayers[currentTeleportPoint].name, debugTeleportPlayers[currentTeleportPoint]);
    //                 currentTeleportPoint = (currentTeleportPoint + 1) % debugTeleportPlayers.Length;
    //             } else 
    //             if(Input.GetKeyUp(KeyCode.UpArrow)){
    //                 Debug.Log(Time.frameCount);
    //                 Debug.Log(myIndex + " | Player debug teleported to index " + currentTeleportPoint + ": " + debugTeleportPlayers[currentTeleportPoint].name, debugTeleportPlayers[currentTeleportPoint]);
    //                 TeleportPlayerTo(debugTeleportPlayers[currentTeleportPoint].transform);
    //             }
    //         }
    //     }
    // }

    [ContextMenu("Teleport Player Here")]
    [Sirenix.OdinInspector.Button]
    private void TeleportPlayerHere(){
        playerObject.transform.position = transform.position;
    }

    // private void TeleportPlayerTo(Transform point){
    //     playerObject.transform.position = point.position;
    // }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta * 0.5f;
        Gizmos.DrawCube(transform.position, Vector3.one * 0.5f);
    }
}
