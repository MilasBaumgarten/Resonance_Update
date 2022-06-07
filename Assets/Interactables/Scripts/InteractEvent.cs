// Author: Noah Stolz
// Used to call an Event when the player interacts with this object
// Should be attached to the Object used to call the event

using UnityEngine.Events;

public class InteractEvent : Interactable {
	public UnityEvent onInteract;

	public override void Interact(ArmTool armTool) {
		if (onlyExecuteLocally) {
			if (!armTool.photonView.IsMine) {
				return;
			}
		}

		onInteract.Invoke();

		if (gameObject.name == "ForschungsKonsole") {
			armTool.GetComponent<ResearchConsole>().Open();
		}
	}
}
