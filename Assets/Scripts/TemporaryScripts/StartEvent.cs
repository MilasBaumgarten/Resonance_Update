// Author: Noah Stolz
// Used to start an InvokeWithDelay event with networking

using UnityEngine;
using UnityEngine.Networking;

public class StartEvent : NetworkBehaviour {

    [SerializeField]
    private GameObject obj;
    
    public void CallEvent()
    {


        CmdCallEvent(obj);

    }

    [Command]
    void CmdCallEvent(GameObject obj)
    {

        RpcCallEvent(obj);

    }

    [ClientRpc]
    void RpcCallEvent(GameObject obj)
    {

        obj.GetComponent<InvokeWithDelay>().OnButtonClicked();

    }
}
