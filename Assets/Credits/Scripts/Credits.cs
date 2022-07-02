using Photon.Pun;
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
		if (Input.GetKeyDown(KeyCode.Escape) || (creditAnimation.GetCurrentAnimatorStateInfo(0).IsName("Credits") && creditAnimation.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)) {
			EndCredits();
		}

		if (alphaFloat >= 0) {
			alphaFloat -= Time.deltaTime * transitionSpeed;
			alphaTransition.a = alphaFloat;
			fadeImage.color = alphaTransition;
		}
	}

	public void StartTimerForEndOfCredits(float timerLength) {
		Debug.Log("Starting Timer for: " + timerLength + " seconds");
		Invoke("EndCredits", timerLength);
	}
	
	public void EndCredits() {
		Debug.Log("Ending Credits");
		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;
		PhotonNetwork.Disconnect();
		Destroy(PlayerManager.localPlayerInstance);
		SceneManager.LoadScene(0);	// load main menu
	}
}
