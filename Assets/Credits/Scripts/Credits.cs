using Photon.Pun;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UI.ProceduralImage;

public class Credits : MonoBehaviour {

	[SerializeField]
	private Image fadeImage;

	[SerializeField]
	[Range(0.1f, 5.0f)]
	private float transitionSpeed;

	[SerializeField]
	private Animator creditAnimation;

	[SerializeField]
	private ProceduralImage skipVisualisation;

	private Color alphaTransition;
	private float alphaFloat;

	[SerializeField]
	private float holdTimeUntilSkip = 1.0f;
	private float heldTime = 0.0f;

	private void Awake() {
		alphaTransition = fadeImage.color;
		alphaFloat = fadeImage.color.a;

		UpdateSkipVisualisation();

		// disable player cameras and lock players
		foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player")) {
			player.GetComponent<CameraMovement>().playerCamera.SetActive(false);
			player.GetComponent<PlayerMovement>().enabled = false;
			player.GetComponent<OpenCanvas>().enabled = false;
		}
	}

	private void Update() {
		if (Input.anyKey) {
			if (heldTime >= holdTimeUntilSkip) {
				StartCoroutine(EndCredits());
			} else {
				heldTime += Time.deltaTime;
				UpdateSkipVisualisation();
			}
		} else {
			if (heldTime > 0) {
				heldTime -= Time.deltaTime;
				UpdateSkipVisualisation();
			}
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

		if (PhotonNetwork.IsMasterClient) {
			if (PhotonNetwork.CountOfPlayers > 1) {
				PhotonNetwork.SetMasterClient(PhotonNetwork.PlayerListOthers[0]);
			}
		}

		if (PhotonNetwork.IsConnected) {
			PhotonNetwork.Disconnect();
		}

		SceneManager.LoadScene(0);	// load main menu
	}

	private void UpdateSkipVisualisation() {
		skipVisualisation.fillAmount = heldTime / holdTimeUntilSkip;
	}
}
