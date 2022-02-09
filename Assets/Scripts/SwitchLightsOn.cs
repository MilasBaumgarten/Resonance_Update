using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class SwitchLightsOn : NetworkBehaviour {
    [SerializeField]
    private List<Light> switchableLights = new List<Light>();

    [SerializeField]
    private Color normalLightColor = new Color();

    private NetworkIdentity identity;
    private NetworkIdentity playerIdentity;

    [SyncVar]
    GameObject player;

    void Awake() {
        identity = GetComponent<NetworkIdentity>();
    }

    void Start() {
        foreach(Light light in switchableLights) {
            light.color = new Color(0, 0, 0, 0);
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.tag.Equals("Player")) {
            player = other.gameObject;
            CmdTurnLightOn(player);
        }
    }

    [Command]
    void CmdTurnLightOn(GameObject player) {
        playerIdentity = player.GetComponent<NetworkIdentity>();
        identity.AssignClientAuthority(playerIdentity.connectionToClient);
        RpcTurnLightOn();
        identity.RemoveClientAuthority(playerIdentity.connectionToClient);
    }

    [ClientRpc]
    void RpcTurnLightOn() {
        foreach (Light light in switchableLights) {
            light.enabled = true;
            light.color = normalLightColor;
        }
    }
}