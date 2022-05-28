using UnityEngine;

namespace Logic.Triggers {
    public class OnEnter : Trigger {
    /*
     * Author: Marisa Schmelzer
     *  - must be attached to the object which triggers the event
     */
        [SerializeField] private LayerMask collisionMask;
        void OnTriggerEnter(Collider other) {
            // check the layer
            if ((collisionMask.value & 1 << other.gameObject.layer) != 0) {
                // called from script Trigger
                Interact();
            }
        }
    }
}
