using Photon.Pun;
using UnityEngine;

public class PlayerAnimatorManager : MonoBehaviourPun, IPunObservable {
    private Animator animator;

    void Start() {
        // check if is local player
        if (photonView.IsMine == false && PhotonNetwork.IsConnected == true) {
            enabled = false;
		}

        animator = GetComponent<Animator>();
        if (!animator) {
            Debug.LogError("PlayerAnimatorManager is Missing Animator Component", this);
        }
    }

    void Update() {
        if (!animator) {
            return;
        }

		// deal with opening and closing Logbook
		if (Input.GetKeyDown(KeyCode.Tab)) {
            if (animator.GetBool("logbook active")) {
                animator.ResetTrigger("logbook active");
            } else {
                animator.SetTrigger("logbook active");
            }
		}
	}

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
        // is local player
        if (stream.IsWriting) {
			// sync stuff
			//stream.SendNext(objectToSync);

		} else {
            //this.objectToSync = (type)stream.ReceiveNext();
        }
    }
}