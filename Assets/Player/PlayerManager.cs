using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviourPun {
	[Tooltip("The local player instance. Use this to know if the local player is represented in the Scene")]
	public static GameObject LocalPlayerInstance;

	void Awake() {
		// #Important
		// used in GameManager.cs: we keep track of the localPlayer instance to prevent instantiation when levels are synchronized
		if (photonView.IsMine) {
			LocalPlayerInstance = gameObject;
		}
		// #Critical
		// we flag as don't destroy on load so that instance survives level synchronization, thus giving a seamless experience when levels load.
		DontDestroyOnLoad(gameObject);

		SceneManager.sceneLoaded += OnSceneLoaded;
	}

	void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode loadingMode) {
		this.CalledOnLevelWasLoaded(scene.buildIndex);
	}

	void CalledOnLevelWasLoaded(int level) {
		// check if we are outside the Arena and if it's the case, spawn around the center of the arena in a safe zone
		if (!Physics.Raycast(transform.position, -Vector3.up, 5f)) {
			transform.position = new Vector3(0f, 5f, 0f);
		}
	}

	public void OnDisable() {
		SceneManager.sceneLoaded -= OnSceneLoaded;
	}
}
