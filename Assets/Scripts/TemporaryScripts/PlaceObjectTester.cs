// Author: Noah Stolz
// For testing purposes only. Used to test the event triggerd by PlaceObject
// Can be attached pretty much anywhere

using UnityEngine.Events;
using UnityEngine;

public class PlaceObjectTester : MonoBehaviour {

    
    [SerializeField]
    [Tooltip("Used to Test if the Event is called the way it should be")]
    private GameObject testObj;

    private MeshRenderer testObjMeshRend; // Needed to change the color of the object

    [SerializeField]
    [Tooltip("The Material that the testObject is given when the event is triggered")]
    private Material mat1;

    private UnityAction placeObjListener;

    void Awake() {
        
        placeObjListener = new UnityAction(PlaceObj);

    }


    void Start() {     

        testObjMeshRend = testObj.GetComponent<MeshRenderer>();

    }

    void OnEnable() {

        // Start listening for the event
        EventManager.instance.StartListening("placeObject", placeObjListener);
        
    }

    void OnDisable() {

        // Stop listening when this script is disabled
        EventManager.instance.StopListening("placeObject", placeObjListener);
        
    }

    // The function that is called when the event is triggered
    public void PlaceObj() {

        testObjMeshRend.material = mat1;

    }
}
