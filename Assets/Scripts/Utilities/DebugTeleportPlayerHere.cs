using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.Utilities;

[Sirenix.OdinInspector.TypeInfoBox("This script can be used to teleport the first object found with tag \"Player\" to the transform position of the object it is attached to.")]
public class DebugTeleportPlayerHere : MonoBehaviour
{
    private static PlayerManager playerObject;


    /// <summary>
    /// This function is called when the object becomes enabled and active.
    /// </summary>
    void OnEnable()
    {
        if(!Application.isEditor){
            Destroy(gameObject);
        }

    }

    void Start()
    {
        playerObject = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>();
        if(!playerObject/*  || myIndex < 0 */){
            Destroy(gameObject);
        }
    }


    [ContextMenu("Teleport Player Here")]
    [Sirenix.OdinInspector.Button]
    private void TeleportPlayerHere(){
        playerObject.transform.position = transform.position;
    }


    void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta * 0.5f;
        Gizmos.DrawCube(transform.position, Vector3.one * 0.5f);
    }
}
