using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ArmTool : NetworkBehaviour {

    [SerializeField]
    private StateMachine stateMachine;
    [SerializeField]
    private ArmToolModule[] equipped;
    [SerializeField]
    private int selected = 0;
    [SerializeField]
    private Image toolIcon;
    [SerializeField]
    private Transform modules;
    private int layerMask = ~(1 << 9);
    private Transform cam;
    public bool armUp = false;

    InputSettings input;
    Settings settings;

    // Use this for initialization
    void Start() {
        input = GameManager.instance.input;
        settings = GameManager.instance.settings;
        cam = transform.GetComponentInChildren<Camera>().transform;
        equipped = new ArmToolModule[4];
        foreach (Transform child in modules) {
            equipped[child.GetSiblingIndex()] = child.GetComponent<ArmToolModule>();
        }
        ChangeIconColor();
    }

    // Update is called once per frame
    void Update() {
        if (isLocalPlayer) {
            if (Input.GetKeyDown(input.armTool)) {

                GameObject interactTarget;
                RaycastHit hit;
                if (Physics.Raycast(cam.position, cam.forward, out hit, settings.maxDist, layerMask)) {
                    interactTarget = hit.transform.gameObject;
                    if (interactTarget.GetComponent<Interactable>()) {
                        CmdInteract(interactTarget);
                    }
                    if (equipped[selected]) {
                        equipped[selected].Function(interactTarget);
                    }
                } else {
                    if (equipped[selected]) {
                        equipped[selected].Function(null);
                    }
                }
            }
            if (Input.GetKeyDown(KeyCode.Alpha1) && equipped[1]) {
                if(selected == 1) {
                    selected = 0;
                    ChangeIconColor();
                } else {
                    selected = 1;
                    ChangeIconColor();
                }
            }
            if (Input.GetKeyDown(KeyCode.Alpha2) && equipped[2]) {
                if (selected == 2) {
                    selected = 0;
                    ChangeIconColor();
                } else {
                    selected = 2;
                    ChangeIconColor();
                }
            }
            if (Input.GetKeyDown(KeyCode.Alpha3) && equipped[3]) {
                if (selected == 3) {
                    selected = 0;
                    ChangeIconColor();
                } else {
                    selected = 3;
                    ChangeIconColor();
                }
            }
            if(selected > 0) {
                armUp = true;
            } else {
                armUp = false;
            }
        }
    }

    public void DeselectTool()
    {
        this.selected = 0;
        ChangeIconColor();
        this.armUp = false;
    }

    private void ChangeIconColor() {
        if (equipped[selected]) {
            toolIcon.color = equipped[selected].color;
        } else {
            toolIcon.color = Color.white;
        }
    }

    public void ModulePickUp(GameObject obj) {
        CmdAddModule(obj);
    }

    private void AddModule(GameObject obj) {
        ArmToolModule module = obj.GetComponent<ArmToolModule>();

        foreach (ArmToolModule currentModule in equipped) {
            if (currentModule) {
                if (currentModule.GetType() == module.GetType()) {
                    //promt the player that they already have this module equipped
                    return;
                }
            }
        }
        module.AttachTo(this);

        for (int i = 1; i < equipped.Length; i++) {
            if (equipped[i] == null) {
                equipped[i] = module;
                module.transform.parent = modules;
                module.transform.localPosition = Vector3.zero;
                module.transform.localRotation = Quaternion.identity;
                //module.GetComponent<MeshRenderer>().enabled = false;
                module.GetComponent<Collider>().enabled = false;
                selected = i;
                ChangeIconColor();
                return;
            }
        }
        //all module slots are filled...
        //maybe replace current selected module??
    }

    public void RemoveModule(System.Type type) {
        foreach(ArmToolModule module in equipped) {
            if (module) {
                if(module.GetType() == type) {
                    CmdRemoveModule(equipped[selected].gameObject);
                }
            }
        }
        //TODO update HUD and maybe shift remaining modules
    }

    [Command]
    void CmdInteract(GameObject target) {
        // send the server a command telling it the player is intending to interact with something
        RpcInteract(target);
    }

    [Command]
    void CmdAddModule(GameObject obj) {
        RpcAddModule(obj);
    }

    [Command]
    void CmdRemoveModule(GameObject obj) {
        RpcRemoveModule(obj);
    }

    [ClientRpc]
    void RpcInteract(GameObject target) {
        // send a call to all clients informing them about the interaction
        target.GetComponent<Interactable>().Interact(this);
    }

    [ClientRpc]
    void RpcAddModule(GameObject obj) {
        AddModule(obj);
    }

    [ClientRpc]
    void RpcRemoveModule(GameObject obj) {
        Destroy(obj);
        equipped[selected] = null;
        ChangeIconColor();
    }

    [Command]
    public void CmdModuleInteract(GameObject target) {
        // send the server a command telling it the player is intending to interact with something
        RpcModuleInteract(target);
    }

    [ClientRpc]
    void RpcModuleInteract(GameObject target) {
        // send a call to all clients informing them about the interaction
        target.GetComponent<ArmToolModuleBehaviour>().Interact(equipped[selected]);
    }
}
