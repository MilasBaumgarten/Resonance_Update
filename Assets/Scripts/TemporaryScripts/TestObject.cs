using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Should be attached to: interactable object
 * 
 * created by: Ines
 * 
 * How to use:
 * Inhertit from InteractableItem and override the Interact() method.
 * Needs a player, who is marked with a 'Player' tag.
 */

public class TestObject : InteractableItem {

	public override void Interact() {
		Debug.Log("Interaction succesfully!");
	}
}
