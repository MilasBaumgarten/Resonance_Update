using Photon.Pun;
using System.Collections;
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

		// disable player cameras and lock players
		foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player")) {
			player.GetComponent<CameraMovement>().playerCamera.gameObject.SetActive(false);
			player.GetComponent<PlayerMovement>().enabled = false;
			player.GetComponent<OpenCanvas>().enabled = false;
		}
	}

	private void Update() {
		if (Input.GetKeyDown(KeyCode.Escape)) {
			StartCoroutine(EndCredits());
		}

		if (alphaFloat >= 0) {
			alphaFloat -= Time.deltaTime * transitionSpeed;
			alphaTransition.a = alphaFloat;
			fadeImage.color = alphaTransition;
		}
	}

	public void StartTimerForEndOfCredits(float timerLength) {
		StartCoroutine(EndCredits(timerLength));
	}
	
	public IEnumerator EndCredits(float delay = 0.0f) {
		Debug.Log("Ending Credits after: " + delay + " seconds.");
		yield return new WaitForSeconds(delay);

		Debug.Log("Ending Credits");
		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;

		if (PhotonNetwork.IsConnected){
			PhotonNetwork.Disconnect();
			Destroy(PlayerManager.localPlayerInstance);
		}
		SceneManager.LoadScene(0);	// load main menu
	}
}
