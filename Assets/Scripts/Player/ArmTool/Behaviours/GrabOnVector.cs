/* Created by: Ines
 * Gizmos-changes: Leon Ullrich
 * 
 * Should be attached to: Grabable objects which should move on defined axises
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabOnVector : Grabable {

	[SerializeField]
	[Tooltip("Defines axises to move the object on with an 1 and static axises with 0")]
	private Vector3 moveAxis = new Vector3(1.0f, 1.0f, 1.0f);

	//push object towards the holding position but eleminates the movement on static axises
	protected override void ApplyForce(Rigidbody rb, Vector3 dist, float movePower, int targetCount) {
        rb.AddForce(Vector3.Project(new Vector3(dist.normalized.x * movePower * targetCount,
			dist.normalized.y * movePower * targetCount,
			dist.normalized.z * movePower * targetCount), moveAxis),
			ForceMode.Force);
	}

    void OnDrawGizmos() {

        Gizmos.color = Color.red;

        Gizmos.DrawRay(transform.position, moveAxis.normalized * 3);
        Gizmos.DrawRay(transform.position, -moveAxis.normalized * 3);
        
    }
}
