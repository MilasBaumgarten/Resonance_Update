using Photon.Pun;
using UnityEngine;

public class EnergyModule : ArmToolModule {

    [SerializeField]
    private GameObject cablePrefab;

    public override ToolType type { get => ToolType.ENERGY; }

    public override Color color {
        get {
            return new Color(0.8118f, 0.7137f, 0.0235f);
        }
    }

    public override void Function(GameObject interactTarget) {
        Debug.LogWarning("Energy Module not Implemented!", this);
		//if (interactTarget) {
		//	Socket targetSocket = interactTarget.transform.GetComponent<Socket>();
		//	if (targetSocket) {
		//		armTool.photonView.RPC("InteractModuleRpc", RpcTarget.All, interactTarget);
		//	}
		//}
	}
}
