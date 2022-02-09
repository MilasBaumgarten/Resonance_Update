using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeCharacters : MonoBehaviour {

	private Renderer[] renderers;
	private Material[] materials;

	public float waitValue = 0.01f;

	// Use this for initialization
	void Start () {
		renderers = GetComponentsInChildren<Renderer>();
		materials = new Material[renderers.Length];

		for (int i = 0; i < renderers.Length; i++) {
			materials[i] = renderers[i].material;
		}
		
		Debug.Log(materials[0].GetColor("_Tint"));
	}

	private void OnTriggerEnter(Collider other) {
		foreach (var mat in materials) {
			StartCoroutine(animate(mat));
		}
	}

	IEnumerator animate(Material mat) {
		while (mat.GetColor("_Tint").a > 0) {
			Color tmp = mat.GetColor("_Tint");
			tmp.a -= waitValue;
			mat.SetColor("_Tint", tmp);

			yield return new WaitForSeconds(waitValue);
		}
	}
}
