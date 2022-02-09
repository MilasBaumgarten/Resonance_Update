using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HackingTester : MonoBehaviour {

    public Hacking hacking;
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(KeyCode.J))
        {

            //hacking.DecreaseTime(5);
            print(Hacking.hackingName);

        }

	}
}
