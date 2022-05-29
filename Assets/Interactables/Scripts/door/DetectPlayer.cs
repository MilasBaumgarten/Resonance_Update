using UnityEngine;

/**
 * Author: Leon Ullrich
 * Script to detect, if a player is nearby for automated doors
 * Place this on a door along with a trigger collider
 */

public class DetectPlayer : MonoBehaviour {

    Animator anim;

    void Start() {
        anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other) {

        if(other.tag == "Player") {
                anim.SetBool("character_nearby", true);
        }     
    }

    private void OnTriggerExit(Collider other) {
        if (other.tag == "Player") {
                anim.SetBool("character_nearby", false);
        }
    }   	
}
