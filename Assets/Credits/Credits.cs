using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Credits : MonoBehaviour {

	[SerializeField]
	private Image fadeImage;

	[SerializeField]
	[Range(0.1f, 5.0f)]
	private float transitionSpeed;

	[SerializeField]
	private Animator creditAnimation;

	private Color alphaTransition;
	private float alphaFloat;

	private void Awake() {
		alphaTransition = fadeImage.color;
		alphaFloat = fadeImage.color.a;
	}

	private void Update() {
		if (Input.GetKeyDown(KeyCode.Escape) || (creditAnimation.GetCurrentAnimatorStateInfo(0).IsName("Credits") && creditAnimation.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)) {
			SceneManager.LoadScene("MainMenu");
		}

		if (alphaFloat >= 0) {
			alphaFloat -= Time.deltaTime * transitionSpeed;
			alphaTransition.a = alphaFloat;
			fadeImage.color = alphaTransition;
		}
	}
}
