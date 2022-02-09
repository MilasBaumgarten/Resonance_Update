// Author: Noah Stolz
// Moves an object to a transform when activated

using UnityEngine;
using Behaviours;

namespace Logic.Actions {

    [RequireComponent(typeof(LerpMovement))]
    public class MoveObjectAction : Action {

        [SerializeField]
        [Tooltip("The Object that is moved")]
        private GameObject targetObject;

        [SerializeField]
        [Tooltip("The location the Object should be moved to")]
        private Transform moveTo;

        [SerializeField]
        [Tooltip("Should the Gizmo be visible")]
        private bool showGizmo = true;

        [SerializeField]
        [Tooltip("How fast the object moves")]
        private float speed = 1f;

        public override void Activate() {

            base.Activate();
            GetComponent<LerpMovement>().Activate(moveTo, targetObject, speed);

        }

        ///<summary>Draws a Gizmo in the Form of the target object</summary>
        private void OnDrawGizmos() {

            if (showGizmo && moveTo != null && targetObject != null) {

                Gizmos.color = new Color(0.5f, 0.5f, 0.5f, 0.7f);
                Gizmos.DrawMesh(targetObject.GetComponent<MeshFilter>().sharedMesh, moveTo.transform.position, moveTo.transform.rotation, targetObject.transform.localScale);

            }

        }

    }

}