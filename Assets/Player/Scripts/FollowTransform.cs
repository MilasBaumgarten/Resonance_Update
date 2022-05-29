using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTransform : MonoBehaviour {

    [SerializeField]
    private Vector3 locationOffset;

    [SerializeField]
    private Transform targetToFollow;

    [SerializeField]
    private bool copyLocation;

    [SerializeField]
    private bool xLoc;
    [SerializeField]
    [Range(0f, 2f)]
    private float xLocInfluence;

    [SerializeField]
    private bool yLoc;
    [SerializeField]
    [Range(0f, 2f)]
    private float yLocInfluence;

    [SerializeField]
    private bool zLoc;
    [SerializeField]
    [Range(0f, 2f)]
    private float zLocInfluence;


    [SerializeField]
    private bool copyRotation;

    [SerializeField]
    private Vector3 rotationOffset;

    [SerializeField]
    private bool Helper;

    [SerializeField]
    private bool xRot;
    [SerializeField]
    [Range(0f, 2f)]
    private float xRotInfluence;

    [SerializeField]
    private bool yRot;
    [SerializeField]
    [Range(0f, 2f)]
    private float yRotInfluence;

    [SerializeField]
    private bool zRot;
    [SerializeField]
    [Range(0f, 2f)]
    private float zRotInfluence;





    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        if (copyLocation) {

            Vector3 targetLocation = this.transform.position;

            if (xLoc) targetLocation.x = targetToFollow.position.x * xLocInfluence;
            if (yLoc) targetLocation.y = targetToFollow.position.y * yLocInfluence;
            if (zLoc) targetLocation.z = targetToFollow.position.z * zLocInfluence;

            this.transform.position = targetLocation + locationOffset;

        }
        if (copyRotation) {

            
            //Helper for this fucked up thing called CameraMovement, wtf?
            float helperAngle = targetToFollow.localEulerAngles.x > 90f ? targetToFollow.eulerAngles.x - 360f : targetToFollow.eulerAngles.x;
            //Debug.Log(a);
            Vector3 targetRotation = this.transform.localEulerAngles;

            

            if (xRot) targetRotation.x = Helper? helperAngle*xRotInfluence : targetToFollow.localEulerAngles.x * xRotInfluence;
            if (yRot) targetRotation.y = targetToFollow.localEulerAngles.y * yRotInfluence;
            if (zRot) targetRotation.z = targetToFollow.localEulerAngles.z * zRotInfluence;

            this.transform.localEulerAngles = targetRotation + rotationOffset;
        } 

	}

    
}
