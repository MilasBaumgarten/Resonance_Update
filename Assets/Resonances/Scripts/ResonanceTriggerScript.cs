using System.Collections;
using UnityEngine;

/*
 * by Andre Spittel 15.10.2018
 * updated by Andre Spittel 30.10.2018 with help from Felix Werner
 * ---------------------------------------------------------------------------------------------------------------------
 * A script to drag and drop the Resonance to the ResonanceTrigger, and to determine the spawnpoint in the Resonance
 * ---------------------------------------------------------------------------------------------------------------------
 * Place this script onto your Object that wants to trigger the Resonance
 */

[RequireComponent(typeof(Collider))]
public class ResonanceTriggerScript : MonoBehaviour {
	// Resonance that will be triggered
	[SerializeField]
	private ResonanceScript targetResonance;

	public ImageFade image;

	private AudioSource background;

	private bool locked = false;

	private void Start() {
		background = GetComponent<AudioSource>();
	}

	// When someone triggers the resonance, the one who triggers it will have the instance of the ResonanceTestScript, so
	// only he will be able to activate the dialogues.
	private void OnTriggerEnter(Collider other) {
		if (!locked) {
			if (other.CompareTag("Player")){
				// So the other Player doesnt activate it in the transition
				GetComponent<Collider>().enabled = false;

				if (background)
					background.Play();

				StartCoroutine(WaitForFade());
				locked = true;
			}
		}
	}

	IEnumerator WaitForFade() {
		image.gameObject.GetComponent<Animation>().Play();
		yield return new WaitUntil(() => image.faded);

		targetResonance.BeginResonance();
	}

	public IEnumerator Fade() {
		image.gameObject.GetComponent<Animation>().Play();
		yield return new WaitUntil(() => image.faded);
	}
}
