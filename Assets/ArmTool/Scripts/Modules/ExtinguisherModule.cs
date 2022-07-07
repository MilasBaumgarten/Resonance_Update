using Photon.Pun;
using UnityEngine;

public class ExtinguisherModule : ArmToolModule {

    [SerializeField]
    private GameObject extinguisher;

    public override ToolType type { get => ToolType.EXTINGUISHER; }

    public override Color color {
        get {
            return new Color(0.8235f, 0.0196f, 0.2863f);
        }
    }

	public override void Function(GameObject interactTarget) {
        armTool.photonView.RPC("InteractModuleRpc", RpcTarget.All, armTool.photonView.ViewID, armTool.GetSelected(), extinguisher.GetPhotonView().ViewID);
    }
}
