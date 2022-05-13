using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorHandler : MonoBehaviour {

	private Animation anim;
	private bool isInCollider;
    public static bool canLeave = false;

	private void Start() {
		anim = GetComponent<Animation>();
		anim.Rewind();
	}

	private void OnTriggerEnter(Collider other) {
        anim.Play();
        isInCollider = true;
	}

	private void OnTriggerExit(Collider other) {
		isInCollider = false;
		foreach (AnimationState animationState in anim) {
			animationState.speed = 1;
		}
	}

	public void WaitUntilPlayerExists() {
		if (isInCollider) {
			foreach (AnimationState animationState in anim) {
				animationState.speed = 0;
			}
		}
	}

	public void StaysInDoor() {
		if (isInCollider) {
            anim.Rewind();
			anim.Play();
		}
	}
}
