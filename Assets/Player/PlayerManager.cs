using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviourPun {
	[Tooltip("The local player instance. Use this to know if the local player is represented in the Scene")]
	public static GameObject LocalPlayerInstance;

	[Tooltip("The Player's UI GameObject Prefab")]
	[SerializeField]
	public GameObject PlayerUiPrefab;

	[SerializeField]
	private GameObject catrionaModel;
	[SerializeField]
	private GameObject robertModel;

	[SerializeField]
	private Vector3 spawnOffset = new Vector3(0.0f, 0.6f, 0.0f);

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

	private void Start() {
		if (PlayerUiPrefab != null) {
			SetupPlayer();
		} else {
			Debug.LogWarning("<Color=Red><a>Missing</a></Color> PlayerUiPrefab reference on player Prefab.", this);
		}
	}

	void OnSceneLoaded(Scene scene, LoadSceneMode loadingMode) {
		CalledOnLevelWasLoaded(scene.buildIndex);
	}

	void CalledOnLevelWasLoaded(int level) {
		// check if we are outside the Arena and if it's the case, spawn around the center of the arena in a safe zone
		if (!Physics.Raycast(transform.position, -Vector3.up, 5f)) {
			transform.position = new Vector3(0f, 5f, 0f);
		}

		if (PlayerUiPrefab != null) {
			SetupPlayer();
		} else {
			Debug.LogWarning("<Color=Red><a>Missing</a></Color> PlayerUiPrefab reference on player Prefab.", this);
		}
	}

	private void SetupPlayer() {
		GameObject _uiGo = Instantiate(PlayerUiPrefab);
		_uiGo.SendMessage("SetTarget", this, SendMessageOptions.RequireReceiver);

		// set player visuals
		string nickname = photonView.Owner.NickName;
		if (nickname.Equals(CharacterEnum.CATRIONA.ToString())) {
			catrionaModel.SetActive(true);
			robertModel.SetActive(false);
			Debug.LogWarning("<Color=Green><a>Player</a></Color> set to Catriona.");
		} else if (nickname.Equals(CharacterEnum.ROBERT.ToString())) {
			catrionaModel.SetActive(false);
			robertModel.SetActive(true);
			Debug.LogWarning("<Color=Green><a>Player</a></Color> set to Robert.");
		} else {
			Debug.LogWarning("<Color=Red><a>Player</a></Color> nickname: " + nickname + " is unknown.");
		}

		// move player to spawnpoint
		GameObject[] spawners = GameObject.FindGameObjectsWithTag("SpawnPoint");

		foreach (GameObject spawner in spawners) {
			if (spawner.name.ToLower().Contains(nickname.ToLower())) {
				transform.position = spawner.transform.position + spawnOffset;
				break;
			}
		}
	}

	public void OnDisable() {
		SceneManager.sceneLoaded -= OnSceneLoaded;
	}
}
