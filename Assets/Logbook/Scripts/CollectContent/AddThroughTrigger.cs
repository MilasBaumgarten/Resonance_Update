using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddThroughTrigger : MonoBehaviour {

    [SerializeField]
    private ObjectContent objCont;

    void OnTriggerEnter(Collider coll)
    {

        if(coll.tag == "Player")
        {

            objCont.Interact(coll.GetComponent<ArmTool>());

        }

    }
}
