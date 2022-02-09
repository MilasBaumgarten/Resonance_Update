using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Should be attached to: Child class should be attached to an interactable object
 * 
 * Created by: Ines
 * 
 * How to use:
 * Use a child class, who inherit from this class to implement object-specific behavior in the Interact() method.
 * Needs a player, who is marked with a 'Player' tag.
 */


public abstract class InteractableItem : MonoBehaviour {

    public InputSettings input;

	[SerializeField]
	[Tooltip("Distance in which the player can interact with the object")]
	private float interactionRange;

	private Transform playerTransform;

	void Awake() {
		playerTransform = GameObject.FindWithTag("Player").transform;
	}

	void Update() {
		if (Input.GetKeyDown(input.interact)) {
			float distance = Vector3.Distance(playerTransform.position, this.transform.position);
			CheckForInteraction(distance);
		}
	}

	void CheckForInteraction(float distance) {
		// if player is in interaction range of object
		if (distance <= interactionRange) {
			Interact();
		}
	}

	public abstract void Interact();
}