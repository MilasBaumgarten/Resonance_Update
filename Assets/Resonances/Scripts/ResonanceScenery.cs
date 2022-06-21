using System.Collections;
using UnityEngine;

public class ResonanceScenery : MonoBehaviour {
	public string dialogueFileName;
	public float fadeWaitValue = 0.01f;
	public bool isDecision;
	public bool isBlocked;

	private Renderer[] renderers;

	[HideInInspector]
	public Material[] materials;

	//[HideInInspector]
	public bool isTriggered;

	[HideInInspector]
	public bool faded = false;

	// Use this for initialization
	void Start() {
		renderers = GetComponentsInChildren<Renderer>();
		materials = new Material[renderers.Length];

		for (int i = 0; i < renderers.Length; i++) {
			materials[i] = renderers[i].material;
		}
	}

	public IEnumerator Fade(Material mat) {
		while (mat.GetColor("_Color").a > 0) {
			Color tmp = mat.GetColor("_Color");
			tmp.a -= fadeWaitValue;
			mat.SetColor("_Color", tmp);

			yield return new WaitForSeconds(fadeWaitValue);
		}

		faded = true;
	}

	public IEnumerator FadeRed(Material mat) {
		while (mat.GetColor("_Emission").b > 0) {
			Color tmp = mat.GetColor("_Emission");
			tmp.b -= fadeWaitValue;
			tmp.g -= fadeWaitValue;
			mat.SetColor("_Emission", tmp);

			yield return new WaitForSeconds(fadeWaitValue);
		}
	}

	public IEnumerator FadeWhite(Material mat) {
		while (mat.GetColor("_Emission").b < 255) {
			Color tmp = mat.GetColor("_Emission");
			tmp.b += fadeWaitValue;
			tmp.g += fadeWaitValue;
			mat.SetColor("_Emission", tmp);

			yield return new WaitForSeconds(fadeWaitValue);
		}
	}

	private void OnTriggerEnter(Collider other) {
		if (!isBlocked) {
			isTriggered = true;
		}
	}
}
