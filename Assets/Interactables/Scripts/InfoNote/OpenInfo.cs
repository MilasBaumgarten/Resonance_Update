using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 *  Author: Leon Ullrich
 *  - enables the player to interact with information-note objects in the scene
 *  - put this script on the player
 */

public class OpenInfo : MonoBehaviour {

    public InputSettings input;
    public Settings settings;

    private GameObject infoObject;

    // ignore player-layer (the 9th layer)
    private int layerMask = ~(1 << 9);

    [SerializeField] private Transform cam;

    // Use this for initialization
    void Start() {
        cam = transform.GetComponentInChildren<Camera>().transform;
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(input.interact)) {
            // search for interactable objects via raycast from the player camera (only supports grabbing for now)
            RaycastHit hit;
            if (Physics.Raycast(cam.position, cam.forward, out hit, settings.forceToolMaxDist, layerMask)) {

                //Debug.Log("RayCast hit " + hit.transform.gameObject.tag);

                if (hit.transform.gameObject.CompareTag("Info")) {

                    //Debug.Log("Info hit");

                    infoObject = hit.transform.gameObject;
                    infoObject.GetComponent<OpenInfoPanel>().Open();

                }
            }
        }
    }

    void OnDrawGizmos() {
        Gizmos.DrawRay(cam.position, cam.forward * settings.forceToolMaxDist);
    }
}
