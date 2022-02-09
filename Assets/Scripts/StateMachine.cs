using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class StateMachine : NetworkBehaviour{

    [SerializeField]
    private Animator anim;
    [SerializeField]
    private LogbookManager logbook;
    [SerializeField]
    private ArmTool armTool;
    [SerializeField]
    private BoneOverride[] boneOverrides;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        if(isLocalPlayer)anim.SetBool("logbook active", logbook.isActive);
        boneOverrides[0].enabled = logbook.isActive;

        //anim.SetBool("armTool active", armTool.armUp);
        boneOverrides[2].enabled = armTool.armUp;
        boneOverrides[1].enabled = armTool.armUp;
        
        //Only for testing purposes
        //if (Input.GetKeyDown(KeyCode.K)) {
        //    anim.SetTrigger("Swipe");
        //}


        //if (Input.GetKeyDown(KeyCode.L))
        //{
        //    anim.SetTrigger("Pointing");
        //}
    }

    public void Swipe()
    {
        anim.SetTrigger("swipe");
    }

    public void Touch()
    {
        anim.SetTrigger("touch");
    }

    //public void UseTool()
    //{
    //    anim.SetTrigger("use Ext");
    //}
}
