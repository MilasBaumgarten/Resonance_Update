using UnityEngine;
using UnityEngine.Networking;

public class SpawnSetupATeam : NetworkBehaviour {

    [SerializeField]
    Behaviour[] ToDisable;
    [SerializeField]
    GameObject[] GameobjectsToDisable;
    [SerializeField]
    GameObject[] GameobjectsToEnable;

    void Start() {
        if (!isLocalPlayer) {
            foreach (Behaviour component in ToDisable) {
                if(component)
                component.enabled = false;

            }

            foreach (GameObject go in GameobjectsToDisable)
            {
                if(go)
                go.SetActive(false);
            }
        }
        else
        {
            foreach (GameObject go in GameobjectsToEnable)
            {
                go.SetActive(true);
            }
        }
    }
}
