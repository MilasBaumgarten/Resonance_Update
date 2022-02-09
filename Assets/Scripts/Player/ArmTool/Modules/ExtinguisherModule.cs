using UnityEngine;
using UnityEngine.Networking;

public class ExtinguisherModule : ArmToolModule {

    [SerializeField]
    private GameObject extinguisher;

    public override Color color {
        get {
            return new Color(0.8235f, 0.0196f, 0.2863f);
        }
    }

    public override void Function(GameObject interactionTarget) {
        armTool.CmdModuleInteract(extinguisher);
    }

    protected override void OnTriggerEnter(Collider other) {
        base.OnTriggerEnter(other);
    }
}
