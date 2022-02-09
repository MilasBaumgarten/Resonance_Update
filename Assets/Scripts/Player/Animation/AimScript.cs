using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimScript : MonoBehaviour {

    public Transform LookAtTarget;
    public Vector3 HeadOffset;
    public Vector3 NeckOffset;
    public Vector3 ChestOffset;

    [SerializeField]
    private bool HeadAim;
    [SerializeField]
    private bool NeckAim;
    [SerializeField]
    private bool ChestAim;

    [SerializeField]
    private Animator anim;
    Transform Head;
    Transform Neck;
    Transform Chest;
    // Use this for initialization
    void Start () {

        Head = anim.GetBoneTransform(HumanBodyBones.Head);
        Neck = anim.GetBoneTransform(HumanBodyBones.Neck);
        Chest = anim.GetBoneTransform(HumanBodyBones.Chest);
    }
	
	// Update is called once per frame
	void LateUpdate () {

        if (HeadAim) {
            Head.LookAt(LookAtTarget.position);
            Head.rotation = Head.rotation * Quaternion.Euler(HeadOffset);
        }

        if (NeckAim){
            Neck.LookAt(LookAtTarget.position);
            Neck.rotation = Neck.rotation * Quaternion.Euler(NeckOffset);
        }

        if (ChestAim) {
            Chest.LookAt(LookAtTarget.position);
            Chest.rotation = Chest.rotation * Quaternion.Euler(ChestOffset);
        }
       
    }
}
