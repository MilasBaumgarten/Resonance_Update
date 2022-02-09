using UnityEngine;
using Unity.Netcode;

public class StateMachine : NetworkBehaviour {
	[SerializeField]
	private Animator anim;
	[SerializeField]
	private LogbookManager logbook;
	[SerializeField]
	private ArmTool armTool;
	[SerializeField]
	private BoneOverride[] boneOverrides;

	void Update() {
		if (IsLocalPlayer) anim.SetBool("logbook active", logbook.isActive);
		boneOverrides[0].enabled = logbook.isActive;

		//anim.SetBool("armTool active", armTool.armUp);
		boneOverrides[2].enabled = armTool.armUp;
		boneOverrides[1].enabled = armTool.armUp;
	}

	public void Swipe() {
		anim.SetTrigger("swipe");
	}

	public void Touch() {
		anim.SetTrigger("touch");
	}
}
