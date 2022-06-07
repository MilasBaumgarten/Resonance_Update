using Photon.Pun;
using UnityEngine;

[RequireComponent(typeof(PhotonView))]
public abstract class ArmToolModuleBehaviour : MonoBehaviour {
    public abstract void Interact(ArmToolModule module);
}
