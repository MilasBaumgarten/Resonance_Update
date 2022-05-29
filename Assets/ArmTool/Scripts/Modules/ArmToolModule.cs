using UnityEngine;

public abstract class ArmToolModule : MonoBehaviour {

    [SerializeField]
    protected ArmTool armTool;

    public abstract Color color { get; }

    public abstract void Function(GameObject interactTarget);
}
