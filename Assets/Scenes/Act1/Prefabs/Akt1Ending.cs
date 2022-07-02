using Photon.Pun;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class Akt1Ending : MonoBehaviourPun {
	[Header("Fade Settings")]
	[SerializeField]
	private GameObject fadeCanvasObject;
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
	private float holdTimeUntilSkip = 1.0f;
	private float heldTime = 0.0f;

	[SerializeField]
	private string nextScene;

	private float fadeAlpha;

	private bool cutsceneRunning = false;

	private void Start() {
		fadeCanvasObject.SetActive(false);
		fadeAlpha = 0;
		fadeColor.a = fadeAlpha;
		videoScreen.color = fadeColor;
	}

	private void Update() {
		if (!cutsceneRunning) {
			return;
		}

		if (Input.anyKey) {
			if (heldTime >= holdTimeUntilSkip) {
				StopCoroutine(Ending());
				photonView.RPC("RequestSceneChangeRPC", RpcTarget.MasterClient);
			} else {
				heldTime += Time.deltaTime;
			}
		} else {
			if (heldTime > 0) {
				heldTime -= Time.deltaTime;
			}
		}
	}

	public void StartEnding() {
		cutsceneRunning = true;
		PlayerManager.localPlayerInstance.GetComponent<PlayerMovement>().enabled = false;
		PlayerManager.localPlayerInstance.GetComponent<CameraMovement>().enabled = false;
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

		photonView.RPC("RequestSceneChangeRPC", RpcTarget.MasterClient);

		yield return null;
	}

	[PunRPC]
	void RequestSceneChangeRPC() {
		PlayerManager.localPlayerInstance.GetComponent<PlayerMovement>().enabled = true;
		PlayerManager.localPlayerInstance.GetComponent<CameraMovement>().enabled = true;
		SceneManager.LoadScene(nextScene);
	}
}
