// Author: Noah Stolz
// Workaround for Objects with the ForceModuleBehaviour, that should be disabled at the start of the game

using UnityEngine.Networking;
using UnityEngine;

public class DeactivateObjects : NetworkBehaviour
{
    [SerializeField]
    [Tooltip("The Objects to be disabled")]
    private GameObject[] objects = new GameObject[0];

    public void Disable()
    {

        //EventManager.instance.TriggerEvent("disableWorkaround");

        RpcCmdDisable();

    }

    public void DisableObjects()
    {

        for (int i = 0; i < objects.Length; i++)
        {

            objects[i].SetActive(false);

        }

    }

    
    [Command]
    void CmdDisable()
    {

        RpcCmdDisable();

    }

    [ClientRpc]
    void RpcCmdDisable()
    {

        for(int i = 0; i < objects.Length; i++)
        {
            
            objects[i].SetActive(false);

        }

    }
}