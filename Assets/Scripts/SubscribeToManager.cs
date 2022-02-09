using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubscribeToManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
        if (gameObject.name.ToLower().Contains(GameManager.instance.catrionaName)) {
            GameManager.instance.catriona = gameObject;
        } else if (gameObject.name.ToLower().Contains(GameManager.instance.robertName)) {
            GameManager.instance.robert = gameObject;
        }
	}

}
