
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/**
 * Author: René Fuhrmann   
 * Script to fixate, release Object
 * Attach this script to the Object, that should be fixated
 */
namespace Behaviours {
    public class Fixate : MonoBehaviour {

        /// <summary>Fixates or releases Object</summary>
        /// <param name="targetObject">The object to be fixated</param>
        /// <param name="status">true to fixate, false to release</param>
        public void FixateObject(GameObject targetObject, bool status) {

            targetObject.GetComponent<Grabable>().enabled = !status;
            targetObject.GetComponent<Rigidbody>().isKinematic = status;

        }


    }
}
