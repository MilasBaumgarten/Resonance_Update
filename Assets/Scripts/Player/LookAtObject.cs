// Author: Noah Stolz
// Used to get information from a looked at object
// Should be attached to the PlayerPrefab
// Requires the PlayerPrefab to have a canvas with two Text-Objects

using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class LookAtObject : MonoBehaviourPun {

	[SerializeField]
	[Tooltip("Camera attached to PlayerPrefab")]
	private Camera cam;

	[SerializeField] private LayerMask interactionLayers;

	[SerializeField]
	[Tooltip("Player settings reference")]
	private Settings playerSettings;

	[SerializeField]
	[Tooltip("InputPrompt reference")]
	private InputPrompt inputPrompt;

	private bool interactableFound = false;
	private bool grabableFound = false;
	private int objectID;
	private int lastObjectID;
	// Transform of the viewed Object
	private Transform objectTransform;
	// Transform of the Object that was viewed in the previous frame
	private Transform lastObjTransform;
	private RaycastHit rayHit;
	private Ray ray;
	private Interactable interactable;
	private Grabable grabable;


	// Use this for initialization
	void Start() {
		if (!photonView.IsMine) {
			this.enabled = false;
		}
	}

	// Update is called once per frame
	void LateUpdate() {
		ray = cam.ScreenPointToRay(Input.mousePosition);
		ray.direction = ray.direction.normalized * playerSettings.forceToolMaxDist * 2;

		// need to check for distance when something's found so we can update the prompt if distance grows too far
		if(interactableFound && Vector3.Distance(ray.origin, rayHit.point) > playerSettings.interactionDistance){
			ToggleInteractibleFound(false, false);
			return;
		}

		if(grabableFound && Vector3.Distance(ray.origin, rayHit.point) > playerSettings.forceToolMaxDist){
			ToggleInteractibleFound(false, false);
			return;
		}

		if (Physics.Raycast(ray, out rayHit, playerSettings.interactionDistance, interactionLayers)) {
			objectID = rayHit.colliderInstanceID;

			// Makes sure that getComponent is not called more than once when the object stays the same
			if (objectID == lastObjectID) {
				return;
			}

			lastObjectID = objectID;

			if(rayHit.transform.TryGetComponent<Interactable>(out interactable) && Vector3.Distance(ray.origin, rayHit.point) <= playerSettings.interactionDistance){
				ToggleInteractibleFound(true, false);
			} 
			else if(rayHit.transform.TryGetComponent<Grabable>(out grabable) && Vector3.Distance(ray.origin, rayHit.point) <= playerSettings.forceToolMaxDist){
				ToggleInteractibleFound(true, true);
			} 
			else { // very unlikely edge case when we've hit an object on the right layer but it can neither be interacted with nor grabbed
				ToggleInteractibleFound(false, false);
			}
		} 
		else {
			if(interactableFound){
				ToggleInteractibleFound(false, false);
			}
		}
	}

	private void ToggleInteractibleFound(bool found, bool isGrabable){
		interactableFound = found && !isGrabable;
		grabableFound = found && isGrabable;
		inputPrompt.TogglePrompt(found, isGrabable);
		objectID = lastObjectID = found ? objectID : -1;
	}

	private void OnDrawGizmos() {
		Gizmos.color = Color.red * 0.5f;
		Gizmos.DrawSphere(rayHit.point, 0.1f);
		Gizmos.DrawRay(ray);
	}
}
