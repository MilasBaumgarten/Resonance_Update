using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoneOverride : MonoBehaviour {


    [SerializeField]
    private bool aimAtTarget;
    public Transform LookAtTarget;
    public Vector3 Offset;

    [HideInInspector]
    public bool isActive;

    [SerializeField]
    private Animator anim;
  
    Transform Bone;
    // Use this for initialization
    void Start () {

        Bone = this.transform;
    }
	
	// Update is called once per frame
	void LateUpdate () {

      
            if(aimAtTarget) Bone.LookAt(LookAtTarget.position);
            Bone.rotation = Bone.rotation * Quaternion.Euler(Offset);
     
    }
}
