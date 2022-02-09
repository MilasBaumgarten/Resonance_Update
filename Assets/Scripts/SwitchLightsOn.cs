using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;


public class SwitchLightsOn : NetworkBehaviour {
    [SerializeField]
    private List<Light> switchableLights = new List<Light>();

    [SerializeField]
    private Color normalLightColor = new Color();

    private NetworkObject identity;
    private NetworkObject playerIdentity;

    //NetworkVariable<GameObject> player = new NetworkVariable<GameObject>();
    GameObject player;

	void Awake() {
        identity = GetComponent<NetworkObject>();
    }

    void Start() {
        foreach(Light light in switchableLights) {
            light.color = new Color(0, 0, 0, 0);
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.tag.Equals("Player")) {
            player = other.gameObject;
            //TurnLightOnServerRpc(player);
        }
    }

    //[ServerRpc]
    //void TurnLightOnServerRpc(GameObject player) {
    //    //playerIdentity = player.GetComponent<NetworkObject>();
    //    //identity.AssignClientAuthority(playerIdentity.connectionToClient);
    //    //RpcTurnLightOn();
    //    //identity.RemoveClientAuthority(playerIdentity.connectionToClient);
    //}

    [ClientRpc]
    void TurnLightOnClientRpc() {
        foreach (Light light in switchableLights) {
            light.enabled = true;
            light.color = normalLightColor;
        }
    }
}