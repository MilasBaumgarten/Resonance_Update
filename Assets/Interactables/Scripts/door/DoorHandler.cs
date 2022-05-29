using UnityEngine;

public class DoorHandler : MonoBehaviour {
	[SerializeField]
	private bool permanentlyClosed = false;
	[SerializeField]
	private LayerMask collisionMask;

	[SerializeField]
	private AudioSource openAudio;
	[SerializeField]
	private AudioSource closeAudio;

	[SerializeField]
	private AudioSource errorAudio;

	private Animation anim;
	private bool isInCollider;
    public static bool canLeave = false;

	private void Start() {
		anim = GetComponent<Animation>();
		anim.Rewind();
	}

	private void OnTriggerEnter(Collider other) {
		// check the layer
		if ((collisionMask.value & 1 << other.gameObject.layer) != 0) {
			if (permanentlyClosed) {
				errorAudio.Play();
				return;
			}

			anim.Play();
			isInCollider = true;
			openAudio.Play();
		}
	}

	private void OnTriggerExit(Collider other) {
		if ((collisionMask.value & 1 << other.gameObject.layer) != 0) {
			if (permanentlyClosed) {
				return;
			}

			isInCollider = false;
			foreach (AnimationState animationState in anim) {
				animationState.speed = 1;
			}
		}
	}

	public void WaitUntilPlayerExists() {
		if (isInCollider) {
			foreach (AnimationState animationState in anim) {
				animationState.speed = 0;
			}
		}
	}

	public void PlayCloseSound() {
		closeAudio.Play();
	}

	public void StaysInDoor() {
		if (isInCollider) {
            
			anim.Rewind();
			anim.Play();
		}
	}
}
