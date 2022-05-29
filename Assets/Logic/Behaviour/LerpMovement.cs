// Author: Noah Stolz
// Used to move objects to a Transform

using UnityEngine;

namespace Behaviours {

    public class LerpMovement : MonoBehaviour {

        // The Object that is moved
        private GameObject targetObject;

        // The location the Object should be moved to
        private Transform moveTo;

        //How fast the object moves
        private float speed;

        // True once the behaivour has been activated
        private bool activated = false;

        // The direction the object should be moved in
        private Vector3 moveDirection;

        // Update is called once per frame
        void Update() {

            if (activated) {

                // Get the vector that points from the object to the position it should be moved to
                moveDirection = moveTo.position - targetObject.transform.position;

                // Make the object snap to the target position for the last frame
                if (moveDirection.magnitude <= speed * Time.deltaTime) {

                    targetObject.transform.position = moveTo.position;

                    activated = false;

                } else {

                    // Give the vector the length of speed
                    moveDirection = moveDirection.normalized * speed;

                    // Move the object
                    targetObject.transform.position += moveDirection * Time.deltaTime;

                }

            }

        }

        /// <summary>Starts the movement of an object to a specified Transform</summary>
        /// <param name="moveTo">The Transform the object should be moved to</param>
        /// <param name="targetObject">The object to be moved</param>
        /// <param name="speed">The speed at wich the object is moved</param>
        public void Activate(Transform moveTo, GameObject targetObject, float speed) {

            this.moveTo = moveTo;
            this.targetObject = targetObject;
            this.speed = speed;
            activated = true;      

        }
    }
}