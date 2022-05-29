using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Author: Leon Ullrich
 * Script that opens a door via event
 * Place this on a door and refer to this in a UnityEvent
 */

public class OpenDoor : MonoBehaviour {

    private Animator anim;

    void Start() {
        anim = GetComponent<Animator>();
    }

    public void Open(bool open) {
        anim.SetBool("open_door", open);
    }

}
