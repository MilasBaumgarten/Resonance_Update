using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

/**
 * Author: Leon Ullrich
 * - script that enables interaction with an object
 * - put this script on an object that can be interacted with
 */

[RequireComponent(typeof(NetworkIdentity))]
public class InteractableObject : NetworkBehaviour {

    // enum for character selection
    public enum Characters {BothCharacters = -1, Robert = 0, Catriona = 1}

    [Header("Settings")]
    [Tooltip("Defines the character who can interact with this object")]
    public Characters interactableBy = Characters.BothCharacters;

    [Tooltip("Set this to true, if the invoked event should be synchronized across all clients.")]
    public bool syncThisEvent = true;
    [Tooltip("If true, the event won't fire if a dialog is playing.")]
    public bool lockIfDialogIsPlaying = true;

    [Tooltip("List of gameobjects that get activated after this is triggered")]
    public GameObject[] activateGameobjects;

    [Header("Event")]
    [Tooltip("The event called when a player interacts with the object.")]
    public UnityEvent interactEvent;


    void Start() {
        if (activateGameobjects.Length > 0) {
            foreach (GameObject obj in activateGameobjects) {
                obj.SetActive(false);
            }
        }
    }

    void InteractedWith(GameObject callingPlayer) {
        if (interactableBy.Equals(Characters.BothCharacters)) {
            Trigger(callingPlayer);
        }
        else if (interactableBy.Equals(Characters.Robert)) {
            if (callingPlayer.transform.name.Contains("Rob")) {
                Trigger(callingPlayer);
            }
        }
        else if (interactableBy.Equals(Characters.Catriona)) {
            if (callingPlayer.transform.name.Contains("Cat")) {
                Trigger(callingPlayer);
            }
        }
    }

    void Trigger(GameObject callingPlayer) {
        if (lockIfDialogIsPlaying) {
            if (!DialogSystem.instance.dialogPlaying) {
                if (!syncThisEvent)
                    InvokeEvent();
                else {
                    if (callingPlayer.GetComponent<NetworkIdentity>().isLocalPlayer) {
                        // pass reference of this object to calling player
                        callingPlayer.GetComponent<Interact>().SyncEvent(gameObject);
                    }
                }
            }
        }
        else {
            if (!syncThisEvent)
                InvokeEvent();
            else {
                if (callingPlayer.GetComponent<NetworkIdentity>().isLocalPlayer) {
                    // pass reference of this object to calling player
                    callingPlayer.GetComponent<Interact>().SyncEvent(gameObject);
                }
            }
        }
    }

    // public method which can be called from the player
    public void InvokeEvent() {
        interactEvent.Invoke();
        ActivateGameObjects();
    }


    public void ActivateGameObjects() {
        if (activateGameobjects.Length > 0) {
            foreach (GameObject obj in activateGameobjects) {
                obj.SetActive(true);
            }
        }
    }
}
