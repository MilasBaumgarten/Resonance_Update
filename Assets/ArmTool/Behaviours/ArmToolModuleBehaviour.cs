using UnityEngine;
using Unity.Netcode;

[RequireComponent(typeof(NetworkObject))]
public abstract class ArmToolModuleBehaviour : NetworkBehaviour {
    public abstract void Interact(ArmToolModule module);
}
