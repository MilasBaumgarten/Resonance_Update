using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(NetworkIdentity))]
public abstract class ArmToolModuleBehaviour : NetworkBehaviour {
    public abstract void Interact(ArmToolModule module);
}
