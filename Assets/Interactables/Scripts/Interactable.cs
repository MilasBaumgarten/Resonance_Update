using Photon.Pun;
using UnityEngine;

[RequireComponent(typeof(PhotonView))]
public abstract class Interactable : MonoBehaviour {

    public abstract void Interact(ArmTool armTool);
}
