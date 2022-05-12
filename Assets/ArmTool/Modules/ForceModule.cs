using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

public class ForceModule : ArmToolModule {

    private Transform cam; // the camera attachtched to the player object
    [SerializeField]
    private Transform holdPos;  // the transform of the position infront of the player where objects should be held at
    private bool grabbing = false;      // indicates if the player is already interacting
    private GameObject interactTarget;

    Settings settings;
    InputSettings input;

    private int layerMask = ~(1 << 9);

    public UnityEvent onGrab;

    private HeadBob headBob;
    private PlayerMovement playerMovement;
    private LineRenderer beamRenderer;

    public override Color color {
        get {
            return new Color(0.0314f, 0.5333f, 0.7176f);
        }
    }

    private void Start() {
        settings = GameManager.instance.settings;
        input = GameManager.instance.input;
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

    public override void AttachTo(ArmTool armTool) {
        base.AttachTo(armTool);
        playerMovement = armTool.GetComponent<PlayerMovement>();
        cam = armTool.GetComponentInChildren<Camera>().transform;
        headBob = cam.GetComponent<HeadBob>();
        holdPos = cam.parent.GetChild(1).transform;
    }

    public override void Function(GameObject interactTarget) {
   //     if (grabbing) {
			//armTool.ModuleInteractServerRpc(this.interactTarget);

			//// unlock player
			//headBob.SetBobbing(true);
   //         playerMovement.enabled = true;

   //     } else {
   //         if (interactTarget) {
   //             // if an interactable object is hit and it is within range, interact with it
   //             if (interactTarget.GetComponent<ForceModuleBehaviour>()) {
   //                 this.interactTarget = interactTarget;
   //                 armTool.ModuleInteractServerRpc(interactTarget);
   //                 holdPos.localPosition = Vector3.forward * (interactTarget.transform.position - transform.position).magnitude;

   //                 // lock player
   //                 headBob.SetBobbing(false);
   //                 playerMovement.enabled = false;
   //             }
   //         }
   //     }
   //     onGrab.Invoke();
    }

    protected override void OnTriggerEnter(Collider other) {
        base.OnTriggerEnter(other);
    }

    // Toggle between true and false
    public void ToggleGrab() {
        grabbing = !grabbing;
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
