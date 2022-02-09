﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/**
 * Author: Leon Ullrich
 * - Component which can fire events when a player enters a colission zone
 */

public class ColliderTrigger : MonoBehaviour {

    // enum for character selection
    public enum Characters { BothCharacters = -1, Robert = 0, Catriona = 1 };

    [Header("Settings")]
    [Tooltip("If true, both players need to enter the collission zone to trigger the event.")]
    public bool needsBothPlayers = false;

    [Tooltip("Choose, which character is able to trigger this.")]
    public Characters triggeredBy = Characters.BothCharacters;

    [Tooltip("If true, gameobject gets destroyed after trigger.")]
    public bool destroyObjectAfterTrigger = false;
    [Tooltip("If true, event locks itself after it's first invocation.")]
    public bool invokeOnlyOnce = true;
    [Tooltip("If true, the event won't fire if a dialog is playing.")]
    public bool lockIfDialogIsPlaying = true;
    [Tooltip("Synchronize the event if true")]
    public bool syncEvent = false;

    [Tooltip("List of gameobjects that get activated after this is triggered")]
    public GameObject[] activateGameobjects;

    [Space]
    [Header("Event")]
    public UnityEvent interaction;

    // private fields
    private int playerCount = 0;
    private bool hasPlayed = false;

    private bool locked = false;

    void Start() {
        // make sure, all activatable gameobjects are inactive by default
        if (activateGameobjects.Length > 0) {
            foreach (GameObject obj in activateGameobjects) {
                obj.SetActive(false);
            }
        }
    }

    void OnTriggerEnter(Collider other) {
        // takes a look if gameobject has player tag and if it was triggered once
        if (other.CompareTag("Player") && !hasPlayed) {
            // increase player-counter
            playerCount++;

            if (!syncEvent) {
                // if both players can trigger this, continue as usual
                if (triggeredBy == Characters.BothCharacters) {
                    Trigger();
                }
                // else, check if the entered player is the one who can trigger this
                else {
                    // only rob can trigger
                    if (triggeredBy == Characters.Robert) {
                        if (other.transform.name.Contains("Rob")) {

                            Trigger();
                        }
                    }
                    // only cat can trigger
                    else if (triggeredBy == Characters.Catriona) {
                        if (other.transform.name.Contains("Cat")) {

                            Trigger();
                        }
                    }
                }
            } else {
                // if both players can trigger this, continue as usual
                if (triggeredBy == Characters.BothCharacters) {
                    Debug.Log(other.gameObject.name);
                    other.gameObject.GetComponent<Interact>().SyncTriggerEvent(gameObject);
                }
                // else, check if the entered player is the one who can trigger this
                else {
                    // only rob can trigger
                    if (triggeredBy == Characters.Robert) {
                        if (other.transform.name.Contains("Rob")) {

                            other.GetComponent<Interact>().SyncTriggerEvent(gameObject);
                        }
                    }
                    // only cat can trigger
                    else if (triggeredBy == Characters.Catriona) {
                        if (other.transform.name.Contains("Cat")) {

                            other.GetComponent<Interact>().SyncTriggerEvent(gameObject);
                        }
                    }
                }
            }
            
        }
    }

    public void Trigger() {

        if (lockIfDialogIsPlaying) {
            if (!DialogSystem.instance.dialogPlaying) {
                if (invokeOnlyOnce) {
                    if (!locked) {
                        InvokeEvent();
                    }
                }
                else {
                    InvokeEvent();
                }
            }
        }
        else {
            if (invokeOnlyOnce) {
                if (!locked) {
                    InvokeEvent();
                }
            }
            else {
                InvokeEvent();
            }
        }
    }

    void OnTriggerExit(Collider other) {
        if (other.CompareTag("Player")) {
            //if one or more players are inside the trigger
            if (playerCount > 0) {
                // decrease player-count
                playerCount--;
            }
        }
    }

    void InvokeEvent() {
        // check if both players are needed to invoke event
        if (needsBothPlayers) {
            // if true, invoke event only if both players are inside the trigger
            if (playerCount > 1) {
                interaction.Invoke();
                if (destroyObjectAfterTrigger) {
                    Destroy(gameObject, 1f);
                }    
                locked = true;
                ActivateGameObjects();
            }           
        }
        else {
            interaction.Invoke();
            if (destroyObjectAfterTrigger)
                Destroy(gameObject, 1f);
            locked = true;
            ActivateGameObjects();
        }
    }

    void ActivateGameObjects() {
        if(activateGameobjects.Length > 0) {
            foreach (GameObject obj in activateGameobjects) {
                obj.SetActive(true);
            }
        }      
    }
}
