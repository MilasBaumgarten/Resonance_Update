/* Created by: Ines
 * 
 * Should be attached to: Pressure Plate
 */

using System.Collections.Generic;
using UnityEngine;
using Logic;

[RequireComponent(typeof(Collider))]
public class PressurePlate : Trigger
{
	[SerializeField]
	[Tooltip("Define number of objects needed to set the state of plate to true.")]
	private int objectsNeeded = 1;


	// store registered objects to count them by identity
	private Dictionary<ulong, GameObject> objectsCounted;

	void Start() {
		objectsCounted = new Dictionary<ulong, GameObject>();
	}

	public void OnCollisionEnter(Collision collision) {

		if (IsValidObject(collision.gameObject)) {

			// if center of mass is vertical in bounds of collider
			if (gameObject.GetComponent<Collider>().bounds.Contains(
				(Vector3.ProjectOnPlane(collision.gameObject.transform.position + collision.gameObject.GetComponent<Rigidbody>().centerOfMass, Vector3.up))
				+ new Vector3(0, gameObject.transform.position.y, 0))) {

				// if it is a new object, then add it
				// TODO: reenable
				//if (!objectsCounted.ContainsKey(collision.gameObject.GetComponent<NetworkObject>().NetworkObjectId)) {
				//	objectsCounted.Add(collision.gameObject.GetComponent<NetworkObject>().NetworkObjectId, collision.gameObject); //register object

				//	// add PressurePlateItem component and hand over reference to this PressurePlate object
				//	// this creates a connection between added object and PressurePlate
				//	collision.gameObject.AddComponent(typeof(PressurePlateItem));
				//	collision.gameObject.GetComponent<PressurePlateItem>().ConnectedPressurePlate = gameObject.GetComponent<PressurePlate>();

				//	// if needed amount is reached
				//	if (objectsCounted.Count == objectsNeeded) {
				//		FlipState();
				//	}
				//}
			}
		}
	}

	public void OnCollisionExit(Collision collision) {
		// delete object if it was registered before (if it was registered before then it is also a valid object)
		// TODO: reenable
		//if (objectsCounted.ContainsKey(collision.gameObject.GetComponent<NetworkObject>().NetworkObjectId)) {

		//	objectsCounted.Remove(collision.gameObject.GetComponent<NetworkObject>().NetworkObjectId); // unregister object
		//	Destroy(collision.gameObject.GetComponent<PressurePlateItem>()); // remove PressurePlateItem component

		//	// if state changes from true to false because of leaving object
		//	if (objectsCounted.Count == objectsNeeded - 1) {
		//		FlipState();
		//	}
		//}
	}

	public bool IsValidObject(GameObject toCheck) {
		return (toCheck.GetComponent<Interactable>() != null ||
			toCheck.GetComponent<ForceModule>() != null);
	}
}