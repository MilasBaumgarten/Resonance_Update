/* Created by: Ines
 * Gizmos-changes: Leon Ullrich
 * 
 * Should be attached to: Grabable objects which should move on defined axises
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabOnAxis : Grabable {

	[SerializeField]
	[Tooltip("Defines axises to move the object on with an 1 and static axises with 0")]
	private Vector3 moveAxis = new Vector3(1.0f, 1.0f, 1.0f);

    //push object towards the holding position but eleminates the movement on static axises
    protected override void ApplyForce(Rigidbody rb, Vector3 dist, float movePower, int targetCount) {
		rb.AddForce(dist.normalized.x * movePower * targetCount * moveAxis.x,
			dist.normalized.y * movePower * targetCount * moveAxis.y,
			dist.normalized.z * movePower * targetCount * moveAxis.z,
			ForceMode.Force);
	}

    void OnDrawGizmos() {
        
        if(moveAxis.x != 0) {
            // Set color to red, similar to the arrows in the editor
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position - new Vector3(moveAxis.x, 0, 0) * 100, new Vector3(moveAxis.x, transform.position.y, transform.position.z) + new Vector3(moveAxis.x, 0, 0) * 100);
        }
        if(moveAxis.y != 0) {
            // Set color to green, similar to the arrows in the editor
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position - new Vector3(0, moveAxis.y, 0) * 100, new Vector3(transform.position.x, moveAxis.y, transform.position.z) + new Vector3(0, moveAxis.y, 0) * 100);
        }
        if(moveAxis.z != 0) {
            // Set color to blue, similar to the arrows in the editor
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position - new Vector3(0, 0, moveAxis.z) * 100, new Vector3(transform.position.x, transform.position.y, moveAxis.z) + new Vector3(0, 0, moveAxis.z) * 100);
        }
    }
}
