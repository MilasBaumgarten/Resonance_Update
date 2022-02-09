using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(Collider))]
public abstract class ArmToolModule : NetworkBehaviour{

    protected ArmTool armTool;

    public abstract Color color { get; }

    public abstract void Function(GameObject interactTarget);

    public virtual void AttachTo(ArmTool armTool) {
        this.armTool = armTool;
    }

    protected virtual void OnTriggerEnter(Collider other) {
        ArmTool armTool = other.GetComponent<ArmTool>();
        if (armTool) {
            armTool.ModulePickUp(this.gameObject);
        }
    }
}
