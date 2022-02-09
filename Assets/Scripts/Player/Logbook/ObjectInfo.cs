// Author: Noah Stolz
// Contains the name and information of an object
// Should be attached to the object that you want to display information about

using UnityEngine;

public class ObjectInfo : MonoBehaviour {

    [Tooltip("Name that appears when the Object is looked at")]
    public string objectName = "Put Name Here";

    [Tooltip("Information that appears when the Object is looked at")]
    public string objectInfo = "Info here";
}
