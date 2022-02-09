using UnityEngine;
using UnityEngine.Networking;
using Unity.Netcode;

[RequireComponent(typeof(NetworkObject))]
public abstract class Interactable : MonoBehaviour {

    public abstract void Interact(ArmTool armTool);
}
