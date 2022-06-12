using UnityEngine;
using Photon.Pun;
using System;

public class StateMachine : MonoBehaviourPun {
	private Animator anim;

	private void Start() {
		anim = GetComponent<PlayerManager>().animator;
	}

	public void Swipe() {
		anim.SetTrigger("swipe");
	}

	public void Touch() {
		anim.SetTrigger("touch");
	}
}
