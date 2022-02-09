using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(NetworkIdentity))]
public abstract class Interactable : MonoBehaviour {

    public abstract void Interact(ArmTool armTool);
}
