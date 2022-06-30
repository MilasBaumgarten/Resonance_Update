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

	public bool isTriggered;

	void Awake() {
		renderers = GetComponentsInChildren<Renderer>();
		materials = new Material[renderers.Length];

		for (int i = 0; i < renderers.Length; i++) {
			materials[i] = renderers[i].material;
		}
	}

	public IEnumerator FadeOut() {
		while (materials[0].color.a > 0) {
			foreach (Material mat in materials) {
				Color tmp = mat.color;
				tmp.a -= fadeWaitValue;
				mat.color = tmp;
			}

			yield return new WaitForSeconds(fadeWaitValue);
		}
		gameObject.SetActive(false);
	}

	public IEnumerator FadeIn() {
		foreach (Material mat in materials) {
			// first make fully transparent and then fade in
			Color tmp = mat.color;
			tmp.a = 0;
			mat.color = tmp;
		}

		while (materials[0].color.a < 1) {
			foreach (Material mat in materials) {
				Color tmp = mat.color;
				tmp.a += fadeWaitValue;
				mat.color = tmp;
			}

			yield return new WaitForSeconds(fadeWaitValue);
		}
	}

	private void OnTriggerEnter(Collider other) {
		if (!isBlocked) {
			isTriggered = true;
		}
	}
}
