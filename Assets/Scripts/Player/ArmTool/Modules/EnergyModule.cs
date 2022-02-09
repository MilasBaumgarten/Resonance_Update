// Author: Feix Werner
//
//
using UnityEngine;
using UnityEngine.Networking;

public class EnergyModule : ArmToolModule {

    [SerializeField]
    private GameObject cablePrefab;
    private GameObject cable;
    private Socket start;
    [SerializeField]
    private float maxDist = 5f;

    public override Color color {
        get {
            return new Color(0.8118f, 0.7137f, 0.0235f);
        }
    }

    public override void Function(GameObject interactionTarget) {
        if (interactionTarget) {
            Socket targetSocket = interactionTarget.transform.GetComponent<Socket>();
            if (targetSocket) {
                armTool.CmdModuleInteract(interactionTarget);
            }
        }
    }

    protected override void OnTriggerEnter(Collider other) {
        base.OnTriggerEnter(other);
    }
}
