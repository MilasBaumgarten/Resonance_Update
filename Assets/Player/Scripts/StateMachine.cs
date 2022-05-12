using UnityEngine;
using Photon.Pun;

public class StateMachine : MonoBehaviourPun {
	[SerializeField]
	private ArmTool armTool;

	[SerializeField]
	private BoneOverride[] boneOverridesCatriona;
	[SerializeField]
	private BoneOverride[] boneOverridesRobert;
	private BoneOverride[] boneOverrides;

	private Animator anim;
	private LogbookManager logbook;

	private void Start() {
		string nickname = photonView.Owner.NickName;
		if (nickname.Equals(CharacterEnum.CATRIONA.ToString())) {
			boneOverrides = boneOverridesCatriona;
		} else if (nickname.Equals(CharacterEnum.ROBERT.ToString())) {
			boneOverrides = boneOverridesRobert;
		} else {
			Debug.LogWarning("<Color=Red><a>Player</a></Color> nickname: " + nickname + " is unknown.");
		}

		anim = GetComponent<PlayerManager>().animator;
		logbook = GetComponent<PlayerManager>().logbook;
	}

	void Update() {
		// ToDo: ist ekelig, stattdessen von LogbookManager aus ändern
		if (photonView.IsMine) 
			anim.SetBool("logbook active", logbook.isActive);
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
