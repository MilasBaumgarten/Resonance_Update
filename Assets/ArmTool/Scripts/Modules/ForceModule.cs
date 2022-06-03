using Photon.Pun;
using UnityEngine;
using UnityEngine.Events;

public class ForceModule : ArmToolModule {
    [SerializeField]
    private Settings settings;
    [SerializeField]
    private InputSettings input;

    [SerializeField]
    private Transform holdPos;  // the transform of the position infront of the player where objects should be held at

    public UnityEvent onGrab;

    //private Transform cam; // the camera attachtched to the player object
    private bool grabbing = false;      // indicates if the player is already interacting
	private GameObject grabbingTarget;

	//private int layerMask = ~(1 << 9);

	[SerializeField]
    private HeadBob headBob;
    [SerializeField]
    private PlayerMovement playerMovement;
    private LineRenderer beamRenderer;

    public override Color color {
        get {
            return new Color(0.0314f, 0.5333f, 0.7176f);
        }
    }

    private void Start() {
        beamRenderer = GetComponentInChildren<LineRenderer>();
        if (beamRenderer.enabled) {
            beamRenderer.enabled = false;
        }
    }

    private void Update() {
        if (holdPos) {
            // move the object further away from/closer to the player
            if (Input.GetKey(input.moveHeldObjectIn) && holdPos.localPosition.magnitude < settings.maxDist) {
                holdPos.localPosition += Vector3.forward * settings.distChangeSpeed * Time.deltaTime;
            }
            if (Input.GetKey(input.moveHeldObjectOut) && holdPos.localPosition.magnitude > settings.minDist) {
                holdPos.localPosition -= Vector3.forward * settings.distChangeSpeed * Time.deltaTime;
            }
        }
    }

    public override void Function(GameObject interactTarget) {
		if (grabbing) {
            armTool.photonView.RPC("InteractModuleRpc", RpcTarget.All, grabbingTarget.GetPhotonView().ViewID);

			// unlock player
			headBob.SetBobbing(true);
			playerMovement.enabled = true;
            grabbing = false;
		} else {
			if (interactTarget) {
				// if an interactable object is hit and it is within range, interact with it
				if (interactTarget.GetComponent<ArmToolModuleBehaviour>()) {
					grabbingTarget = interactTarget;
					armTool.photonView.RPC("InteractModuleRpc", RpcTarget.All, interactTarget.GetPhotonView().ViewID);
                    holdPos.localPosition = Vector3.forward * (interactTarget.transform.position - transform.position).magnitude;

					// lock player
					headBob.SetBobbing(false);
					playerMovement.enabled = false;
                    grabbing = true;

                }
			}
		}
		onGrab.Invoke();
	}

    public bool GetGrabStatus() {
        return grabbing;
    }

    public Transform GetHoldPosition() {
        return holdPos;
    }

    public LineRenderer GetBeamRenderer() {
        return beamRenderer;
    }
}
