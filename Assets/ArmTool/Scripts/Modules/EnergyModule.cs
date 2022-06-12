using Photon.Pun;
using UnityEngine;

public class EnergyModule : ArmToolModule {

    [SerializeField]
    private GameObject cablePrefab;

    public override Color color {
        get {
            return new Color(0.8118f, 0.7137f, 0.0235f);
        }
    }

    public override void Function(GameObject interactTarget) {
		if (interactTarget) {
			Socket targetSocket = interactTarget.transform.GetComponent<Socket>();
			if (targetSocket) {
				armTool.photonView.RPC("InteractModuleRpc", RpcTarget.All, interactTarget);
			}
		}
	}
}
