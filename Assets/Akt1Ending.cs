using System.Collections;
using UnityEngine;
using UnityEngine.Events;

using UnityEngine.UI;
using UnityEngine.Video;

public class Akt1Ending : MonoBehaviour {
	[Header("Fade Settings")]
	[SerializeField]
	private GameObject fadeCanvasObject;
	//[SerializeField]
	//private Image fadeImage;
	[SerializeField]
	private Color fadeColor;
	[SerializeField]
	[Range(0.01f, 0.5f)]
	private float fadeTime;

	[Header("Video Settings")]
	[SerializeField]
	[Space]
	private VideoPlayer endingVideo;
	[SerializeField]
	private RawImage videoScreen;

	[SerializeField]
	private UnityEvent afterVideoFinished;

	private float fadeAlpha;
	private bool startEnd = false;

	private void Start() {
		#region Fade Presets
		fadeCanvasObject.SetActive(false);
		fadeAlpha = 0;
		fadeColor.a = fadeAlpha;
		videoScreen.color = fadeColor;
		#endregion
	}

	public void StartEnding() {
		StartCoroutine(Ending());
	}

	private IEnumerator Ending() {
		fadeCanvasObject.SetActive(true);
		float steps = 0.05f;

		while (fadeAlpha < 1) {
			fadeAlpha += steps;
			fadeColor.a = fadeAlpha;
			videoScreen.color = fadeColor;
			yield return new WaitForSeconds(fadeTime);
		}

		endingVideo.Prepare();
		WaitForSeconds wait = new WaitForSeconds(1);

		while (!endingVideo.isPrepared) {
			yield return wait;
			break;
		}

		videoScreen.color = Color.white;
		videoScreen.texture = endingVideo.texture;
		endingVideo.Play();

		yield return new WaitForSeconds((float)endingVideo.clip.length);

		afterVideoFinished.Invoke();

		yield return null;
	}

}
