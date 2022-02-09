// Author: Noah Stolz
// Used to set the position of the Players

using UnityEngine;
using UnityEngine.Networking;

public class SetPlayerPositions : NetworkBehaviour {

    [SerializeField]
    [Tooltip("The position player1 should be set to")]
    private Transform player1Pos;

    [SerializeField]
    [Tooltip("The position player2 should be set to")]
    private Transform player2Pos;

    public void triggerEvent()
    {

        //EventManager.instance.TriggerEvent("setPlayerPosition");
        CmdSetPlayerPosition();

    }

    /// <summary>
    /// Used to set the player positions with the BothPlayersEnter script
    /// </summary>
    public void SetPlayerPositionByTrigger()
    {
        
        BothPlayersEnter.player1.transform.position = player1Pos.position;
        BothPlayersEnter.player2.transform.position = player2Pos.position;
        
    }

    [Command]
    void CmdSetPlayerPosition()
    {
        
        RpcSetPlayerPosition();
    }

    [ClientRpc]
    void RpcSetPlayerPosition()
    {

        BothPlayersEnter.player1.transform.position = player1Pos.position;
        BothPlayersEnter.player2.transform.position = player2Pos.position;

    }
}
