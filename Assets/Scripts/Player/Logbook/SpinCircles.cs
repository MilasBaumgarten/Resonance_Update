// Author: Noah Stolz
// Used to make the circles in the background of the logbook spin
// should be attached to the circle you want to spin


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpinCircles : MonoBehaviour {

    [SerializeField]
    [Tooltip("How fast the circle should spin")]
    private float speed;

    private RectTransform recTrans;

	void Awake () {

        recTrans = this.GetComponent<RectTransform>();

	}
	
	// Update is called once per frame
	void FixedUpdate () {

        Spin();

	}

    public void Spin()
    {
        // Rotate the object by the specified speed
        recTrans.Rotate(new Vector3(0f, 0f, speed));

    }


}
