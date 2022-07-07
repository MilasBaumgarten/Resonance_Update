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
	private string nextScene;

	[SerializeField]
	private AudioSource levelAudio;

	private float fadeAlpha;

	private void Start() {
		fadeCanvasObject.SetActive(false);
		fadeAlpha = 0;
		fadeColor.a = fadeAlpha;
		videoScreen.color = fadeColor;
	}

	public void StartEnding() {
		PlayerManager.localPlayerInstance.GetComponent<PlayerMovement>().enabled = false;
		PlayerManager.localPlayerInstance.GetComponent<CameraMovement>().enabled = false;
		levelAudio.Stop();
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

		photonView.RPC("ReactivatePlayerRPC", RpcTarget.All);
		if (PhotonNetwork.IsMasterClient) {
			SceneManager.LoadScene(nextScene);
		}
	}

	[PunRPC]
	private void ReactivatePlayerRPC() {
		//PlayerManager.localPlayerInstance.transform.position = Vector3.zero;
		PlayerManager.localPlayerInstance.GetComponent<PlayerMovement>().enabled = true;
		PlayerManager.localPlayerInstance.GetComponent<CameraMovement>().enabled = true;
	}
}
