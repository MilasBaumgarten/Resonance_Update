using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Behaviours;

/**
 * Author: René Fuhrmann   
 * Actionscript to fixate Object.
 * Attach this script to the Object, that should be fixated.
 */

namespace Logic.Actions {

    [RequireComponent(typeof(Fixate))]
    public class FixateObject : Action {

        [SerializeField]
        [Tooltip("The Object that is fixated")]
        private GameObject targetObject;

        public override void Activate() {
            base.Activate();
            // call Behaviour Script
            GetComponent<Fixate>().FixateObject(targetObject, true);
        }
    }
}