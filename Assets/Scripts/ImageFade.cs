using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageFade : MonoBehaviour {

	public float fadeTime = 2f;
	public bool faded;
	private Image image;

	private void Start() {
		image = GetComponent<Image>();
	}

	public void ChangeFaded() {
		faded = !faded;
	}

    public void playAnimation()
    {
        GetComponent<Animation>().Play();
    }
}
