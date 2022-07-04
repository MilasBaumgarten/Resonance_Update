using UnityEngine;

public enum ToolType {
    NONE,
    FORCE,
    EXTINGUISHER,
    ENERGY
}

public abstract class ArmToolModule : MonoBehaviour {
    public abstract ToolType type { get;}

    [SerializeField]
    protected ArmTool armTool;

    public abstract Color color { get; }

    public abstract void Function(GameObject interactTarget);
}
