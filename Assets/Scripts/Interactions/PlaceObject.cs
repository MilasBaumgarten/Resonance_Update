// Author: Noah Stolz
// Triggers an Event when the specified object enters the Collider
// Should be attached to the Trigger zone

using UnityEngine;

[RequireComponent(typeof(Collider))]

public class PlaceObject : MonoBehaviour {

    [SerializeField]
    [Tooltip("The Object that triggers the event")]
    private GameObject targetObject;   

    [SerializeField]
    [Tooltip("The location the Object should snap to")]
    private Transform snapTo;

    [SerializeField]
    [Tooltip("Should the Gizmo be visible")]
    private bool showGizmo = true;

    void OnTriggerEnter(Collider col) {
        
        if (targetObject.Equals(col.gameObject)) {
            
            EventManager.instance.TriggerEvent("placeObject");
            SnapPosition(col.gameObject);

        }

    }

    // Makes the detected object snap to the position of the given transform
    public void SnapPosition(GameObject detectedObject) {

        Rigidbody rigBody = detectedObject.GetComponent<Rigidbody>();

		// Sets the rotation of the object to it's default value
		detectedObject.transform.rotation = snapTo.rotation;
		detectedObject.transform.position = snapTo.position;

        // As long as there is no way to force players to drop objects they are holding with the forcetool, objects have to be frozen
        rigBody.constraints = RigidbodyConstraints.FreezeAll;

        rigBody.useGravity = false;
    }

    // Draws a Gizmo in the Form of the target object 
    private void OnDrawGizmos() {
        if (showGizmo) {
            Gizmos.color = new Color(0.5f, 0.5f, 0.5f, 0.7f);
            Gizmos.DrawMesh(targetObject.GetComponent<MeshFilter>().sharedMesh, snapTo.transform.position, snapTo.transform.rotation, targetObject.transform.localScale);
        }
        
    }
}
