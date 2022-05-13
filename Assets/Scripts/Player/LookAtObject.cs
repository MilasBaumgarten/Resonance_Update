// Author: Noah Stolz
// Used to get information from a looked at object
// Should be attached to the PlayerPrefab
// Requires the PlayerPrefab to have a canvas with two Text-Objects

using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class LookAtObject : MonoBehaviourPun {

	[SerializeField]
	[Tooltip("The UI-Text that displays the object name")]
	private Text nameText;

	[SerializeField]
	[Tooltip("The UI-Text that displays the object info")]
	private Text infoText;

	[SerializeField]
	[Tooltip("Camera attached to PlayerPrefab")]
	private Camera cam;

	[SerializeField]
	[Tooltip("How far away can an Object be and still get registered")]
	private float maxDistance = 5f;

	// Transform of the viewed Object
	private Transform objectTransform;
	// Transform of the Object that was viewed in the previous frame
	private Transform lastObjTransform;
	private RaycastHit rayHit;
	private Ray ray;

	// Use this for initialization
	void Start() {
		if (!photonView.IsMine) {
			this.enabled = false;
		}
	}

	// Update is called once per frame
	void Update() {
		ray = cam.ScreenPointToRay(Input.mousePosition);

		if (Physics.Raycast(ray, out rayHit)) {
			objectTransform = rayHit.transform;

			// Uses the position of the player and the object the find the distance between them
			float distance = Vector3.Magnitude(objectTransform.position - this.transform.position);

			if (distance > maxDistance) {
				infoText.text = " ";
				nameText.text = " ";
				return;
			}

			// Makes sure that getComponent is not called more than once when the object stays the same
			if (objectTransform == lastObjTransform) {
				return;
			}

			lastObjTransform = objectTransform;

			ObjectInfo objInfo = objectTransform.GetComponent<ObjectInfo>();

			if (objInfo != null) {
				infoText.text = objInfo.objectInfo;
				nameText.text = objInfo.objectName;
			} else {
				infoText.text = " ";
				nameText.text = " ";
			}
		}
	}
}
