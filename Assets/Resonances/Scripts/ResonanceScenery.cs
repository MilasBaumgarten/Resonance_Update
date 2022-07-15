using System.Collections;
using UnityEngine;

public class ResonanceScenery : MonoBehaviour {
	public string dialogueFileName;
	public float timeToFadeIn = 5f, timeToFadeOut = 5f;
	public bool isDecision;
	public bool isBlocked;

	private Renderer[] renderers;

	[HideInInspector]
	public Material[] materials;

	public bool isTriggered;

	private Color[] tmpColors;

	void Awake() {
		renderers = GetComponentsInChildren<Renderer>();
		materials = new Material[renderers.Length];
		tmpColors = new Color[renderers.Length];

		for (int i = 0; i < renderers.Length; i++) {
			materials[i] = renderers[i].material;
		}
	}

	public IEnumerator FadeMaterialColors(bool inOut){
		for(int i = 0; i < materials.Length; i++){
			tmpColors[i] = materials[i].color;
			tmpColors[i].a = inOut ? 0f : 1f; // set full transparent if fading in, full opaque if fading out
			materials[i].color = tmpColors[i];
		}
		
		float timeToFade = inOut ? timeToFadeIn : timeToFadeOut;
		float lerp = 0f;

		if(inOut){
			for(float t = 0f; t < timeToFade; t += Time.deltaTime){
				lerp = t / timeToFade;

				for(int i = 0; i < materials.Length; i++){
					tmpColors[i].a = lerp;
					materials[i].color = tmpColors[i];
				}
				yield return null;
			}
		} else {
			for(float t = timeToFade; t > 0; t -= Time.deltaTime){
				lerp = t / timeToFade;

				for(int i = 0; i < materials.Length; i++){
					tmpColors[i].a = lerp;
					materials[i].color = tmpColors[i];
				}
				yield return null;
			}
		}
	}

	private void OnTriggerEnter(Collider other) {
		if (!isBlocked) {
			isTriggered = true;
		}
	}
}
