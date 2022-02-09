/* Created by: Ines
 * 
 * Should be attached to: nothing, PressurePlate automatically adds this component to objects lying on it to create a connection.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(NetworkIdentity))]
public class PressurePlateItem : MonoBehaviour {

	private int collisions;
	private PressurePlate connectedPressurePlate = null;

	public PressurePlate ConnectedPressurePlate {
		get {
			return connectedPressurePlate;
		}

		set {
			connectedPressurePlate = value;
		}
	}

	private void Start() {
		collisions = 1; // script is added to object because of collision, therefore it is initialized with 1
	}

	private void OnCollisionEnter(Collision collision) {
		if (connectedPressurePlate != null && connectedPressurePlate.IsValidObject(gameObject)) {
			connectedPressurePlate.OnCollisionEnter(collision); // hand off collision event to connected PressurePlate
			collisions++;
		}
	}

	private void OnCollisionExit(Collision collision) {
		if (connectedPressurePlate != null && connectedPressurePlate.IsValidObject(collision.gameObject)) {
			collisions--;

			// if object has no contact to other PressurePlateItem or PressurePlate itself
			if (collisions == 0) {
				connectedPressurePlate.OnCollisionExit(collision);
			}
		}
	}
}
