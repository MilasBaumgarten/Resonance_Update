using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/**
 * Author: Leon Ullrich
 * - controlls text of the info-panel in the canvas
 * - put this script on the info-panel in the canvas
 */

public class OpenInfoPanel : MonoBehaviour {

    public TextMeshProUGUI text;
    public GameObject infoCanvas;

    public GameObject keyPad;

	// Use this for initialization
	void Start () {
        //text.text = keyPad.GetComponent<KeyPad>().targetValue;
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Y)) {
            infoCanvas.SetActive(false);
        }
	}

    public void Open() {
        infoCanvas.SetActive(true);
    }
}
