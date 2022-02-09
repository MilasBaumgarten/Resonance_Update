using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForcetoolVisualizer : MonoBehaviour {

	private LineRenderer lineRenderer;
	private ForceModule forceTool;
	private AudioSource aduioSource;

	// Use this for initialization
	void Start() {
		forceTool = GetComponent<ForceModule>();
		lineRenderer = GetComponent<LineRenderer>();
		aduioSource = GetComponent<AudioSource>();
	}

	// Update is called once per frame
	void Update() {
		if (forceTool.GetGrabStatus()) {

			if (!aduioSource.isPlaying) {
				aduioSource.Play();
			}

			lineRenderer.enabled = true;
			lineRenderer.SetPosition(0, transform.position + new Vector3(0, 1, 0));
			lineRenderer.SetPosition(1, forceTool.GetHoldPosition().position);
		} else {
			aduioSource.Pause();
			lineRenderer.enabled = false;
		}
	}
}
