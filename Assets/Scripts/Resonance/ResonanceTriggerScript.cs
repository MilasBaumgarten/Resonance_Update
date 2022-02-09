using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

/*
 * by Andre Spittel 15.10.2018
 * updated by Andre Spittel 30.10.2018 with help from Felix Werner
 * ---------------------------------------------------------------------------------------------------------------------
 * A script to drag and drop the Resonance to the ResonanceTrigger, and to determine the spawnpoint in the Resonance
 * ---------------------------------------------------------------------------------------------------------------------
 * Place this script onto your Object that wants to trigger the Resonance
 */

[RequireComponent(typeof(NetworkIdentity), typeof(Collider))]
public class ResonanceTriggerScript : NetworkBehaviour {

    // We need the reference to get the player´s input, and to set the bool "activated" in ResonanceTestScript to true,
    // if the player collided with the resonance.
    [HideInInspector]
    public ResonanceTestScript rts;

    public ImageFade image;
    
    private AudioSource background;
    private Collider activatingPlayer;

    // When someone triggers the resonance, the one who triggers it will have the instance of the ResonanceTestScript, so
    // only he will be able to activate the dialogues.

    private void Start() {
        background = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other) {
        rts = other.gameObject.GetComponent<ResonanceTestScript>();
		if (rts) {

        // So the other Player doesnt activate it in the transition
        GetComponent<Collider>().enabled = false;
        
        if(background)
            background.Play();

        StartCoroutine(WaitForFade());

		}
    }

    IEnumerator WaitForFade() {
        image.gameObject.GetComponent<Animation>().Play();
        yield return new WaitUntil(() => image.faded);

        if (rts) {
            
            EventManager.instance.TriggerEvent("ResonanceTrigger");
        }
    }

    public IEnumerator Fade() {
        image.gameObject.GetComponent<Animation>().Play();
        yield return new WaitUntil(() => image.faded);
    }

}
